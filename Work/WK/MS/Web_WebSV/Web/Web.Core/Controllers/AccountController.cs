using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Account;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Param.Client;
using JxBackendService.Model.Param.ThirdParty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using Web.Infrastructure.Filters;

namespace Web.Controllers
{
    public class AccountController : BaseController
    {
        private static readonly string s_nativeAppDepositUrl = "seal://common/webview?type=deposit";

        private readonly Lazy<IRouteUtilService> _routeUtilService;

        private readonly Lazy<IAccountControllerService> _accountControllerService;

        public AccountController()
        {
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();
            _accountControllerService = DependencyUtil.ResolveService<IAccountControllerService>();
        }

        /// <summary>
        /// Logon view post.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [WebValidModelState(HttpStatusCode.BadRequest)]
        [LogDebugUserActionExecutingTime]
        public async Task<IActionResult> LogOn(LogonParam logonParam)
        {
            ClientWebPage clientWebPage;

            if (!logonParam.ClientWebPageValue.IsNullOrEmpty())
            {
                clientWebPage = ClientWebPage.GetSingle(logonParam.ClientWebPageValue);

                if (clientWebPage == null)
                {
                    return await Task.FromResult(new StatusCodeResult((int)HttpStatusCode.BadRequest));
                }
            }
            else
            {
                clientWebPage = ClientWebPage.LotterySpa;
            }

            if (logonParam.DepositUrl.IsNullOrEmpty())
            {
                logonParam.DepositUrl = s_nativeAppDepositUrl;
            }

            var validateLogonParam = logonParam.CastByJson<ValidateLogonParam>();
            validateLogonParam.UserKeyExpiredMinutes = EnvLoginUser.Application.UserKeyExpiredMinutes;
            validateLogonParam.IsSlidingExpiration = EnvLoginUser.Application.IsSlidingUserKeyCache;

            LogonResult logonResult = _accountControllerService.Value.LogOn(validateLogonParam);

            string token = logonResult.Token;
            string controllerName = clientWebPage.Controller;
            string actionName = clientWebPage.Action;

            Dictionary<string, object> routeValues = ConvertToRouteValues(clientWebPage, logonParam);
            routeValues.Add(RouteUtil.RouteMiseWebTokenName, token);
            string url = _routeUtilService.Value.GetMiseWebTokenUrl(actionName, controllerName, routeValues);

            if (clientWebPage.LogonViewWaitSeconds.HasValue)
            {
                if(url.Contains("MM/Index") || url.Contains("LotterySpa/Index"))
                {
                    ViewBag.RedirectUrl = "";
                    ViewBag.EncPath = XorEncryptTool.XorEncryptToString(url);
                }
                else
                {
                    ViewBag.RedirectUrl = url;
                }
                
                ViewBag.RedirectAfterSeconds = clientWebPage.LogonViewWaitSeconds.Value;

                return await Task.FromResult(View(nameof(PublicController.EnterGameLoading)));
            }

            return await Task.FromResult(new RedirectResult(url));
        }

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
                routeValueDictionary.Add("pageParamInfo", logonParam.PageParamInfo);
            }
            else if (clientWebPage == ClientWebPage.PMEBAnchorRoom)
            {
                routeValueDictionary.Add(nameof(BaseGameCenterLogin.RemoteCode), logonParam.AnchorId);
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