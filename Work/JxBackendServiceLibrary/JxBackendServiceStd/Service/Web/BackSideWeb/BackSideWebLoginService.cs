using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Login;
using JxBackendService.Model.Param.Authenticator;
using JxBackendService.Model.Param.Security;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Authenticator;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;

namespace JxBackendServiceN6.Service.Web.BackSideWeb
{
    public class BackSideWebLoginService : BaseService, IBackSideWebLoginService
    {
        private static readonly JxApplication s_application = JxApplication.BackSideWeb;

        private static readonly int s_loginStringExpiredSeconds = 30;

        private readonly IBWAuthenticatorService _bwAuthenticatorService;

        private readonly IBWRoleInfoService _bwRoleInfoService;

        private readonly IBackSideWebUserService _backSideWebUserService;

        private readonly IBWUserInfoRep _bwUserInfoRep;

        public BackSideWebLoginService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwAuthenticatorService = ResolveJxBackendService<IBWAuthenticatorService>();
            _bwRoleInfoService = ResolveJxBackendService<IBWRoleInfoService>();
            _backSideWebUserService = ResolveJxBackendService<IBackSideWebUserService>();
            _bwUserInfoRep = ResolveJxBackendService<IBWUserInfoRep>();
        }

        public BaseReturnDataModel<BWLoginResultParam> Login(LoginDetailParam loginDetailParam,
            Action<ClaimsPrincipal, AuthenticationProperties> doHttpSignIn)
        {
            var loginResult = new BWLoginResultParam()
            {
                UserName = loginDetailParam.UserName,
                MachineName = loginDetailParam.MachineName,
                WinLoginName = loginDetailParam.WinLoginName,
                LocalUTCTime = loginDetailParam.UTCTime,
                LoginToolVersion = loginDetailParam.LoginToolVersion
            };

            if (IsExpired(loginDetailParam.UTCTime))
            {
                loginResult.LoginStatus = LoginStatuses.LoginCodeExpired;

                return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.ValidateCodeTimeout, loginResult);
            }

            BWUserInfo userInfo = _bwUserInfoRep.GetUserInfoIdByUsername(loginDetailParam.UserName);
            string userPasswordHash = loginDetailParam.UserPWD.ToPasswordHash();
            string dbPasswordHash = userInfo?.Password;

            if (userInfo == null || !string.Equals(dbPasswordHash, userPasswordHash, StringComparison.Ordinal))
            {
                loginResult.LoginStatus = LoginStatuses.UserNameOrPasswordIsNotValid;

                return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.LoginFail, loginResult);
            }

            int userId = userInfo.UserID;
            loginResult.UserID = userId;

            BWUserAuthenticatorInfo userAuthenticatorInfo = _bwAuthenticatorService.GetUserAuthenticatorInfo(userId).DataModel;
            BWUserAuthenticator userAuthenticator = userAuthenticatorInfo.BWUserAuthenticator;

            if (userAuthenticator == null)
            {
                loginResult.LoginStatus = LoginStatuses.NoAuthenticator;

                return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.AuthenticatorUnverified, loginResult);
            }

            if (userAuthenticatorInfo.BWUserAuthenticatorStatus == BWUserAuthenticatorStatuses.Expired)
            {
                loginResult.LoginStatus = LoginStatuses.AuthenticatorExpired;

                return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.AuthenticatorExpired, loginResult);
            }

            AuthenticatorType authenticatorType = AuthenticatorType.GetSingle(userAuthenticatorInfo.BWUserAuthenticator.AuthenticatorType);

            if (authenticatorType == null)
            {
                throw new ArgumentOutOfRangeException("authenticatorType is null");
            }

            BaseReturnModel isVerifyCodeValid = _bwAuthenticatorService.IsVerifyCodeValid(new ValidVerifyCodeParam()
            {
                UserId = userId,
                VerifyCode = loginDetailParam.AuthenticatorCode,
                IsCompareExactly = false
            });

            if (!isVerifyCodeValid.IsSuccess)
            {
                loginResult.LoginStatus = LoginStatuses.AuthenticatorPinIsNotValid;

                return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.ValidateCodeIncorrect, loginResult);
            }

            loginResult.LoginStatus = LoginStatuses.Success;

            string userKey = LoginKeyUtil.Create(s_application, userId);

            var backSideWebUser = new BackSideWebUser()
            {
                UserId = userId,
                UserName = loginDetailParam.UserName,
                UserKey = userKey,
            };

            backSideWebUser.PermissionMap = _bwRoleInfoService.GetUserRolePermissions(userId);

            _backSideWebUserService.SetLoginCache(userKey, backSideWebUser);
            _backSideWebUserService.SignIn(backSideWebUser, doHttpSignIn);

            return new BaseReturnDataModel<BWLoginResultParam>(ReturnCode.Success, loginResult);
        }

        private bool IsExpired(DateTime utcTime)
        {
            if (DateTime.UtcNow > utcTime)
            {
                return DateTime.UtcNow.Subtract(utcTime).TotalSeconds > s_loginStringExpiredSeconds;
            }

            return true;
        }
    }
}