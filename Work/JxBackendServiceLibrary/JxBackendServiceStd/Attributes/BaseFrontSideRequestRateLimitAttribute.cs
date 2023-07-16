using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace JxBackendService.Attributes
{
    /// <summary>
    /// Request頻率限制
    /// </summary>
    public abstract class BaseFrontSideRequestRateLimitAttribute : ActionFilterAttribute
    {
        private readonly bool _isWithLock;

        protected bool IsLimitByIdentity { get; set; }

        protected double IntervalSeconds { get; set; }

        protected abstract string GetIdentity(ActionExecutingContext actionExecutingContext);

        protected abstract string GetActionName(ActionExecutingContext actionExecutingContext);

        protected abstract string GetControllerName(ActionExecutingContext actionExecutingContext);

        protected abstract bool IsAllowExecuting(string key, bool isWithLock);

        protected virtual HttpStatusCode RejectExecutingStatus => HttpStatusCode.BadRequest;

        protected abstract void DoRejectExecuting(ActionExecutingContext actionExecutingContext);

        public BaseFrontSideRequestRateLimitAttribute(bool isLimitByUserKey = true, double intervalSeconds = 1, bool isWithLock = false)
        {
            IsLimitByIdentity = isLimitByUserKey;
            IntervalSeconds = intervalSeconds;
            _isWithLock = isWithLock;
        }

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            base.OnActionExecuting(actionExecutingContext);

            string identity;

            if (IsLimitByIdentity)
            {
                identity = GetIdentity(actionExecutingContext);
            }
            else
            {
                var ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
                identity = ipUtilService.GetIPAddress();
            }

            string controllerName = GetControllerName(actionExecutingContext);
            string actionName = GetActionName(actionExecutingContext);

            string key = $"{controllerName}:{actionName}:{identity}";

            bool isAllow = false; ;

            if (!identity.IsNullOrEmpty())
            {
                isAllow = IsAllowExecuting(key, _isWithLock);
            }

            if (!isAllow)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                logUtilService.ForcedDebug($"{GetType().Name}: {key} invalid");

                actionExecutingContext.HttpContext.Response.StatusCode = (int)RejectExecutingStatus;
                DoRejectExecuting(actionExecutingContext);
            }
        }
    }
}