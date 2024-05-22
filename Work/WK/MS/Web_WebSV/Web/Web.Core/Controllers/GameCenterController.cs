using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using Web.Infrastructure.Filters;

namespace Web.Controllers
{
    /// <summary>第三方遊戲controller</summary>
    [LogDebugUserActionExecutingTime]
    public class GameCenterController : BaseController
    {
        private static readonly bool s_isMobile = true;

        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        private readonly Lazy<IThirdPartyApiWebSVService> _thirdPartyApiWebSVService;

        private readonly Lazy<IRouteUtilService> _routeUtilService;

        private readonly Lazy<IGameCenterControllerService> _gameCenterControllerService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        /// <summary></summary>
        public GameCenterController()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            _thirdPartyApiWebSVService = DependencyUtil.ResolveService<IThirdPartyApiWebSVService>();
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();
            _gameCenterControllerService = DependencyUtil.ResolveService<IGameCenterControllerService>();

            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                Application,
                SharedAppSettings.PlatformMerchant);
        }

        /// <summary>轉導第三方遊戲入口</summary>
        public IActionResult Index(WebGameCenterLogin gameCenterLogin)
        {
            var frontSideMainMenu = new FrontSideMainMenu()
            {
                ProductCode = gameCenterLogin.ProductCode,
                GameCode = gameCenterLogin.GameCode
            };

            //檢查menu是否開啟
            if (!_slPolyGameWebSVService.Value.IsFrontsideMenuActive(frontSideMainMenu).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                return View("Error");
            }

            string loginInfoJson = new LoginInfo()
            {
                GameCode = gameCenterLogin.GameCode,
                RemoteCode = gameCenterLogin.RemoteCode
            }.ToJsonString();

            //依照Code去取得登入網址
            BaseReturnDataModel<TPGameOpenParam> result = _thirdPartyApiWebSVService.Value.GetForwardGameUrl(
                new ForwardGameUrlSVApiParam()
                {
                    ProductCode = gameCenterLogin.ProductCode,
                    LoginInfoJson = loginInfoJson,
                    IsMobile = s_isMobile,
                    CorrelationId = Guid.NewGuid().ToString()
                });

            if (!result.IsSuccess)
            {
                return View("Error");
            }

            OpenGameMode openGameMode = OpenGameMode.GetSingle(result.DataModel.OpenGameModeValue);

            return OpenGame(openGameMode, result.DataModel.Url);
        }

        public JsonResult GetForwardGameUrl(WebGameCenterLogin webGameCenterLogin)
        {
            BaseReturnDataModel<CommonOpenUrlInfo> returnDataModel = _gameCenterControllerService.Value.GetWebForwardGameUrl(webGameCenterLogin);

            if (!returnDataModel.IsSuccess)
            {
                return PascalCaseJson(returnDataModel);
            }

            AppOpenUrlInfo appOpenUrlInfo = returnDataModel.DataModel;
            var logonMode = LogonMode.GetSingle(AuthenticationUtil.GetLoginUserFromCache().LogonMode);

            string redirectUrl = ToFullScreenUrlByDebugSetting(
                appOpenUrlInfo.Url,
                appOpenUrlInfo.IsHideHeaderWithFullScreen,
                webGameCenterLogin.Title,
                logonMode);

            return PascalCaseJson(new BaseReturnDataModel<string>(ReturnCode.Success, redirectUrl));
        }

        public IActionResult EnterThirdPartyGame(EnterThirdPartyGameParam enterThirdPartyGameParam)
        {
            BaseReturnDataModel<EnterTPGameUrlInfo> returnDataModel = _gameCenterControllerService.Value.GetWebEnterThirdPartyGame(enterThirdPartyGameParam);

            if (!returnDataModel.IsSuccess)
            {
                return View("Error");
            }

            if (!returnDataModel.DataModel.GameLobbyTypeValue.IsNullOrEmpty() && !returnDataModel.DataModel.Url.IsNullOrEmpty())
            {
                List<string> pathNames = returnDataModel.DataModel.Url.Split('/').Where(w => !w.IsNullOrEmpty()).ToList();
                string url = _routeUtilService.Value.GetMiseWebTokenUrl(action: pathNames[1], controller: pathNames[0], routeParams: null);

                return Redirect(url);
            }

            OpenGameMode openGameMode = OpenGameMode.GetSingle(returnDataModel.DataModel.OpenGameModeValue);

            return OpenGame(openGameMode, returnDataModel.DataModel.Url);
        }

        [AllowAnonymous]
        public ActionResult LaunchURLHTML(string token)
        {
            if (token.IsNullOrEmpty())
            {
                return RedirectToReconnectTips();
            }

            string response = _thirdPartyApiWebSVService.Value.GetTPGameLaunchURLHTML(token);

            if (response.IsNullOrEmpty())
            {
                return RedirectToReconnectTips();
            }

            ViewBag.UrlHtml = response;

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate"; // 設置不允許緩存

            return View();
        }

        private ActionResult RedirectToReconnectTips()
        {
            return Redirect("/ReconnectTips");
        }

        private IActionResult OpenGame(OpenGameMode openGameMode, string url)
        {
            if (openGameMode == OpenGameMode.IFrame)
            {
                ViewBag.URL = url;

                return View("~/Views/Shared/ThirdPartyGame.cshtml");
            }
            else if (openGameMode == OpenGameMode.Redirect)
            {
                return Redirect(url);
            }

            throw new NotSupportedException();
        }
    }
}