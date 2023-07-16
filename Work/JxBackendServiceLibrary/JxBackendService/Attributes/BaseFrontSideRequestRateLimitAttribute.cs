using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using System.Net;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    /// <summary>
    /// Request頻率限制
    /// </summary>
    public abstract class BaseFrontSideRequestRateLimitAttribute : ActionFilterAttribute
    {
        protected bool IsLimitByUserKey { get; set; }
        
        protected int IntervalSeconds { get; set; }

        protected abstract string GetUserKey();
        
        protected abstract bool IsAllowExecuting(string key);

        protected virtual HttpStatusCode RejectExecutingStatus => HttpStatusCode.BadRequest;

        protected abstract void DoRejectExecuting(ActionExecutingContext filterContext);

        public BaseFrontSideRequestRateLimitAttribute(bool isLimitByUserKey = true, int intervalSeconds = 1)
        {
            IsLimitByUserKey = isLimitByUserKey;
            IntervalSeconds = intervalSeconds;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string identity;

            if (IsLimitByUserKey)
            {
                identity = GetUserKey();
            }
            else
            {
                identity = IpUtil.GetDoWorkIP();
            }

            string controllerName = RouteUtil.GetControllerName();
            string actionName = RouteUtil.GetActionName();

            string key = $"{controllerName}:{actionName}:{identity}";

            bool isAllow = false; ;

            if (!identity.IsNullOrEmpty())
            {
                isAllow = IsAllowExecuting(key);
            }

            if (!isAllow)
            {
                filterContext.HttpContext.Response.StatusCode = (int)RejectExecutingStatus;
                DoRejectExecuting(filterContext);                
            }
        }

        
    }
}

