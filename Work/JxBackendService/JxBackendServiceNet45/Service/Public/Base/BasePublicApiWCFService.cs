using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Merchant;
using JxBackendService.Model.ViewModel.Security;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Interface.Service.Public;
using JxBackendServiceNet45.Model.Param;
using System;

namespace JxBackendServiceNet45.Service.Public.Base
{
    public abstract class BasePublicApiWCFService : BaseApplicationService, IPublicApiService
    {
        private readonly IUserAuthenticatorValidReadService _userAuthenticatorValidReadService;
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;
        private readonly IUserInfoRelatedService _userInfoRelatedService;        
        private readonly IOperationLogService _operationLogService;
        private readonly IBlockActionService _blackActionService;
        private readonly IJxCacheService _jxCacheService;

        public abstract void MarkUserToDelete(int userId, string userName);

        public BasePublicApiWCFService()
        {
            _userAuthenticatorValidReadService = ResolveJxBackendService<IUserAuthenticatorValidReadService>(DbConnectionTypes.Slave);
            _userInfoRelatedReadService = ResolveJxBackendService<IUserInfoRelatedReadService>(DbConnectionTypes.Slave);
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Master);
            _operationLogService = ResolveJxBackendService<IOperationLogService>(DbConnectionTypes.Master);
            _blackActionService = ResolveJxBackendService<IBlockActionService>(DbConnectionTypes.Master);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        public BaseReturnModel UpdatePasswordHashByAuthenticator(SaveFindPasswordHashParam param)
        {
            PasswordType passwordType = PasswordType.GetSingle(param.PasswordType);

            if (passwordType == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            int? userId = null;

            if (passwordType.IsOnlyUpdateByLoginUser)
            {
                if (EnvLoginUser.LoginUser.UserName.IsNullOrEmpty())
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                param.UserName = EnvLoginUser.LoginUser.UserName;
                userId = EnvLoginUser.LoginUser.UserId;
            }
            else
            {
                userId = _userInfoRelatedReadService.GetFrontSideUserId(param.UserName);

                if (!userId.HasValue)
                {
                    return new BaseReturnModel(UserAuthElement.GoogleAuthFail);
                }
            }

            if (param.VerifyCode.IsNullOrEmpty())
            {
                return new BaseReturnModel(UserAuthElement.GoogleAuthCodeIsEmpty);
            }

            BaseReturnModel validResult = _userAuthenticatorValidReadService.IsUserTwoFactorPinValid(userId.Value, param.VerifyCode);

            if (!validResult.IsSuccess)
            {
                //寫入操作日誌
                string content = string.Join(", ", 
                    passwordType.OperationContentName,
                    OperationLogContentElement.IncorrectGoogleAuthCode);

                _operationLogService.InsertFrontSideOperationLogWithUserLoginDetails(new InsertFrontSideOperationLogParam
                {
                    AffectedUserId = userId.Value,
                    AffectedUserName = param.UserName,
                    Content = content
                });

                return validResult;
            }

            BaseReturnModel returnModel = _userInfoRelatedService.SavePasswordHashByOtherWay(new SavePasswordByOtherWayParam()
            {
                WayName = ForgetPasswordElement.GoogleAuthenticator,
                NewPasswordHash = param.NewPasswordHash,
                UserID = userId.Value,
                SavePasswordType = passwordType,
            });

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            if (passwordType == PasswordType.Login)
            {
                MarkUserToDelete(userId.Value, param.UserName);
            }

            return returnModel;
        }

        public bool BlockWebActionClient(string ipAddress, int webActionTypeValue)
        {
            return _blackActionService.BlockWebActionClient(ipAddress, webActionTypeValue);
        }

        /// <summary>
        /// 產生圖形驗證碼
        /// </summary>
        /// <returns></returns>
        public BaseReturnDataModel<string> GetValidateCodeImage(int actionTypeValue , string userIdentityKey, bool isForceRefresh = false)
        {
            var _validateCodeService = ResolveJxBackendService<IValidateCodeService>(DbConnectionTypes.Slave);

            return _validateCodeService.GetGraphicValidateCodeImage(WebActionType.GetSingle(actionTypeValue), userIdentityKey, isForceRefresh);
        }

        public void LogOff()
        {
            if (!EnvLoginUser.LoginUser.UserKey.IsNullOrEmpty())
            {
                _jxCacheService.RemoveCache(CacheKey.GetFrontSideUserInfoKey(EnvLoginUser.LoginUser.UserKey));
            }
        }

        public BaseReturnDataModel<BaseUserInfoToken> GetUserByToken(string encryptedToken)
        {
            IAppSettingService appSettingService = ResolveKeyedForModel<IAppSettingService>(EnvLoginUser.Application);
            string token = encryptedToken.ToDescryptedData(appSettingService.CommonDataHash);
            SignInToken signInToken = token.Deserialize<SignInToken>();

            //todo 檢查過期時間


            BaseReturnDataModel<BaseUserInfoToken> returnDataModel =  GetUserByUserKey(EnvLoginUser.Application, signInToken.UserKey, null);
            return returnDataModel;
        }

        public MerchantSettingModel GetMerchantSetting()
        {
            var merchantSettingService = DependencyUtil.ResolveKeyed<IMerchantSettingService>(SharedAppSettings.PlatformMerchant);

            return new MerchantSettingModel()
            {
                IsRWD = merchantSettingService.IsRWD
            };
        }     

        #region 替換舊版 Remote CacheBase 而開的Service

        public string GetRemoteCacheForOldService(string key)
        {
            CacheKey cacheKey = CacheKey.GetKeyForDistributeMemCache(key);
            return _jxCacheService.GetCache<string>(cacheKey);
        }
        
        public void RemoveRemoteCacheForOldService(string key)
        {
            CacheKey cacheKey = CacheKey.GetKeyForDistributeMemCache(key);
            _jxCacheService.RemoveCache(cacheKey);
        }
        
        public void SetRemoteCacheForOldService(string key, int cacheSeconds, bool isSlidingExpiration, string value)
        {
            CacheKey cacheKey = CacheKey.GetKeyForDistributeMemCache(key);

            SetCacheParam setCacheParam = new SetCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = cacheSeconds,
                IsSlidingExpiration = isSlidingExpiration
            };

            _jxCacheService.SetCache(setCacheParam, value);
        }
        #endregion
    }
}