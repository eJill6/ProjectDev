using Google.Authenticator;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.Authenticator;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Authenticator;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWAuthenticatorService : BaseBackSideService, IBWAuthenticatorService
    {
        private static readonly int s_randomKeyLength = 10;
        private static readonly int s_authenticatorExpiredDays = 30;
        private static readonly int s_authenticatorNoticeDays = 5;
        private static readonly AuthenticatorType s_authenticatorType = AuthenticatorType.Google;
        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.UserManagement;

        protected readonly IAppSettingService _appSettingService;

        protected readonly IMerchantSettingService _merchantSettingService;

        private readonly IBWUserAuthenticatorRep _bwUserAuthenticatorRep;

        private readonly IBWUserInfoRep _bwUserInfoRep;

        public BWAuthenticatorService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _appSettingService = ResolveKeyedForModel<IAppSettingService>(envLoginUser.Application);
            _merchantSettingService = ResolveJxBackendService<IMerchantSettingService>(Merchant, dbConnectionType);
            _bwUserAuthenticatorRep = ResolveJxBackendService<IBWUserAuthenticatorRep>();
            _bwUserInfoRep = ResolveJxBackendService<IBWUserInfoRep>();
        }

        public BaseReturnDataModel<BWUserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId)
        {
            BWUserAuthenticator userAuthenticator = GetUserAuthenticator(userId);

            var authenticatorInfo = new BWUserAuthenticatorInfo()
            {
                BWUserAuthenticator = userAuthenticator,
            };

            if (userAuthenticator != null)
            {
                if (userAuthenticator.ExpiredDate.HasValue && userAuthenticator.ExpiredDate.Value < DateTime.Now)
                {
                    authenticatorInfo.BWUserAuthenticatorStatus = BWUserAuthenticatorStatuses.Expired;

                    return new BaseReturnDataModel<BWUserAuthenticatorInfo>(string.Format(MessageElement.SomeKindAuthenticatorExpired,
                        s_authenticatorType.Name), authenticatorInfo);
                }

                authenticatorInfo.BWUserAuthenticatorStatus = BWUserAuthenticatorStatuses.Verified;

                return new BaseReturnDataModel<BWUserAuthenticatorInfo>(ReturnCode.Success, authenticatorInfo);
            }

            authenticatorInfo.BWUserAuthenticatorStatus = BWUserAuthenticatorStatuses.NoVerified;

            return new BaseReturnDataModel<BWUserAuthenticatorInfo>(ReturnCode.UserMustUserAuth, authenticatorInfo);
        }

        public BaseReturnModel IsVerifyCodeValid(ValidVerifyCodeParam validVerifyCodeParam)
        {
            BWUserAuthenticator userAuthenticator = GetUserAuthenticator(validVerifyCodeParam.UserId);
            string accountSecretKey = userAuthenticator.EncryptAccountSecretKey.ToDescryptedData(_appSettingService.CommonDataHash);

            bool isValid = IsGooglePinValid(accountSecretKey, validVerifyCodeParam.VerifyCode, validVerifyCodeParam.IsCompareExactly);

            if (!isValid)
            {
                return new BaseReturnModel(ReturnCode.ValidateCodeIncorrect);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnDataModel<QrCodeViewModel> GetQrCode(CreateQrCodeViewModelParam createQrCodeViewModelParam)
        {
            var qrCodeViewModel = new QrCodeViewModel();
            string accountSecretKey = null;
            BWUserInfo userInfo = _bwUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWUserInfo { UserID = createQrCodeViewModelParam.UserId });

            if (userInfo == null)
            {
                return new BaseReturnDataModel<QrCodeViewModel>(ReturnCode.UserNotFound, qrCodeViewModel);
            }

            if (!createQrCodeViewModelParam.IsForcedRefresh)
            {
                BWUserAuthenticator userAuthenticator = GetUserAuthenticator(createQrCodeViewModelParam.UserId);

                if (userAuthenticator == null)
                {
                    createQrCodeViewModelParam.IsForcedRefresh = true;
                }
                else
                {
                    accountSecretKey = userAuthenticator.EncryptAccountSecretKey.ToDescryptedData(_appSettingService.CommonDataHash);
                    qrCodeViewModel.UpdateDate = userAuthenticator.UpdateDate;
                }
            }

            if (createQrCodeViewModelParam.IsForcedRefresh)
            {
                accountSecretKey = CreateNewUserSecretKey(
                    createQrCodeViewModelParam.UserId,
                    s_authenticatorExpiredDays,
                    (secretKey) =>
                    {
                        return secretKey.ToEncryptedData(_appSettingService.CommonDataHash);
                    });

                qrCodeViewModel.UpdateDate = DateTime.Now;

                CreateOperationLog(PermissionElement.Update, userInfo.UserName);
            }

            SetupCode setupCode = GetSetupCode(new CreateQrCodeImageParam()
            {
                Application = EnvLoginUser.Application,
                AccountSecretKey = accountSecretKey,
                AccountTitleNoSpaces = userInfo.UserName
            });

            qrCodeViewModel.ImageUrl = setupCode.QrCodeSetupImageUrl;
            qrCodeViewModel.DisplayManualEntryKey = ConvertToDisplayManualEntryKey(setupCode.ManualEntryKey);
            qrCodeViewModel.UserID = createQrCodeViewModelParam.UserId;

            return new BaseReturnDataModel<QrCodeViewModel>(new SuccessMessage(MessageElement.UpdateSuccess), qrCodeViewModel);
        }

        public BaseReturnModel CheckVerificationExpiring(int userId)
        {
            BWUserAuthenticator bwUserAuthenticator = GetUserAuthenticator(userId);

            if (bwUserAuthenticator == null)
            {
                return new BaseReturnModel(ReturnCode.AuthenticatorUnverified);
            }

            DateTime expirationTime = DateTime.Now.AddDays(s_authenticatorNoticeDays);

            if (bwUserAuthenticator.ExpiredDate < expirationTime)
            {
                return new BaseReturnModel(ReturnCode.GoogleAuthExpiryNotice);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        #region Private Method

        private BWUserAuthenticator GetUserAuthenticator(int userId)
        {
            return _bwUserAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb, new BWUserAuthenticator() { UserID = userId }, false);
        }

        private bool IsGooglePinValid(string accountSecretKey, string verifyCode, bool isCompareExactly)
        {
            //這邊不使用套件的驗證,測試沒有緩衝的效果
            //current會在[10], [9]會是過期的前一個
            //這邊取得兩個讓用戶最少有30秒的緩衝

            const int currentPinIndex = 10;
            string[] allPins = new TwoFactorAuthenticator().GetCurrentPINs(accountSecretKey);
            List<string> filterPins = new List<string>();
            filterPins.Add(allPins[currentPinIndex]);

            if (!isCompareExactly)
            {
                filterPins.Add(allPins[currentPinIndex - 1]);
            }

            return filterPins.Any(a => a == verifyCode);
        }

        private string CreateNewUserSecretKey(int userId, int keyExpiredDays,
            Func<string, string> getEncryptKey)
        {
            string secretKey = StringUtil.CreateRandomString(s_randomKeyLength);
            BWUserAuthenticator userAuthenticator = _bwUserAuthenticatorRep.GetSingleByKey(InlodbType.Inlodb,
                new BWUserAuthenticator() { UserID = userId, }, false);

            if (userAuthenticator != null)
            {
                userAuthenticator.EncryptAccountSecretKey = getEncryptKey.Invoke(secretKey);
                userAuthenticator.ExpiredDate = DateTime.Now.AddDays(keyExpiredDays);
                userAuthenticator.UpdateUser = EnvLoginUser.LoginUser.UserId.ToString();

                _bwUserAuthenticatorRep.UpdateByProcedure(userAuthenticator);
            }
            else
            {
                userAuthenticator = new BWUserAuthenticator()
                {
                    UserID = userId,
                    AuthenticatorType = s_authenticatorType.Value,
                    EncryptAccountSecretKey = getEncryptKey(secretKey),
                    ExpiredDate = DateTime.Now.AddDays(keyExpiredDays)
                };

                _bwUserAuthenticatorRep.CreateByProcedure(userAuthenticator);
            }

            return secretKey;
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

        private SetupCode GetSetupCode(CreateQrCodeImageParam createQrCodeImageParam)
        {
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            string issuer = _merchantSettingService.GoogleAuthenticatorIssuer;

            if (createQrCodeImageParam.EnvironmentCode != EnvironmentCode.Production)
            {
                issuer += $"({createQrCodeImageParam.EnvironmentCode.Value})";
            }

            bool secretIsBase32 = false;

            SetupCode setupCode = twoFactorAuthenticator.GenerateSetupCode(
                issuer,
                createQrCodeImageParam.AccountTitleNoSpaces,
                createQrCodeImageParam.AccountSecretKey, secretIsBase32);

            return setupCode;
        }

        private void CreateOperationLog(string actionName, string userName)
        {
            string content = string.Format(BWOperationLogElement.UpdateGoogleAuthenticatorMessage,
                _permissionKey.Name,
                actionName,
                userName);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        #endregion Private Method
    }
}