using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using SLPolyGame.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers.Security;
using Web.Infrastructure.Filters;
using Web.Models.Base;
using Web.Services;
using Web.SLPolyGameService;
using Web.ThirdPartyApiService;

namespace Web.Controllers
{
    /// <summary>第三方遊戲controller</summary>
    [LogDebugUserActionExecutingTime]
    public class GameCenterController : BaseController
    {
        private static readonly bool s_isMobile = true;

        private readonly ISLPolyGameWebSVService _slPolyGameWebSVService;

        private readonly IThirdPartyApiWebSVService _thirdPartyApiWebSVService;

        private readonly IRouteUtilService _routeUtilService;

        private readonly IPlatformProductService _platformProductService;

        /// <summary></summary>
        public GameCenterController(IUserService userService,
            ICacheService cacheService) : base(cacheService, userService)
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            _thirdPartyApiWebSVService = DependencyUtil.ResolveService<IThirdPartyApiWebSVService>();
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();

            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                JxApplication.FrontSideWeb,
                SharedAppSettings.PlatformMerchant);
        }

        /// <summary>轉導第三方遊戲入口</summary>
        public ActionResult Index(GameCenterLogin gameCenterLogin)
        {
            //檢查menu是否開啟
            if (!_slPolyGameWebSVService.IsFrontsideMenuActive(gameCenterLogin.ProductCode))
            {
                return View("Error");
            }

            string loginInfoJson = new LoginInfo()
            {
                GameCode = gameCenterLogin.GameCode,
                RemoteCode = gameCenterLogin.RemoteCode
            }.ToJsonString();

            //依照Code去取得登入網址
            BaseReturnDataModel<JxBackendService.Model.Param.ThirdParty.TPGameOpenParam> result = _thirdPartyApiWebSVService.GetForwardGameUrl(
                gameCenterLogin.ProductCode,
                loginInfoJson,
                s_isMobile,
                correlationId: Guid.NewGuid().ToString());

            if (!result.IsSuccess)
            {
                return View("Error");
            }

            OpenGameMode openGameMode = OpenGameMode.GetSingle(result.DataModel.OpenGameModeValue);

            if (openGameMode == OpenGameMode.IFrame)
            {
                ViewBag.URL = result.DataModel.Url;

                return View("~/Views/Shared/ThirdPartyGame.cshtml");
            }
            else if (openGameMode == OpenGameMode.Redirect)
            {
                return Redirect(result.DataModel.Url);
            }

            throw new NotSupportedException();
        }

        public JsonNetResult GetForwardGameUrl(GameCenterLogin gameCenterLogin)
        {
            //檢查menu是否開啟
            if (!_slPolyGameWebSVService.IsFrontsideMenuActive(gameCenterLogin.ProductCode))
            {
                return new JsonNetResult(new BaseReturnDataModel<string>(ThirdPartyGameElement.GameMaintain));
            }

            string loginInfoJson = new LoginInfo()
            {
                GameCode = gameCenterLogin.GameCode,
                RemoteCode = gameCenterLogin.RemoteCode
            }
            .ToJsonString();

            //依照Code去取得登入網址
            BaseReturnDataModel<JxBackendService.Model.Param.ThirdParty.TPGameOpenParam> result = _thirdPartyApiWebSVService.GetForwardGameUrl(
                gameCenterLogin.ProductCode,
                loginInfoJson,
                s_isMobile,
                correlationId: Guid.NewGuid().ToString());

            if (!result.IsSuccess)
            {
                return new JsonNetResult(new BaseReturnDataModel<string>(result.Message));
            }

            PlatformProduct product = _platformProductService.GetSingle(gameCenterLogin.ProductCode);
            bool isHideHeaderWithFullScreen = product.ProductType.IsHideHeaderWithFullScreen;
            var logonMode = LogonMode.GetSingle(AuthenticationUtil.GetLoginUserFromCache().LogonMode);

            result.DataModel.Url = ToFullScreenUrlByDebugSetting(result.DataModel.Url,
                isHideHeaderWithFullScreen,
                gameCenterLogin.Title,
                logonMode);

            return new JsonNetResult(new BaseReturnDataModel<string>(ReturnCode.Success, result.DataModel.Url));
        }

        public ActionResult EnterThirdPartyGame(string gameId, string remoteCode)
        {
            var orderGameId = MiseOrderGameId.GetSingle(gameId);

            if (orderGameId == null)
            {
                return View("Error");
            }

            GameCenterLogin gameCenterLogin = null;

            if (!remoteCode.IsNullOrEmpty())
            {
                gameCenterLogin = new GameCenterLogin
                {
                    ProductCode = orderGameId.Product.Value,
                    GameCode = orderGameId.SubGameCode,
                    RemoteCode = remoteCode,
                };

                return Index(gameCenterLogin);
            }

            //如果有大廳要直接轉往大廳
            JxBackendService.Model.Entity.FrontsideMenu frontsideMenu = _thirdPartyApiWebSVService
                .GetActiveFrontsideMenu(orderGameId.Product.Value, orderGameId.SubGameCode.ToNonNullString());

            if (!frontsideMenu.Url.IsNullOrEmpty())
            {
                List<string> pathNames = frontsideMenu.Url.Split('/').Where(w => !w.IsNullOrEmpty()).ToList();
                string url = _routeUtilService.GetMiseWebTokenUrl(action: pathNames[1], controller: pathNames[0], routeParams: null);

                return Redirect(url);
            }

            gameCenterLogin = new GameCenterLogin
            {
                ProductCode = orderGameId.Product.Value,
                GameCode = orderGameId.SubGameCode,
            };

            return Index(gameCenterLogin);
        }
    }
}