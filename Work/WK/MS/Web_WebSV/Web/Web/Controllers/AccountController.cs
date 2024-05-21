using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.Model.Param.Client;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Interface.Service.Net;
using System.Net;
using System.Web.Mvc;
using Web.Cookie;
using Web.Helpers.Security;
using Web.Infrastructure.Filters;
using Web.Models.Account;
using Web.Services;
using JxBackendService.DependencyInjection;
using System.Collections.Generic;
using SLPolyGame.Web.Model;

namespace Web.Controllers
{
    public class AccountController : BaseController
    {
        protected readonly IPlayInfoService _playInfoService = null;

        private readonly IHttpCookieService _httpCookieService;

        private readonly IRouteUtilService _routeUtilService;

        public AccountController(IUserService userService,
            ICacheService cacheService,
            IPlayInfoService playInfoService,
            IHttpCookieService httpCookieService) : base(cacheService, userService)
        {
            _playInfoService = playInfoService;
            _httpCookieService = httpCookieService;
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();
        }

        /// <summary>
        /// Logon view post.
        /// </summary>
        [HttpGet]
        [Anonymous]
        [WebValidModelState(HttpStatusCode.BadRequest)]
        [LogDebugUserActionExecutingTime]
        [LogOnRateLimit()]
        public ActionResult LogOn(LogonParam logonParam)
        {
            ClientWebPage clientWebPage;

            if (!logonParam.ClientWebPageValue.IsNullOrEmpty())
            {
                clientWebPage = ClientWebPage.GetSingle(logonParam.ClientWebPageValue);

                if (clientWebPage == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                clientWebPage = ClientWebPage.LotterySpa;
            }

            UserAuthInformation userAuthInformation = _userService.ValidateLogin(new LoginRequestParam
            {
                UserId = logonParam.UserID.Value,
                UserName = logonParam.UserName,
                RoomNo = logonParam.RoomNo,
                GameID = logonParam.GameID,
                DepositUrl = logonParam.DepositUrl,
                LogonMode = logonParam.LogonMode
            });

            var basicUserInfo = new BasicUserInfo()
            {
                UserId = userAuthInformation.UserId,
                UserKey = userAuthInformation.Key
            };

            string token = AuthenticationUtil.CreateMiseWebToken(basicUserInfo);

            string controllerName = clientWebPage.Controller;
            string actionName = clientWebPage.Action;

            Dictionary<string, object> routeValues = ConvertToRouteValues(clientWebPage, logonParam);
            routeValues.Add(RouteUtil.RouteMiseWebTokenName, token);
            string url = _routeUtilService.GetMiseWebTokenUrl(actionName, controllerName, routeValues);

            if (clientWebPage.HasLogonView)
            {
                ViewBag.RedirectUrl = url;

                return View(nameof(PublicController.EnterGameLoading));
            }

            return Redirect(url);
        }

        ///// <summary>
        ///// 取得最新餘額json
        ///// </summary>
        //[HttpPost]
        //public ActionResult GetRefreshToken()
        //{
        //    var model = AuthenticationUtil.GetTokenModel();
        //    return Json(new
        //    {
        //        UserKey = TokenProvider.GenerateTokenString(model.Key, model.UserName),
        //    }, JsonRequestBehavior.AllowGet);
        //}

        private Dictionary<string, object> ConvertToRouteValues(ClientWebPage clientWebPage, LogonParam logonParam)
        {
            var routeValueDictionary = new Dictionary<string, object>();

            if (clientWebPage.RouteValues != null)
            {
                routeValueDictionary = RouteUtil.ConvertToRouteValues(clientWebPage.RouteValues);
            }

            if (clientWebPage == ClientWebPage.LotterySpa)
            {
                routeValueDictionary.Add("orderNo", logonParam.OrderNo);
            }
            else if (clientWebPage == ClientWebPage.PMEBAnchorRoom)
            {
                routeValueDictionary.Add(nameof(GameCenterLogin.RemoteCode), logonParam.AnchorId);
            }
            else if (clientWebPage == ClientWebPage.EnterThirdPartyGame)
            {
                routeValueDictionary.Add("gameId", logonParam.GameID);
                routeValueDictionary.Add("remoteCode", logonParam.PageParamInfo);
            }
            else if (clientWebPage == ClientWebPage.MM)
            {
                routeValueDictionary.Add("pageParamInfo", logonParam.PageParamInfo);
            }

            return routeValueDictionary;
        }
    }
}