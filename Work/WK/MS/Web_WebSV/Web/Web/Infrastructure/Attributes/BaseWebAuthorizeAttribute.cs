using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Infrastructure.Filters;
using Web.Models.Base;

namespace Web.Infrastructure.Attributes
{
    public abstract class BaseWebAuthorizeAttribute : AuthorizeAttribute
    {
        protected BaseWebAuthorizeAttribute()
        {
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                int httpStatusCode = (int)HttpStatusCode.Forbidden;
                var result = new { code = httpStatusCode, error = "登录已过期！" };

                filterContext.Result = new JsonNetResult(result);
                filterContext.HttpContext.Response.StatusCode = httpStatusCode;
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected abstract ActionResult GetUnauthorizedActionResult();

        protected abstract bool DoAuthorizeCore(HttpContextBase httpContext);

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var hasAnonymous = filterContext
                .ActionDescriptor
                .GetCustomAttributes(typeof(AnonymousAttribute), false)
                .Any();

            if (hasAnonymous)
            {
                return;
            }

            if (!DoAuthorizeCore(filterContext.HttpContext))
            {
                filterContext.Result = GetUnauthorizedActionResult();

                return;
            }

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return DoAuthorizeCore(httpContext);
        }
    }
}