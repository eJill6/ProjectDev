using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IPToolModel;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class UserInfoRelatedService : BaseService, IUserInfoRelatedService, IUserInfoRelatedReadService
    {
        private readonly string _oldPasswordWrong = "旧密码错误";
        private readonly IUserInfoRep _userInfoRep;
        private readonly Lazy<IAuditInfoService> _auditInfoService;
        private readonly IUserInfoAdditionalRep _userInfoAdditionalRep;
        private readonly IUserLevelRep _userLevelRep;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IJxCacheService _jxCacheService;
        private readonly IAppSettingService _appSettingService;
        private readonly Lazy<IBlockActionService> _blockActionService;
        private readonly IFailureLoginHistoryRep _failureLoginHistoryRep;
        private readonly IFailureOperationService _failureOperationService;
        private readonly IConfigService _configService;
        private readonly IDeviceService _deviceService;
        private readonly string _emptySymbol = "-";
        private readonly int _maxNicknameLength = 15;

        public UserInfoRelatedService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _auditInfoService = new Lazy<IAuditInfoService>(() => ResolveJxBackendService<IAuditInfoService>());
            _userInfoAdditionalRep = ResolveJxBackendService<IUserInfoAdditionalRep>();
            _userLevelRep = ResolveJxBackendService<IUserLevelRep>();
            _operationLogService = new Lazy<IOperationLogService>(() => ResolveJxBackendService<IOperationLogService>());
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _blockActionService = new Lazy<IBlockActionService>(() => ResolveJxBackendService<IBlockActionService>());
            _failureLoginHistoryRep = ResolveJxBackendService<IFailureLoginHistoryRep>();
            _failureOperationService = ResolveJxBackendService<IFailureOperationService>();
            _configService = ResolveJxBackendService<IConfigService>();
            _deviceService = ResolveKeyed<IDeviceService>(envLoginUser.Application);
        }

        /// <summary>
        /// 驗證用戶是否有綁定銀行卡
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userDataType"></param>
        /// <returns></returns>
        public BaseReturnModel CheckUserBankHasActive(int userID, ModifyUserDataTypes userDataType)
        {
            if (userDataType == ModifyUserDataTypes.UserEmail)
            {
                bool isInitializeUser = _userInfoRep.IsInitializeUser(userID);

                if (!isInitializeUser)
                {
                    return new BaseReturnModel(ReturnCode.UserInitializeIncomplete);
                }
            }

            List<UserBankInfo> userBankInfos = _userInfoRep.GetUserBankInfos(userID);

            if (userBankInfos.Count == 0)
            {
                return new BaseReturnModel(ReturnCode.UserUnboundBankCard);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 傳入用戶銀行信息為綁定狀態且是否驗證成功
        /// </summary>
        public BaseReturnDataModel<string> ValidationUserBank(UserVaildBankParam userVaildBankParam)
        {
            if (userVaildBankParam.BankCard.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(string.Format(MessageElement.FieldIsNotAllowEmpty, UserRelatedElement.BankCard), string.Empty);
            }

            if (userVaildBankParam.CardUser.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(string.Format(MessageElement.FieldIsNotAllowEmpty, UserRelatedElement.CardUser), string.Empty);
            }

            List<UserBankInfo> userBankInfos = _userInfoRep.GetUserBankInfos(userVaildBankParam.UserID);

            if (userBankInfos.Count == 0)
            {
                return new BaseReturnDataModel<string>(ReturnCode.UserUnboundBankCard);
            }

            UserBankInfo bankInfo = userBankInfos
                .Where(w => w.BankTypeID == userVaildBankParam.BankTypeID &&
                            w.BankCard == userVaildBankParam.BankCard &&
                            string.Equals(w.CardUser, userVaildBankParam.CardUser, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (bankInfo == null)
            {
                return new BaseReturnDataModel<string>(ReturnCode.UserVaildFailed);
            }

            string maskContent = GetUserModifyContent(userVaildBankParam.UserID, userVaildBankParam.ModifyUserDataType);

            return new BaseReturnDataModel<string>(ReturnCode.Success, maskContent);
        }

        public bool CheckUserIdInUserPath(int loginUserId, int searchUserID)
        {
            if (loginUserId == searchUserID)
            {
                return true;
            }

            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = searchUserID });

            if (userInfo == null)
            {
                return false;
            }

            userInfo.UserPaths = userInfo.UserPaths + "/" + searchUserID;
            userInfo.UserPaths = userInfo.UserPaths.Replace("//", "/");
            return userInfo.UserPaths.Split('/').Contains(loginUserId.ToString());
        }

        #region 遮罩手機號碼或EMAIL
        public string GetUserModifyContent(int userId, ModifyUserDataTypes userDataType)
        {
            if (userDataType == ModifyUserDataTypes.UserEmail)
            {
                return GetMaskUserEmail(userId);
            }

            return string.Empty;
        }

        private string GetUserEmail(int userId)
        {
            string encryptContent = _userInfoRep.GetUserEmail(userId);

            if (encryptContent.IsNullOrEmpty())
            {
                return _emptySymbol;
            }

            return encryptContent.ToDescryptedEmail(_appSettingService.EmailHash);
        }

        public string GetMaskUserEmail(int userId)
        {
            return GetUserEmail(userId).ToMaskEmail();
        }

        private string GetUserPhoneNumber(int userId, string phoneHash)
        {
            string encryptContent = _userInfoRep.GetUserBindPhoneNumber(userId);

            if (encryptContent.IsNullOrEmpty())
            {
                return _emptySymbol;
            }

            return encryptContent.ToDescryptedPhone(phoneHash).ToMaskPhoneNumber();
        }

        private string GetMaskUserPhoneNumber(int userId, string phoneHash)
        {
            return GetUserPhoneNumber(userId, phoneHash).ToMaskPhoneNumber();
        }
        #endregion

        /// <summary>
        /// 修改用戶資料。回傳string因為有function會回傳泛型資料，WCF必須對齊不然會發生錯誤
        /// </summary>
        public BaseReturnModel ModifyUserDataContent(UserModifyDataParam userModifyDataParam)
        {
            if (userModifyDataParam.Memo.IsNullOrEmpty())
            {
                return new BaseReturnModel(string.Format(UserRelatedElement.PleaseInputData, UserRelatedElement.MemoContent));
            }

            #region 檢查是否為合法的資料型態
            if (userModifyDataParam.ModifyUserDataType != ModifyUserDataTypes.UserEmail &&
                userModifyDataParam.ModifyUserDataType != ModifyUserDataTypes.LoginPassword &&
                userModifyDataParam.ModifyUserDataType != ModifyUserDataTypes.MoneyPassword)
            {
                return new BaseReturnDataModel<string>(ReturnCode.OperationFailed);
            }
            #endregion

            #region 檢查修改內容是否合法 & 資料處理
            AuditTypeValue auditType = null;
            PasswordType passwordType = null;
            var checkModifyContent = new BaseReturnDataModel<string>(ReturnCode.Success);
            string oldEncryptContent = string.Empty;
            string newContent = string.Empty;
            string newEncryptContent = string.Empty;
            string auditPassReturnMessage = string.Empty;
            string addtionalAuditValue = string.Empty;

            bool isCheckBankCard = true; // 是否需要檢查銀行卡資訊 預設為需要

            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = userModifyDataParam.UserID });

            if (userModifyDataParam.ModifyUserDataType == ModifyUserDataTypes.UserEmail)
            {
                auditType = AuditTypeValue.Email;

                userModifyDataParam.BeforeContent = userInfo.Email;
                oldEncryptContent = GetUserEncryptContent(userModifyDataParam.BeforeContent, userModifyDataParam.ModifyUserDataType);

                if (userModifyDataParam.IsClearMail == false)
                {
                    checkModifyContent = VaildModifyEmail(userModifyDataParam.ModifyContent);
                    newContent = userModifyDataParam.ModifyContent;
                    newEncryptContent = checkModifyContent.DataModel;
                }
                else
                {
                    newContent = UserRelatedElement.ClearContent;
                }

                addtionalAuditValue = userModifyDataParam.ModifyContent;
            }
            else if (userModifyDataParam.ModifyUserDataType == ModifyUserDataTypes.LoginPassword)
            {
                passwordType = PasswordType.Login; // 指派密碼種類, 後面會做相應的處理

                oldEncryptContent = userInfo.UserPwd; // 拿舊的密碼, 執行SP時需要
            }
            else if (userModifyDataParam.ModifyUserDataType == ModifyUserDataTypes.MoneyPassword)
            {
                passwordType = PasswordType.Money;

                oldEncryptContent = userInfo.MoneyPwd; // 拿舊的密碼, 執行SP時需要
            }
            else
            {
                throw new NotSupportedException();
            }

            if ((userModifyDataParam.ModifyUserDataType == ModifyUserDataTypes.LoginPassword ||
                 userModifyDataParam.ModifyUserDataType == ModifyUserDataTypes.MoneyPassword)
                    && passwordType != null)
            {
                auditType = passwordType.AuditTypeValue;

                string newPassword = GetRandomPassword(passwordType); // 產生新的密碼
                newContent = UserRelatedElement.MaskContent; // 不能寫明碼
                newEncryptContent = newPassword.ToPasswordHash();

                addtionalAuditValue = newEncryptContent;

                // 組出審核通過的ReturnMessage
                string resetPasswordSuccessMessageRaw = string.Format(AuditElement.ResetPasswordSuccessMessage, passwordType.Name, newPassword);
                string auditPassReturnMessageRaw = string.Format("{0}\n{1} {2}\n{3}", AuditElement.AuditDone, UserRelatedElement.User, userInfo.UserName, resetPasswordSuccessMessageRaw);
                auditPassReturnMessage = auditPassReturnMessageRaw.ToEncryptedData(_appSettingService.CommonDataHash); // DES加密

                // 從設定檔取出設定值(是否需要檢查銀行卡資訊)
                isCheckBankCard = _configService.IsActive(ConfigGroupNameEnum.MoneyBankVerify_PasswordReset, passwordType.ResetPasswordCheckBankCardConfigSettingKey);
            }

            if (!checkModifyContent.IsSuccess)
            {
                return checkModifyContent;
            }
            #endregion

            #region 二次檢查傳入的銀行卡資訊是否正確
            //如果需要, 則做檢查
            if (isCheckBankCard)
            {
                UserVaildBankParam vaildBankParam = userModifyDataParam.CloneByJson();
                BaseReturnModel checkReturnModel = ValidationUserBank(vaildBankParam);

                if (!checkReturnModel.IsSuccess)
                {
                    return checkReturnModel;
                }
            }
            #endregion

            var auditInfo = new AuditInfoParam()
            {
                AuditType = auditType,
                RefID = userModifyDataParam.UserID.ToString(),
                UserID = userModifyDataParam.UserID,
                Memo = userModifyDataParam.Memo,
                BeforeValue = new AuditUserDataParam()
                {
                    ModifyUserDataType = (int)userModifyDataParam.ModifyUserDataType,
                    Content = userModifyDataParam.BeforeContent,
                    EncryptContent = oldEncryptContent
                }.ToJsonString(),
                AuditValue = new AuditUserDataParam()
                {
                    ModifyUserDataType = (int)userModifyDataParam.ModifyUserDataType,
                    Content = newContent,
                    EncryptContent = newEncryptContent,
                    OldEncryptContent = oldEncryptContent,
                    AuditPassReturnMessage = auditPassReturnMessage
                }.ToJsonString(),
                AddtionalAuditValue = new AuditUserDataCheckParam()
                {
                    Content = addtionalAuditValue // 建單時用於判別不同單子的
                }.ToJsonString()
            };

            //新建審核單
            var returnModel = _auditInfoService.Value.CreateAuditInfo(auditInfo);
            return returnModel;
        }

        private string GetRandomPassword(PasswordType passwordType)
        {
            string randomPassword = string.Empty;

            if (passwordType == PasswordType.Login)
            {
                randomPassword = StringUtil.CreateRandomCode(2, 3, 3, 0);
            }
            else if (passwordType == PasswordType.Money)
            {
                randomPassword = StringUtil.CreateRandomCode(3, 3, 3, 3);
            }
            else
            {
                randomPassword = StringUtil.CreateRandomCode(4, 4, 4, 4);
            }

            return randomPassword;
        }

        private BaseReturnDataModel<string> VaildModifyEmail(string modifyEmail)
        {
            if (modifyEmail.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(string.Format(UserRelatedElement.PleaseInputData, UserRelatedElement.NewEmail), string.Empty);
            }

            if (!Regex.Match(modifyEmail, RegularExpressionType.Email.Pattern).Success)
            {
                return new BaseReturnDataModel<string>(ReturnCode.EmailRuleFail);
            }

            string encryptContent = GetUserEncryptContent(modifyEmail, ModifyUserDataTypes.UserEmail);

            if (encryptContent.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(ReturnCode.OperationFailed);
            }

            if (_userInfoRep.IsHasEmail(encryptContent))
            {
                return new BaseReturnDataModel<string>(string.Format(UserRelatedElement.DuplicateContent, UserRelatedElement.Email), string.Empty);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, encryptContent);
        }

        #region 加密內容
        private string GetUserEncryptContent(string modifyContent, ModifyUserDataTypes userDataType)
        {
            if (userDataType == ModifyUserDataTypes.UserEmail)
            {
                return GetEncryptUserEmail(modifyContent);
            }

            return string.Empty;
        }

        private string GetEncryptUserEmail(string emailContent)
        {
            if (emailContent.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return emailContent.ToEncryptedEmail(_appSettingService.EmailHash);
        }

        private string GetEncryptUserPhoneNumber(string phoneNumberContent, string phoneHash)
        {
            if (phoneNumberContent.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return phoneNumberContent.ToEncryptedPhone(phoneHash);
        }
        #endregion

        public string GetUserName(int userId)
        {
            return _userInfoRep.GetUserName(userId);
        }

        public int? GetFrontSideUserId(string userName)
        {
            return _userInfoRep.GetFrontSideUserId(userName);
        }

        public int? GetParentUserId(int userId)
        {
            UserInfo userInfo = GetUserInfo(userId);

            if (userInfo == null)
            {
                return null;
            }

            return userInfo.ParentID;
        }

        public UserInfo GetUserInfo(int userId)
        {
            return _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });
        }

        public UserInfo GetUserInfo(string userName)
        {
            return _userInfoRep.GetUserInfo(userName);
        }

        public List<int> GetAllFirstChild(int parentId)
        {
            return _userInfoRep.GetAllFirstChild(parentId);
        }

        public List<UserInfo> GetAllFirstChildUserInfo(int parentId, bool isForceRefresh = false)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.AllFirstChildUserInfo(parentId),
                CacheSeconds = 180,
                IsForceRefresh = isForceRefresh
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                return _userInfoRep.GetAllFirstChildUserInfo(parentId);
            });
        }

        public BaseReturnModel SaveChildTransferStatus(int userId, TransferToChildStatus transferToChildStatus, string memo)
        {
            bool? isAllowSetTransferByParent = null;
            bool isLowMoneyIn = true;
            string operationLogContent = null;

            UserInfo userInfo = GetUserInfo(userId);

            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            TransferToChildStatus oldTransferToChildStatus = GetUserTransferToChildStatus(userId);
            OperationLogSetting operationSetting = GetOperationLogSetting();

            if (operationSetting.IsMemoRequired && !IsValidRequired(memo))
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            operationLogContent = string.Format(operationSetting.MemoTemplate,
                oldTransferToChildStatus.Name,
                transferToChildStatus.Name,
                memo ?? _deviceService.GetDeviceName());

            if (IsValidSaveParamLegality)
            {
                if (oldTransferToChildStatus == TransferToChildStatus.ForceDisabled) //null or true表示可被修改
                {
                    return new BaseReturnModel(ReturnCode.TransferToChildForcedClose);
                }
                else if (EnvLoginUser.LoginUser.UserId != GetParentUserId(userId).GetValueOrDefault())
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
            }

            if (oldTransferToChildStatus == transferToChildStatus)
            {
                return new BaseReturnModel(ReturnCode.NoDataChanged);
            }


            if (transferToChildStatus == TransferToChildStatus.Enabled)
            {
                isLowMoneyIn = true;
                isAllowSetTransferByParent = true;
            }
            else if (transferToChildStatus == TransferToChildStatus.Disabled)
            {
                isLowMoneyIn = false;
                isAllowSetTransferByParent = true;
            }
            else if (transferToChildStatus == TransferToChildStatus.ForceDisabled)
            {
                isLowMoneyIn = false;
                isAllowSetTransferByParent = false;
            }

            if (!IsAllowChangeSetTransferByParent)
            {
                isAllowSetTransferByParent = null; //讓sp跳過存檔
            }

            SPReturnModel spReturnModel = _userInfoAdditionalRep
                .SaveUserTransferChildStatus(userId, EnvLoginUser.LoginUser.UserName, isLowMoneyIn, isAllowSetTransferByParent);


            var baseReturnModel = new BaseReturnModel(ReturnCode.GetSingle(spReturnModel.ReturnCode));

            if (!operationLogContent.IsNullOrEmpty())
            {
                _operationLogService.Value.InsertModifyMemberOperationLog(new InsertModifyMemberOperationLogParam
                {
                    Category = operationSetting.Category,
                    AffectedUserId = userId,
                    AffectedUserName = userInfo.UserName,
                    Content = operationLogContent
                });
            }

            if (baseReturnModel.IsSuccess)
            {
                baseReturnModel.Message = MessageElement.OperationSuccess;
            }

            return baseReturnModel;
        }

        public TransferToChildStatus GetUserTransferToChildStatus(int userId)
        {
            UserInfo userInfo = GetUserInfo(userId);
            TransferToChildStatus returnValue = TransferToChildStatus.Disabled;

            if (userInfo == null)
            {
                return null;
            }

            if (userInfo.IslowMoneyIn.HasValue)
            {
                returnValue = TransferToChildStatus.GetSingle(Convert.ToInt32(userInfo.IslowMoneyIn.Value));
            }

            UserInfoAdditional userInfoAdditional = _userInfoAdditionalRep.GetSingleByKey(InlodbType.Inlodb, new UserInfoAdditional() { UserID = userId });

            if (userInfoAdditional != null && userInfoAdditional.IsAllowSetTransferByParent == false)
            {
                returnValue = TransferToChildStatus.ForceDisabled;
            }

            return returnValue;
        }

        public UserInfo GetMasterOrSlaveRelationshipUserInfo(int loginUserId, int targetUserId)
        {
            var userIds = new List<int>() { loginUserId, targetUserId };

            //找當前登入User及傳入的UserId的UserInfo
            var userInfos = _userInfoRep.GetUserInfos(userIds);

            //理論上傳入的筆數要全部找到，沒找到等於有一個UserId是不合法的，回傳null出去
            if (userInfos.Count() != userIds.Count())
            {
                return null;
            }

            var loginUserInfo = userInfos.Where(ui => ui.UserID == loginUserId).Single();
            var targetUserInfo = userInfos.Where(ui => ui.UserID == targetUserId).Single();

            //判斷是否互為上下級關係
            bool isMasterOrSlaveRelationship = ((loginUserInfo.UserID == targetUserInfo.ParentID) ||
                                               (targetUserInfo.UserID == loginUserInfo.ParentID));

            if (isMasterOrSlaveRelationship)
            {
                return targetUserInfo;
            }
            else
            {
                return null;
            }
        }

        public bool IsMoneyPasswordExpired(int userId)
        {
            var userInfo = GetUserInfo(userId);

            //找不到用戶資料或密碼為空都當做過期
            if (userInfo == null || userInfo.MoneyPwd.IsNullOrEmpty())
            {
                return true;
            }

            return userInfo.MoneyPwdExpiredDate.IsTimeExpired();
        }

        public BaseReturnModel SavePassword(SavePasswordParam saveMoneyPasswordParam, Action afterSaveLoginPasswordSuccess)
        {
            if (!IsValidRequired(saveMoneyPasswordParam.OldPasswordHash, saveMoneyPasswordParam.NewPasswordHash))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            BaseReturnModel saveReturnModel = _userInfoRep.UpdatePassword(saveMoneyPasswordParam);
            string addPasswordFailHistoryJobMsg = null;

            if (saveReturnModel.IsSuccess)
            {
                string operationLogContent = string.Format(OperationLogContentElement.SavePassword,
                    saveMoneyPasswordParam.SavePasswordType.Name,
                    saveMoneyPasswordParam.OldPasswordHash,
                    saveMoneyPasswordParam.NewPasswordHash);

                _operationLogService.Value.InsertFrontSideOperationLog(new InsertFrontSideOperationLogParam()
                {
                    AffectedUserId = EnvLoginUser.LoginUser.UserId,
                    AffectedUserName = EnvLoginUser.LoginUser.UserName,
                    Content = operationLogContent
                });

                //只有修改登入密碼才需要踢人
                if (saveMoneyPasswordParam.SavePasswordType == PasswordType.Login)
                {
                    _failureLoginHistoryRep.DeleteFailureLoginHistory(EnvLoginUser.LoginUser.UserName);

                    if (afterSaveLoginPasswordSuccess != null)
                    {
                        afterSaveLoginPasswordSuccess.Invoke();
                    }
                }
                else if (saveMoneyPasswordParam.SavePasswordType == PasswordType.Money)
                {
                    _failureOperationService.ClearCount(WebActionType.ModifyMoneyPassword, SubActionType.MoneyPasswordWrong);
                }
            }
            else if (!saveReturnModel.IsSuccess && saveReturnModel.Message == _oldPasswordWrong)
            {
                //舊密碼錯誤處理
                if (saveMoneyPasswordParam.SavePasswordType == PasswordType.Login)
                {
                    string ip = IpUtil.GetDoWorkIP();
                    string errorMsg = null;

                    int failureTimes = _failureLoginHistoryRep.GetFailureLoginTimes(EnvLoginUser.LoginUser.UserName);
                    DateTime now = DateTime.Now;

                    _failureLoginHistoryRep.CreateByProcedure(new FailureLoginHistory()
                    {
                        UserName = EnvLoginUser.LoginUser.UserName,
                        LoginIp = ip,
                        LoginTime = now
                    });

                    if (failureTimes + 1 > 4)
                    {
                        if (UpdateUserActive(EnvLoginUser.LoginUser.UserId, false))
                        {
                            errorMsg = $"{saveMoneyPasswordParam.SavePasswordType.Name}错误次数过多，账号已被锁定";
                            LogUtil.ForcedDebug($"用户 {EnvLoginUser.LoginUser.UserName} 在 {now.ToFormatDateTimeString()}" +
                                $"连续修改{saveMoneyPasswordParam.SavePasswordType.Name}错误次数过多，被冻结");

                            _failureLoginHistoryRep.DeleteFailureLoginHistory(EnvLoginUser.LoginUser.UserName);
                        }
                    }
                    else
                    {
                        errorMsg = $"{saveMoneyPasswordParam.SavePasswordType.Name}错误，错误次数过多，会导致账号被锁定";
                    }

                    addPasswordFailHistoryJobMsg = errorMsg;
                }
                else if (saveMoneyPasswordParam.SavePasswordType == PasswordType.Money) //只有資金密碼要寫操作紀錄, 登錄密碼目前不用
                {
                    _failureOperationService.AddFailOperation(WebActionType.ModifyMoneyPassword, SubActionType.MoneyPasswordWrong);
                    addPasswordFailHistoryJobMsg = ReturnCode.YourMoneyPasswordIsWrong.Name;
                }
            }

            string blockActionMsg = CheckUserBlockAction(saveMoneyPasswordParam.SavePasswordType, saveReturnModel);

            if (!blockActionMsg.IsNullOrEmpty())
            {
                _jxCacheService.RemoveCache(CacheKey.GetFrontSideUserInfoKey(EnvLoginUser.LoginUser.UserKey));

                return new BaseReturnModel(blockActionMsg);
            }
            else if (!addPasswordFailHistoryJobMsg.IsNullOrEmpty())
            {
                return new BaseReturnModel(addPasswordFailHistoryJobMsg);
            }

            return saveReturnModel;
        }

        private string CheckUserBlockAction(PasswordType passwordType, BaseReturnModel saveReturnModel)
        {
            //修改成功 或是 舊密碼錯誤
            if (saveReturnModel.IsSuccess || saveReturnModel.Message == _oldPasswordWrong)
            {
                IPInformation ipInfo = IpUtil.GetDoWorkIPInformation();
                string ipAddress = ipInfo.DestinationIP;

                WebActionType webActionType = PasswordType.Login == passwordType ? WebActionType.ModifyLoginPassword : WebActionType.ModifyMoneyPassword;

                bool isBlock = _blockActionService.Value.BlockWebActionClient(ipAddress, webActionType.Value);

                // IP已進入黑名單
                if (isBlock == true)
                {
                    return SecurityElement.ModifiedManyTimesToBlockIp;
                }
            }

            return null;
        }

        public BaseReturnModel SavePasswordByOtherWay(SaveNonHashManualPasswordByOtherWayParam param)
        {
            BaseReturnModel returnModel = IsPasswordFormatValid(param.SavePasswordType, param.NewPassword);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            return SavePasswordHashByOtherWay(new SavePasswordByOtherWayParam()
            {
                UserID = param.UserID,
                SavePasswordType = param.SavePasswordType,
                NewPasswordHash = param.NewPassword.ToPasswordHash(),
                WayName = param.WayName
            });
        }

        public BaseReturnModel SavePasswordHashByOtherWay(SavePasswordByOtherWayParam param)
        {
            if (!IsValidRequired(param.WayName, param.NewPasswordHash))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            BaseReturnModel saveReturnModel = _userInfoRep.UpdatePasswordByOtherWay(param);

            string operationLogContent = string.Format(OperationLogContentElement.SavePasswordByOtherWay,
                param.WayName,
                param.SavePasswordType.Name,
                param.NewPasswordHash);

            _operationLogService.Value.InsertFrontSideOperationLog(new InsertFrontSideOperationLogParam()
            {
                AffectedUserId = EnvLoginUser.LoginUser.UserId,
                AffectedUserName = EnvLoginUser.LoginUser.UserName,
                Content = operationLogContent
            });

            return saveReturnModel;
        }

        /// <summary>
        /// 儲存用戶暱稱
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public BaseReturnModel SaveUserNickname(string nickname)
        {
            if (nickname.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            if (System.Text.Encoding.Default.GetBytes(nickname).Length > _maxNicknameLength)
            {
                return new BaseReturnModel(ReturnCode.NicknameTooManyWords);
            }

            BaseReturnModel saveReturnModel = _userInfoRep.SaveUserNickname(EnvLoginUser.LoginUser.UserId, nickname);

            return saveReturnModel;
        }

        public string GetUserNickname()
        {
            return _userInfoRep.GetUserNickname(EnvLoginUser.LoginUser.UserId);
        }

        public BaseReturnModel CheckUserEmail(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            string userEmail = _userInfoRep.GetUserEmail(EnvLoginUser.LoginUser.UserId);

            if (!userEmail.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.AlreadySecurityStatus);
            }

            if (_userInfoRep.IsHasEmail(email.ToEncryptedEmail(_appSettingService.EmailHash)))
            {
                return new BaseReturnModel(ReturnCode.EmailIsUsed);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel SaveUserSecurityInfo(SaveSecuritySetting securitySetting)
        {
            if (!IsValidRequired(securitySetting.MoneyPasswordHash, securitySetting.Email,
                securitySetting.FirstQuestionId, securitySetting.FirstAnswer,
                securitySetting.SecondQuestionId, securitySetting.SecondAnswer))
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            if (!Regex.Match(securitySetting.Email, RegularExpressionType.Email.Pattern).Success)
            {
                return new BaseReturnModel(ReturnCode.EmailRuleFail);
            }

            BaseReturnModel checkUserEmail = CheckUserEmail(securitySetting.Email);

            if (!checkUserEmail.IsSuccess)
            {
                return new BaseReturnModel(checkUserEmail.Message);
            }

            var spSaveUserSecurityInfoParam = securitySetting.CastByJson<SpSaveUserSecurityInfoParam>();
            spSaveUserSecurityInfoParam.EmailEncrypt = securitySetting.Email.ToEncryptedEmail(_appSettingService.EmailHash);
            spSaveUserSecurityInfoParam.UserID = EnvLoginUser.LoginUser.UserId;

            BaseReturnModel returnModel = _userInfoRep.SaveUserSecurityInfo(spSaveUserSecurityInfoParam);

            return returnModel;
        }

        public bool HasUserActiveBankInfo()
        {
            return _userInfoRep.HasUserActiveBankInfo(EnvLoginUser.LoginUser.UserId);
        }

        public bool HasUserInitializationComplete(int userId)
        {
            return _userInfoRep.IsInitializeUser(userId);
        }

        public bool UpdateUserActive(int userId, bool isActive)
        {
            return _userInfoRep.UpdateUserActive(userId, isActive);
        }

        public UserLevel GetUserLevel(int userId)
        {
            UserLevel userLevel = _userLevelRep.GetSingleByKey(InlodbType.Inlodb, new UserLevel() { UserID = userId });

            if (userLevel == null)
            {
                userLevel = new UserLevel()
                {
                    Level = 1,
                    UserID = userId
                };
            }

            return userLevel;
        }

        private BaseReturnModel IsPasswordFormatValid(PasswordType passwordType, string password)
        {
            if (!Regex.IsMatch(password, passwordType.ValidRegularExpressionType.Pattern))
            {
                return new BaseReturnModel(passwordType.ValidRegularExpressionType.ErrorMsg);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        #region 根據站台而有不同的屬性(這邊規則較單純,故不使用抽象方法把規則拆出去)

        /// <summary>是否允許異動「是否可以被上級設置轉帳開關」欄位</summary>
        private bool IsAllowChangeSetTransferByParent => EnvLoginUser.Application == JxApplication.BackSideWeb;

        /// <summary>是否要檢查異動人員與登入人員的上下級關係</summary>
        private bool IsValidSaveParamLegality => EnvLoginUser.Application != JxApplication.BackSideWeb;

        private OperationLogSetting GetOperationLogSetting()
        {
            JxOperationLogCategory category;
            string memoTemplate;
            bool isMemoRequired;

            if (EnvLoginUser.Application == JxApplication.BackSideWeb)
            {
                category = JxOperationLogCategory.Member;
                memoTemplate = OperationLogContentElement.SaveUserTransferChildStatus;
                isMemoRequired = true;
            }
            else
            {
                category = JxOperationLogCategory.ChangeUserInfo;
                memoTemplate = OperationLogContentElement.SaveUserTransferChildStatusByFrontSide;
                isMemoRequired = false;
            }

            return new OperationLogSetting
            {
                Category = category,
                MemoTemplate = memoTemplate,
                IsMemoRequired = isMemoRequired
            };

        }

        #endregion
    }
}
