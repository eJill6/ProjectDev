using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.MiseLive.Response;
using SLPolyGame.Web.Interface;
using System;
using System.Net;
using System.Web.Mvc;
using Web.Controllers;
using Web.Models.Account;
using Web.Services;

namespace Web.Infrastructure.Filters
{
    public class LogOnRateLimitAttribute : BaseFrontSideRequestRateLimitNFAttribute
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
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new MiseLiveResponse<object>()
                    {
                        Success = false,
                        Error = s_errorMsg
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = nameof(PublicController.EnterGameLoading),
                    ViewData = filterContext.Controller.ViewData
                };

                result.ViewBag.RedirectAfterSeconds = s_retrySeconds;
                result.ViewBag.RedirectUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = result;
            }
        }

        protected override string GetIdentity(ActionExecutingContext actionExecutingContext)
        {
            foreach (string key in actionExecutingContext.ActionParameters.Keys)
            {
                if (actionExecutingContext.ActionParameters[key] is LogonParam)
                {
                    var logonParam = actionExecutingContext.ActionParameters[key] as LogonParam;

                    return logonParam.UserID.ToString();
                }
            }

            return null;
        }

        protected override bool IsAllowExecuting(string key, bool isWithLock)
        {
            var publicApiWebSVService = DependencyUtil.ResolveService<IPublicApiWebSVService>();

            if (isWithLock)
            {
                return publicApiWebSVService.IsAllowExecutingActionWithLock(key, IntervalSeconds);
            }
            else
            {
                return publicApiWebSVService.IsAllowExecutingAction(key, IntervalSeconds);
            }
        }
    }
}