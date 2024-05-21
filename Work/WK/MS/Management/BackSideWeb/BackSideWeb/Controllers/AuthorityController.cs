using BackSideWeb.Controllers.Base;
using BackSideWebLoginToolService.Model;
using JxBackendService.Attributes;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Security;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BackSideWeb.Controllers
{
    public class AuthorityController : BaseController
    {
        private readonly Lazy<IBackSideWebUserService> _backSideWebUserService;

        private readonly Lazy<IBWLoginDetailService> _bwLoginDetailService;

        private readonly Lazy<IBackSideWebLoginService> _backSideWebLoginService;

        public AuthorityController()
        {
            _backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>();
            _bwLoginDetailService = DependencyUtil.ResolveJxBackendService<IBWLoginDetailService>(EnvLoginUser, DbConnectionTypes.Master);
            _backSideWebLoginService = DependencyUtil.ResolveJxBackendService<IBackSideWebLoginService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            SetPageTitle(CommonElement.Login);

            return View(new LoginParam());
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [AjaxValidModelState]
        public IActionResult Login(LoginParam loginParam)
        {
            SetPageTitle(CommonElement.Login);
            BWLoginToolParam loginParamModel = null;

            var returnDataModel = new BaseReturnDataModel<BWLoginResultParam>
            {
                IsSuccess = false
            };

            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                var desTool = new DESTool(LoginParamSettings.Key, LoginParamSettings.Iv);
                loginParamModel = desTool.DESDeCode(loginParam.EncryptLoginString).Deserialize<BWLoginToolParam>();
            });

            if (loginParamModel == null)
            {
                ViewBag.ErrorMessage = MessageElement.LoginCodeFormatIsNotValid;

                return View();
            }

            var loginDetailParam = new LoginDetailParam()
            {
                UserName = loginParamModel.UserId,
                UserPWD = loginParamModel.UserPWD,
                AuthenticatorCode = loginParamModel.AuthenticatorCode,
                MachineName = loginParamModel.MachineName,
                WinLoginName = loginParamModel.WinLoginName,
                LoginToolVersion = loginParamModel.LoginToolVersion,
                UTCTime = loginParamModel.UTCTime
            };

            BaseReturnDataModel<BWLoginResultParam> returnModel = _backSideWebLoginService.Value.Login(
                loginDetailParam,
                (principal, authenticationProperties) =>
                {
                    HttpContext.SignInAsync(principal, authenticationProperties).Wait();
                });

            if (returnModel.DataModel != null)
            {
                _bwLoginDetailService.Value.InsertLoginDetail(returnModel.DataModel);
            }

            if (!returnModel.IsSuccess)
            {
                ViewBag.ErrorMessage = returnModel.Message;

                return View();
            }

            return DoRedirect(loginParam.ReturnUrl);
        }

        public IActionResult Logout()
        {
            return _backSideWebUserService.Value.Logout(() => HttpContext.SignOutAsync().Wait());
        }

        private IActionResult DoRedirect(string redirectUrl)
        {
            if (redirectUrl.IsNullOrEmpty())
            {
                redirectUrl = Url.Action(nameof(HomeController.Index), nameof(HomeController).RemoveControllerNameSuffix());
            }

            return LocalRedirect(redirectUrl);
        }
    }
}