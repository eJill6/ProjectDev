using ControllerShareLib.Models.Account;
using JxBackendService.Attributes;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.MiseLive.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SLPolyGame.Web.Interface;
using System.Net;
using Web.Controllers;

namespace Web.Infrastructure.Filters
{
    public class LogOnRateLimitAttribute : BaseFrontSideRequestRateLimitAttribute
    {
        private static readonly string s_errorMsg = "Logon存取过于频繁";

        private static readonly decimal s_retrySeconds = 1.5m;

        private static readonly double s_logOnIntervalSeconds = 1;

        protected override HttpStatusCode RejectExecutingStatus => HttpStatusCode.OK;

        public LogOnRateLimitAttribute() : base(intervalSeconds: s_logOnIntervalSeconds, isWithLock: true)
        {
        }

        protected override void DoRejectExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var miseLiveResponse = new MiseLiveResponse<object>()
                {
                    Success = false,
                    Error = s_errorMsg
                };

                filterContext.Result = new JsonResult(miseLiveResponse);
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = nameof(PublicController.EnterGameLoading),
                    ViewData = (filterContext.Controller as Controller).ViewData
                };

                HttpRequest request = filterContext.HttpContext.Request;
                string redirectUrl = $"{request.Path}{request.QueryString}";

                result.ViewData["RedirectAfterSeconds"] = s_retrySeconds;
                result.ViewData["RedirectUrl"] = redirectUrl;
                filterContext.Result = result;
            }
        }

        protected override string GetIdentity(ActionExecutingContext actionExecutingContext)
        {
            foreach (string key in actionExecutingContext.ActionArguments.Keys)
            {
                if (actionExecutingContext.ActionArguments[key] is LogonParam)
                {
                    var logonParam = actionExecutingContext.ActionArguments[key] as LogonParam;

                    return logonParam.UserID.GetValueOrDefault().ToString();
                }
            }

            return null;
        }

        protected override bool IsAllowExecuting(string key, bool isWithLock)
        {
            var publicApiWebSVService = DependencyUtil.ResolveService<IPublicApiWebSVService>().Value;

            if (isWithLock)
            {
                return publicApiWebSVService.IsAllowExecutingActionWithLock(key, IntervalSeconds).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                return publicApiWebSVService.IsAllowExecutingAction(key, IntervalSeconds).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}