using Google.Authenticator;
using IPToolModel;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Client;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Model.Enums;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JxBackendServiceNet45.Service.Authenticator
{
    public class UserAuthenticatorValidService : BaseService, IUserAuthenticatorValidService, IUserAuthenticatorValidReadService
    {
        private static readonly int _randomKeyLength = 10;
        private static readonly int _warningExpiredAuthenticatorDays = 5;
        private readonly IUserInfoRep _userInfoRep;
        private readonly IUserAuthenticatorRep _userAuthenticatorRep;
        private readonly IAppSettingService _appSettingService;
        private readonly IOperationLogService _operationLogService;
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;
        private readonly IConfigService _configService;
        private readonly IFailureOperationService _failureOperationService;

        public UserAuthenticatorValidService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _userAuthenticatorRep = ResolveJxBackendService<IUserAuthenticatorRep>();
            _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _operationLogService = ResolveJxBackendService<IOperationLogService>();
            _userInfoRelatedReadService = ResolveJxBackendService<IUserInfoRelatedReadService>();
            _configService = ResolveJxBackendService<IConfigService>();
            _failureOperationService = ResolveJxBackendService<IFailureOperationService>();
        }

        public QrCodeViewModel GetQrCode(CreateQrCodeViewModelParam createQrCodeViewModelParam)
        {
            var qrCodeViewModel = new QrCodeViewModel();
            string accountSecretKey = null;
            AuthenticatorType authenticatorType = createQrCodeViewModelParam.CreateQrCodeWithType;

            if (!createQrCodeViewModelParam.IsForcedRefresh)
            {
                UserAuthenticator userAuthenticator = GetUserAuthenticator(createQrCodeViewModelParam.SearchUser.UserId);

                if (userAuthenticator == null)
                {
                    createQrCodeViewModelParam.IsForcedRefresh = true;
                }
                else
                {
                    accountSecretKey = userAuthenticator.EncryptAccountSecretKey.ToDescryptedData(_appSettingService.CommonDataHash);
                    //用當初用戶設定的驗證方式
                    authenticatorType = AuthenticatorType.GetSingle(userAuthenticator.AuthenticatorType);
                    qrCodeViewModel.UpdateDate = userAuthenticator.UpdateDate;
                }
            }

            if (createQrCodeViewModelParam.IsForcedRefresh)
            {
                accountSecretKey = CreateNewUserSecretKey(
                    createQrCodeViewModelParam.SearchUser.UserId,
                    createQrCodeViewModelParam.CreateQrCodeWithType,
                    _appSettingService.AuthenticatorExpiredDays,
                    (secretKey) =>
                    {
                        return secretKey.ToEncryptedData(_appSettingService.CommonDataHash);
                    });

                qrCodeViewModel.UpdateDate = DateTime.Now;
            }

            IAuthenticatorService authenticatorService = GetAuthenticatorService(authenticatorType);

            SetupCode setupCode = authenticatorService.GetSetupCode(new CreateQrCodeImageParam()
            {
                Application = createQrCodeViewModelParam.Application,
                AccountSecretKey = accountSecretKey,
                AccountTitleNoSpaces = createQrCodeViewModelParam.SearchUser.UserName
            });

            qrCodeViewModel.ImageUrl = setupCode.QrCodeSetupImageUrl;
            qrCodeViewModel.DisplayManualEntryKey = ConvertToDisplayManualEntryKey(setupCode.ManualEntryKey);

            return qrCodeViewModel;
        }

        public BaseReturnModel IsUserTwoFactorPinValid(int userId, UserAuthenticatorSettingTypes userAuthenticatorSettingType, string clientPin)
        {
            AuthenticatorPermission authenticatorPermission = GetAuthenticatorPermission(userId, userAuthenticatorSettingType);

            if (!authenticatorPermission.IsAllowExecuted)
            {
                return new BaseReturnModel(authenticatorPermission.ErrorMessage);
            }

            if (authenticatorPermission.IsAuthenticatorRequired)
            {
                BaseReturnModel validResult = GetTwoFactorPinValidResult(authenticatorPermission.UserAuthenticatorInfo.UserAuthenticator, clientPin);

                if (!validResult.IsSuccess)
                {
                    return validResult;
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 單純驗證pin
        /// </summary>
        public BaseReturnModel IsUserTwoFactorPinValid(int userId, string clientPin)
        {
            if (!Regex.IsMatch(clientPin, RegularExpressionType.DynamicPassword.Pattern))
            {
                return new BaseReturnModel(UserAuthElement.GoogleAuthFail);
            }

            BaseReturnDataModel<UserAuthenticatorInfo> userAuthenticatorInfo = GetUserAuthenticatorInfo(userId);
            string authenticatorTypeName = null;

            if (userAuthenticatorInfo.DataModel.UserAuthenticator != null)
            {
                AuthenticatorType authenticatorType = AuthenticatorType.GetSingle(userAuthenticatorInfo.DataModel.UserAuthenticator.AuthenticatorType);
                authenticatorTypeName = GetAuthenticatorTypeName(authenticatorType);
            }

            string defaultErrorMsg = string.Format(MessageElement.SomeKindPinIsNotValid, authenticatorTypeName);

            if (!userAuthenticatorInfo.IsSuccess)
            {
                string errorMsg;

                if (userAuthenticatorInfo.DataModel.UserAuthenticatorStatus == UserAuthenticatorStatuses.Expired)
                {
                    errorMsg = userAuthenticatorInfo.Message;
                }
                else
                {
                    errorMsg = defaultErrorMsg;
                }

                return new BaseReturnModel(errorMsg);
            }

            BaseReturnModel validResult = GetTwoFactorPinValidResult(userAuthenticatorInfo.DataModel.UserAuthenticator, clientPin);

            if (!validResult.IsSuccess)
            {
                return validResult;
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private BaseReturnModel GetTwoFactorPinValidResult(UserAuthenticator userAuthenticator, string clientPin)
        {
            AuthenticatorType authenticatorType = AuthenticatorType.GetSingle(userAuthenticator.AuthenticatorType);

            if (clientPin.IsNullOrEmpty())
            {
                return new BaseReturnModel(string.Format(MessageElement.SomeKindPinIsEmpty, authenticatorType.Name));
            }

            string accountSecretKey = userAuthenticator.EncryptAccountSecretKey.ToDescryptedData(_appSettingService.CommonDataHash);
            bool isValid = IsTwoFactorPinValid(authenticatorType, accountSecretKey, clientPin);

            if (!isValid)
            {
                return new BaseReturnModel(string.Format(MessageElement.SomeKindPinIsNotValid, authenticatorType.Name));
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 給後台公開使用與內部方法驗證用
        /// </summary>        
        public bool IsTwoFactorPinValid(AuthenticatorType authenticatorType, string accountSecretKey, string twoFactorCodeFromClient)
        {
            IAuthenticatorService authenticatorService = GetAuthenticatorService(authenticatorType);
            return authenticatorService.IsPinValid(accountSecretKey, twoFactorCodeFromClient, _appSettingService.IsClientPinCompareExactly);
        }

        public string GetAuthenticatorExpiredWarningMsg(int userId)
        {
            UserAuthenticator userAuthenticator = GetUserAuthenticator(userId);
            string warningMessag = null;

            if (userAuthenticator == null)
            {
                return warningMessag;
            }

            if (userAuthenticator.ExpiredDate.HasValue &&
                userAuthenticator.ExpiredDate.Value.Subtract(DateTime.Now).TotalDays <= _warningExpiredAuthenticatorDays)
            {
                warningMessag = MessageElement.GoogleAuthenticatorExpiredWarning;
            }

            return warningMessag;
        }

        public UserAuthenticator GetUserAuthenticator(int userId)
        {
            return _userAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb, new UserAuthenticator() { UserID = userId }, false);
        }

        public UserAuthenticator GetUserAuthenticator(string userName)
        {
            UserInfo userInfo = _userInfoRep.GetUserInfo(userName);

            if (userInfo == null)
            {
                return null;
            }

            return _userAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb, new UserAuthenticator() { UserID = userInfo.UserID }, false);
        }

        /// <summary>
        /// 前台判斷是否有綁定的Authenticator
        /// </summary>
        public BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId)
        {
            UserAuthenticator userAuthenticator = GetUserAuthenticator(userId);

            if (userAuthenticator != null)
            {
                if (userAuthenticator.ExpiredDate.HasValue && userAuthenticator.ExpiredDate.Value < DateTime.Now)
                {
                    return new BaseReturnDataModel<UserAuthenticatorInfo>(string.Format(MessageElement.SomeKindAuthenticatorExpired,
                        AuthenticatorType.GetSingle(userAuthenticator.AuthenticatorType).Name),
                        new UserAuthenticatorInfo()
                        {
                            UserAuthenticator = userAuthenticator,
                            UserAuthenticatorStatus = UserAuthenticatorStatuses.Expired
                        });
                }

                if (userAuthenticator.IsVerified == true)
                {
                    return new BaseReturnDataModel<UserAuthenticatorInfo>(ReturnCode.Success,
                        new UserAuthenticatorInfo()
                        {
                            UserAuthenticator = userAuthenticator,
                            UserAuthenticatorStatus = UserAuthenticatorStatuses.Verified
                        });
                }
            }

            return new BaseReturnDataModel<UserAuthenticatorInfo>(ReturnCode.UserMustUserAuth,
                new UserAuthenticatorInfo()
                {
                    UserAuthenticator = userAuthenticator,
                    UserAuthenticatorStatus = UserAuthenticatorStatuses.NoVerified
                });
        }

        /// <summary>
        /// 綁定/解除綁定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseReturnModel VerifyAuthenticator(VerifiedAuthenticatorParam param)
        {
            if (param.MoneyPasswordHash.IsNullOrEmpty())
            {
                return new BaseReturnModel(MessageElement.PlzInputMoneyPassword);
            }

            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = param.User.UserId });

            //先把null排除避免後面一直在判斷
            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (!userInfo.IsActive)
            {
                return new BaseReturnModel(ReturnCode.YourAccountIsDisabled);
            }

            UserAuthenticator userAuthenticator = _userAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb,
                new UserAuthenticator() { UserID = param.User.UserId });
            AuthenticatorType authenticatorType = AuthenticatorType.GetSingle(userAuthenticator.AuthenticatorType);

            if (userAuthenticator == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (param.Pin.IsNullOrEmpty())
            {
                return new BaseReturnModel(string.Format(MessageElement.PlzInputSomeKindPin, authenticatorType.Name));
            }

            if (userAuthenticator.ExpiredDate.HasValue && userAuthenticator.ExpiredDate.Value < DateTime.Now)
            {
                LogUtil.ForcedDebug($"Authenticator過期, {new { param.User.UserId, userAuthenticator.ExpiredDate }.ToJsonString()}");
                return new BaseReturnModel(ReturnCode.GoogleAuthenticatorExpired);
            }

            string accountSecretKey = userAuthenticator.EncryptAccountSecretKey.ToDescryptedData(_appSettingService.CommonDataHash);

            if (!IsTwoFactorPinValid(authenticatorType, accountSecretKey, param.Pin))
            {
                LogUtil.ForcedDebug($"IsTwoFactorPinValid失敗, {new { param.User.UserId, param.Pin }.ToJsonString()}");
                return new BaseReturnModel(string.Format(MessageElement.SomeKindPinIsNotValid, authenticatorType.Name));
            }

            //驗證資金密碼
            if (userInfo.MoneyPwd.IsNullOrEmpty() ||
                userInfo.MoneyPwd != param.MoneyPasswordHash)
            {
                if (userInfo.MoneyPwd != param.MoneyPasswordHash)
                {
                    string webActionTypeNameParam = SelectItemElement.Verify_False;

                    if (param.IsVerified)
                    {
                        webActionTypeNameParam = SelectItemElement.Verify_True;
                    }

                    _failureOperationService.AddFailOperation(WebActionType.BindAuthenticator, SubActionType.MoneyPasswordWrong, webActionTypeNameParam);
                }

                LogUtil.ForcedDebug($"资金密码错误，错误次数过多，会导致账号被锁定, {new { param.User.UserId, param.MoneyPasswordHash }.ToJsonString()}");
                return new BaseReturnModel(ReturnCode.YourMoneyPasswordIsWrong);
            }

            if (userInfo.MoneyPwdExpiredDate.IsTimeExpired())
            {
                LogUtil.ForcedDebug($"資金密碼過期, {new { param.User.UserId, param.MoneyPasswordHash }.ToJsonString()}");
                return new BaseReturnModel(ReturnCode.MoneyPasswordExpired);
            }

            //更新驗證狀態
            userAuthenticator.IsVerified = param.IsVerified;

            if (_userAuthenticatorRep.UpdateByProcedure(userAuthenticator))
            {
                _failureOperationService.ClearCount(WebActionType.BindAuthenticator, SubActionType.MoneyPasswordWrong);

                //新增操作紀錄
                BaseReturnDataModel<long> saveOperationLogResult = _operationLogService.InsertFrontSideOperationLog(new InsertFrontSideOperationLogParam()
                {
                    AffectedUserId = param.User.UserId,
                    AffectedUserName = param.User.UserName,
                    Content = GetVerifyCompletedText(authenticatorType, param.IsVerified)
                });

                if (saveOperationLogResult.IsSuccess)
                {
                    return CreateVerifySuccessReturnModel(authenticatorType, param.IsVerified);
                }
                else
                {
                    return saveOperationLogResult;
                }
            }

            return new BaseReturnModel(ReturnCode.OperationFailed);
        }

        /// <summary>
        /// 強迫解綁, 適用後台功能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseReturnModel ForceUnverifiedAuthenticator(string userName)
        {
            UserInfo userInfo = _userInfoRep.GetUserInfo(userName);
            UserAuthenticator userAuthenticator = _userAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb,
                new UserAuthenticator() { UserID = userInfo.UserID });

            if (userAuthenticator == null || userAuthenticator.IsVerified != true)
            {
                return new BaseReturnModel(MessageElement.UserHasNotSetAuthenticator);
            }

            //更新驗證狀態
            userAuthenticator.IsVerified = false;

            if (_userAuthenticatorRep.UpdateByProcedure(userAuthenticator))
            {
                //新增操作紀錄
                BaseReturnDataModel<long> saveOperationLogResult = _operationLogService.InsertModifyMemberOperationLog(new InsertModifyMemberOperationLogParam()
                {
                    Category = JxOperationLogCategory.Member,
                    AffectedUserId = userInfo.UserID,
                    AffectedUserName = userInfo.UserName,
                    Content = MessageElement.UnverifiedAuthenticatorByBackSideWebSuccessfully
                });

                if (saveOperationLogResult.IsSuccess)
                {
                    return new BaseReturnModel(new SuccessMessage() { Text = MessageElement.OperationSuccess });
                }
                else
                {
                    return saveOperationLogResult;
                }
            }

            return new BaseReturnModel(ReturnCode.OperationFailed);
        }

        public AuthenticatorPermission GetAuthenticatorPermission(int userId, UserAuthenticatorSettingTypes userAuthenticatorSettingType)
        {
            //是否檢查二次驗證
            bool isValidUserAuthenticator = true;

            var authenticatorPermission = new AuthenticatorPermission()
            {
                IsAllowExecuted = true,
                IsAuthenticatorRequired = false,
                IsShowAuthenticatorConfirm = false
            };

            ClientAppPage redirectPageAfterCancel = null;
            switch (userAuthenticatorSettingType)
            {
                case UserAuthenticatorSettingTypes.TransferToChild:
                    //如果是下線轉帳,要額外判斷ConfigGroup開關
                    UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = userId });

                    if (userInfo == null ||
                        userInfo.IsOutMoney != true ||
                        userInfo.IslowMoneyIn != true ||
                        !_configService.IsActive(ConfigGroupNameEnum.ParentToSonTransfer))
                    {
                        authenticatorPermission.IsAllowExecuted = false;
                        authenticatorPermission.SetBehaviorErrorDialog(MessageElement.TransferToChildIsDisabled, null);

                        return authenticatorPermission;
                    }

                    break;
                case UserAuthenticatorSettingTypes.Withdraw:
                case UserAuthenticatorSettingTypes.ModifyUSDTAccount:
                    // 一般提現&usdt判斷綁定銀行卡
                    List<UserBankInfo> userBankList = _userInfoRep.GetUserBankInfos(userId);

                    if (!userBankList.Any())
                    {
                        authenticatorPermission.IsAllowExecuted = false;

                        authenticatorPermission.SetBehaviorConfirmDialog(
                            MessageElement.PlzBindBankCard,
                            string.Empty,
                            CommonElement.SettingRightNow,
                            CommonElement.TalkAboutItNextTime,
                            ClientAppPage.BindBankCard,
                            redirectPageAfterCancel);

                        return authenticatorPermission;
                    }

                    break;
                case UserAuthenticatorSettingTypes.MoneyPassword:
                    isValidUserAuthenticator = false;
                    break;
            }

            //必要檢查  檢查是否需要重設資金密碼
            if (_userInfoRelatedReadService.IsMoneyPasswordExpired(userId))
            {
                authenticatorPermission.IsAllowExecuted = false;

                authenticatorPermission.SetBehaviorConfirmDialog(
                    ReturnCode.UserMustResetMoneyPwd.Name,
                    MessageElement.ResetMoneyPasswordDescription,
                    CommonElement.ResetRightNow,
                    CommonElement.TalkAboutItNextTime,
                    ClientAppPage.ModifyMoneyPassword,
                    redirectPageAfterCancel);

                return authenticatorPermission;
            }

            if (isValidUserAuthenticator)
            {
                //檢查設定檔
                bool isForcedValidUserAuthenticator = _configService.IsActive(ConfigGroupNameEnum.GoogleVerify, (int)userAuthenticatorSettingType);
                BaseReturnDataModel<UserAuthenticatorInfo> returnDataModel = GetUserAuthenticatorInfo(userId);
                authenticatorPermission.UserAuthenticatorInfo = returnDataModel.DataModel;

                if (returnDataModel.DataModel.UserAuthenticatorStatus == UserAuthenticatorStatuses.Expired)
                {
                    authenticatorPermission.IsAllowExecuted = false;
                    authenticatorPermission.SetBehaviorErrorDialog(returnDataModel.Message, null);

                    return authenticatorPermission;
                }

                //強制啟用才需要檢查是否綁定
                if (isForcedValidUserAuthenticator && !returnDataModel.IsSuccess)
                {
                    authenticatorPermission.IsAllowExecuted = false;
                    authenticatorPermission.IsShowAuthenticatorConfirm = true;

                    authenticatorPermission.SetBehaviorConfirmDialog(
                        returnDataModel.Message,
                        MessageElement.GoogleAuthenticatorDescription,
                        CommonElement.BindRightNow,
                        CommonElement.TalkAboutItNextTime,
                        ClientAppPage.BindAuthenticator,
                        null);

                    return authenticatorPermission;
                }

                if (returnDataModel.IsSuccess)
                {
                    authenticatorPermission.IsAuthenticatorRequired = true;
                }
            }

            return authenticatorPermission;
        }

        private string CreateNewUserSecretKey(int userId, AuthenticatorType authenticatorType, int keyExpiredDays,
            Func<string, string> getEncryptKey)
        {
            string secretKey = StringUtil.CreateRandomString(_randomKeyLength);
            UserAuthenticator userAuthenticator = _userAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb,
                new UserAuthenticator() { UserID = userId, });

            if (userAuthenticator != null)
            {
                userAuthenticator.EncryptAccountSecretKey = getEncryptKey.Invoke(secretKey);
                userAuthenticator.ExpiredDate = DateTime.Now.AddDays(keyExpiredDays);
                userAuthenticator.IsVerified = null;
                _userAuthenticatorRep.UpdateByProcedure(userAuthenticator);
            }
            else
            {
                _userAuthenticatorRep.CreateByProcedure(new UserAuthenticator()
                {
                    UserID = userId,
                    AuthenticatorType = authenticatorType.Value,
                    EncryptAccountSecretKey = getEncryptKey(secretKey),
                    ExpiredDate = DateTime.Now.AddDays(keyExpiredDays)
                });
            }

            return secretKey;
        }

        private IAuthenticatorService GetAuthenticatorService(AuthenticatorType authenticatorType)
        {
            return DependencyUtil.ResolveKeyed<IAuthenticatorService>(()=>authenticatorType.Value);
        }

        private string ConvertToDisplayManualEntryKey(string manualEntryKey)
        {
            //每4個空一格
            const int splitLength = 4;
            char[] chars = manualEntryKey.ToCharArray();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i % splitLength == 0)
                {
                    result.Append(" ");
                }

                result.Append(chars[i].ToString());
            }

            return result.ToString().Trim();
        }

        private BaseReturnModel CreateVerifySuccessReturnModel(AuthenticatorType authenticatorType, bool isVerified)
        {
            var returnModel = new BaseReturnModel(ReturnCode.Success);
            returnModel.Message = GetVerifyCompletedText(authenticatorType, isVerified);
            return returnModel;
        }

        private string GetVerifyCompletedText(AuthenticatorType authenticatorType, bool isVerified)
        {
            string verifyText = null;

            if (isVerified)
            {
                verifyText = SelectItemElement.Verify_True;
            }
            else
            {
                verifyText = SelectItemElement.Verify_False;
            }

            return string.Format(MessageElement.SomeKindAuthenticatorVerifyCompleted, GetAuthenticatorTypeName(authenticatorType), verifyText);
        }

        private string GetAuthenticatorTypeName(AuthenticatorType authenticatorType)
        {
            string authenticatorTypeName = null;

            if (authenticatorType != null)
            {
                authenticatorTypeName = authenticatorType.Name;
            }
            else
            {
                authenticatorTypeName = AuthenticatorType.GetAll().First().Name;
            }

            return authenticatorTypeName;
        }
    }
}
