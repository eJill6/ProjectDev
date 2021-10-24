using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.DownloadFile;
using JxBackendService.Model.Param.Client;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendService.Model.ViewModel.Security;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Interface.Service.Finance;
using JxBackendServiceNet45.Model.Enums;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;
using System;
using System.Web;

namespace JxBackendServiceNet45.Service.Authenticator.Base
{
    public abstract class BaseUserAuthApiWCFService : BaseApplicationService, IUserAuthApiService
    {
        private readonly IUserAuthenticatorValidService _userAuthenticatorValidService;
        private readonly IUserAuthenticatorValidReadService _userAuthenticatorValidReadService;
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;
        private readonly IUserInfoRelatedService _userInfoRelatedService;
        private readonly IUserFinanceService _userFinanceService;
        private readonly IUserFinanceReadService _userFinanceReadService;
        private readonly IBlackLocationReadService _blackLocationReadService;
        private readonly IWhiteIpListReadService _whiteIpListReadService;
        private readonly IJxCacheService _jxCacheService;
        private readonly IIpUtilService _ipUtilService;

        private readonly int _securityTokenExpiredSeconds = 10 * 60;

        public BaseUserAuthApiWCFService()
        {
            _userAuthenticatorValidService = ResolveJxBackendService<IUserAuthenticatorValidService>(DbConnectionTypes.Master);
            _userAuthenticatorValidReadService = ResolveJxBackendService<IUserAuthenticatorValidReadService>(DbConnectionTypes.Slave);
            _userInfoRelatedReadService = ResolveJxBackendService<IUserInfoRelatedReadService>(DbConnectionTypes.Slave);
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Master);
            _userFinanceService = ResolveJxBackendService<IUserFinanceService>(DbConnectionTypes.Master);
            _userFinanceReadService = ResolveJxBackendService<IUserFinanceReadService>(DbConnectionTypes.Slave);
            _blackLocationReadService = ResolveJxBackendService<IBlackLocationReadService>(DbConnectionTypes.Slave);
            _whiteIpListReadService = ResolveJxBackendService<IWhiteIpListReadService>(DbConnectionTypes.Slave);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
        }

        public BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo()
        {
            return _userAuthenticatorValidReadService.GetUserAuthenticatorInfo(EnvLoginUser.LoginUser.UserId);
        }

        public BaseReturnDataModel<UserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId)
        {
            return _userAuthenticatorValidReadService.GetUserAuthenticatorInfo(userId);
        }

        public BaseReturnDataModel<bool> IsMoneyPasswordExpired()
        {
            return new BaseReturnDataModel<bool>(ReturnCode.Success, _userInfoRelatedReadService.IsMoneyPasswordExpired(EnvLoginUser.LoginUser.UserId));
        }

        public BaseReturnDataModel<bool> HasGoogleBound()
        {
            return new BaseReturnDataModel<bool>(ReturnCode.Success,
                GetUserAuthenticatorInfo().DataModel.UserAuthenticatorStatus == UserAuthenticatorStatuses.Verified);
        }

        /// <summary>
        /// 取得用戶未綁定Google驗證QRCode資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BaseReturnDataModel<UserAuthInfo> GetUserAuthInfo()
        {
            var userAuthInfo = new UserAuthInfo
            {
                IsVerified = GetUserAuthenticatorInfo().IsSuccess,
                UserId = EnvLoginUser.LoginUser.UserId,
                UserName = EnvLoginUser.LoginUser.UserName,
            };

            if (!userAuthInfo.IsVerified)
            {
                userAuthInfo.QrCodeViewModel = _userAuthenticatorValidService.GetQrCode(new CreateQrCodeViewModelParam()
                {
                    Application = EnvLoginUser.Application,
                    CreateQrCodeWithType = AuthenticatorType.Google,
                    IsForcedRefresh = true,
                    SearchUser = EnvLoginUser.LoginUser,
                });

                if (userAuthInfo.QrCodeViewModel != null)
                {
                    return new BaseReturnDataModel<UserAuthInfo>(ReturnCode.Success, userAuthInfo);
                }
            }

            return new BaseReturnDataModel<UserAuthInfo>(ReturnCode.GoogleAuthenticatorVerified, userAuthInfo);
        }

        /// <summary>
        /// 綁定/解除綁定
        /// </summary>
        /// <param name="authParam"></param>
        /// <returns></returns>
        public BaseReturnModel VerifyAuthenticator(string moneyPasswordHash, string clientPin, bool isVerified)
        {
            return _userAuthenticatorValidService.VerifyAuthenticator(new VerifiedAuthenticatorParam()
            {
                User = EnvLoginUser.LoginUser,
                MoneyPasswordHash = moneyPasswordHash,
                Pin = clientPin,
                IsVerified = isVerified
            });
        }

        public BaseReturnModel UserAuthBeforeCheck(int userAuthenticatorSettingType)
        {
            AuthenticatorPermission authenticatorPermission = _userAuthenticatorValidReadService
                .GetAuthenticatorPermission(EnvLoginUser.LoginUser.UserId, (UserAuthenticatorSettingTypes)userAuthenticatorSettingType);

            if (authenticatorPermission.IsAllowExecuted)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(authenticatorPermission.ErrorMessage);
            }
        }

        public BaseReturnModel TransferToChild(BasicTransferToChildParam basicTransferToChildParam)
        {
            BaseReturnModel returnModel = _userFinanceService.TransferToChild(new TransferToChildParam()
            {
                LoginUser = EnvLoginUser.LoginUser,
                MoneyPasswordHash = basicTransferToChildParam.MoneyPasswordHash,
                TransferAmount = basicTransferToChildParam.TransferAmount,
                ChildUserName = basicTransferToChildParam.ChildUserName,
                ClientPin = basicTransferToChildParam.ClientPin,
                IpAddress = IpUtil.GetDoWorkIpAddressFromHeader()
            });

            return returnModel;
        }

        public BaseReturnModel UpdateUSDTWallet(BasicBlockChainInfoParam basicBlockChainInfoParam)
        {
            BaseReturnModel returnModel = _userFinanceService.UpdateUSDTWallet(new BlockChainInfoParam()
            {
                LoginUser = EnvLoginUser.LoginUser,
                WalletAddr = basicBlockChainInfoParam.WalletAddr,
                MoneyPasswordHash = basicBlockChainInfoParam.MoneyPasswordHash,
                ClientPin = basicBlockChainInfoParam.ClientPin,
                IsActive = basicBlockChainInfoParam.IsActive,
                IpAddress = IpUtil.GetDoWorkIpAddressFromHeader()
            });

            return returnModel;
        }

        public BaseAuthenticatorPermission GetAuthenticatorPermission(int userAuthenticatorSettingType)
        {
            var userAuthenticatorValidReadService = ResolveJxBackendService<IUserAuthenticatorValidReadService>(DbConnectionTypes.Slave);
            var authenticatorPermission = userAuthenticatorValidReadService.GetAuthenticatorPermission(EnvLoginUser.LoginUser.UserId,
                (UserAuthenticatorSettingTypes)userAuthenticatorSettingType);

            BaseAuthenticatorPermission baseAuthenticatorPermission = authenticatorPermission.CastByJson<BaseAuthenticatorPermission>();
            return baseAuthenticatorPermission;
        }

        public string CreateSecurityToken(TokenType tokenType, int stepId, string data)
        {
            var userSecurityService = DependencyUtil.ResolveServiceForModel<IUserSecurityService>(EnvLoginUser.Application);

            return userSecurityService.CreateSecurityToken(EnvLoginUser.LoginUser.UserId, tokenType, stepId, data);
        }

        public BaseReturnModel IsSecurityTokenValid(string accessToken, TokenType tokenType, int tokenStepId, int expiredSeconds, string data)
        {
            var userSecurityService = DependencyUtil.ResolveServiceForModel<IUserSecurityService>(EnvLoginUser.Application);

            return userSecurityService.IsSecurityTokenValid(
                EnvLoginUser.LoginUser.UserId,
                accessToken,
                tokenType,
                tokenStepId,
                expiredSeconds,
                data);
        }
        public string GetUserNickname()
        {
            return _userInfoRelatedReadService.GetUserNickname();
        }

        public SecurityNextStep SaveUserNickname(string nickname)
        {
            BaseReturnModel baseReturnModel = _userInfoRelatedService.SaveUserNickname(nickname);
            var nextStep = new SecurityNextStep();
            nextStep.SetBehaviorErrorDialog(baseReturnModel.Message);

            if (baseReturnModel.IsSuccess)
            {
                nextStep.AccessToken = CreateSecurityToken(TokenType.SecurityInitialize, (int)SecurityStep.SaveNickname, null);
                nextStep.SetBehaviorRedirectPage(ClientAppPage.InitialSecurity);
            }

            return nextStep;
        }

        public BaseReturnModel SaveUserSecurityInfo(SaveSecuritySetting securitySetting)
        {
            if (securitySetting.AccessToken.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.EntranceIsNotAllowed);
            }

            BaseReturnModel securityTokenReturnModel = IsSecurityTokenValid(securitySetting.AccessToken, TokenType.SecurityInitialize,
                (int)SecurityStep.SaveNickname, _securityTokenExpiredSeconds, null);

            if (!securityTokenReturnModel.IsSuccess)
            {
                return securityTokenReturnModel;
            }

            BaseReturnModel returnModel = _userInfoRelatedService.SaveUserSecurityInfo(securitySetting);

            return returnModel;
        }

        public BaseReturnModel IsUserTwoFactorPinValid(int userAuthenticatorSettingTypeValue, string clientPin)
        {
            return _userAuthenticatorValidReadService.IsUserTwoFactorPinValid(
                EnvLoginUser.LoginUser.UserId,
                (UserAuthenticatorSettingTypes)userAuthenticatorSettingTypeValue,
                clientPin);
        }

        public bool HasUserActiveBankInfo()
        {
            return _userInfoRelatedReadService.HasUserActiveBankInfo();
        }

        public bool HasUserActiveUsdtAccount()
        {
            return _userFinanceReadService.HasUserActiveUsdtAccount();
        }

        public bool HasUserInitializationComplete(int userId)
        {
            return _userInfoRelatedService.HasUserInitializationComplete(userId);
        }

        public string GetGoogleBindTutorialUrl()
        {
            return StringUtil.ToFullFileUrl(
                GetCommonStaticFileDomain(),
                DownloadFilePathTypes.GoogleBindTutorialHtml.FilePath);
        }

        public bool CheckUserAllowedLogin()
        {
            bool isUserAllowedLogin = true;

            if (EnvLoginUser == null || EnvLoginUser.LoginUser == null || EnvLoginUser.LoginUser.UserId == 0)
            {
                isUserAllowedLogin = false;
            }

            //檢查是否被凍結
            UserInfo userInfo = _userInfoRelatedReadService.GetUserInfo(EnvLoginUser.LoginUser.UserId);

            if (isUserAllowedLogin && (userInfo != null && !userInfo.IsActive))
            {
                isUserAllowedLogin = false;
            }

            JxIpInformation ipInformation = _ipUtilService.GetDoWorkIPInformation();

            if (isUserAllowedLogin && !_blackLocationReadService.IsFrontSideLoginIpActive(ipInformation))
            {
                isUserAllowedLogin = false;
            }

            if (!isUserAllowedLogin)
            {
                _jxCacheService.RemoveCache(CacheKey.GetFrontSideUserInfoKey(EnvLoginUser.LoginUser.UserKey));
            }

            return isUserAllowedLogin;
        }
        
        public string GetWebForwardUrl(int clientWebPageValue)
        {
            var signInToken = new SignInToken() {
                UserKey = EnvLoginUser.LoginUser.UserKey,
                ExpiredDate = DateTime.Now.AddSeconds(30)
            };

            IAppSettingService appSettingService = ResolveKeyedForModel<IAppSettingService>(EnvLoginUser.Application);
            string token = signInToken.ToJsonString().ToEncryptedData(appSettingService.CommonDataHash);

            return SharedAppSettings.FrontSideWebUrl + $"/Account/SignInByToken?clientWebPageValue={clientWebPageValue}&token={HttpUtility.UrlEncode(token)}";
        }
    }
}