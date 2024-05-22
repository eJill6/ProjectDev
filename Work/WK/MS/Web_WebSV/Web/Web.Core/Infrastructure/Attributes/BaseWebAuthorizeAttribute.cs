using JxBackendService.Attributes.Extensions;
using JxBackendService.Common.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Web.Infrastructure.Attributes
{
    public abstract class BaseWebAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        protected abstract IActionResult GetUnauthorizedPageActionResult();

        protected abstract JsonResult GetUnauthorizedAjaxActionResult();

        protected virtual HttpStatusCode GetUnauthorizedPageStatusCode() => HttpStatusCode.OK;

        protected abstract HttpStatusCode GetUnauthorizedAjaxStatusCode();

        protected abstract bool DoAuthorizeJob(HttpContext httpContext);

        protected BaseWebAuthorizeAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HasAllowAnonymous())
            {
                return;
            }

            if (!DoAuthorizeJob(context.HttpContext))
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = GetUnauthorizedAjaxActionResult();
                    context.HttpContext.Response.StatusCode = (int)GetUnauthorizedAjaxStatusCode();
                }
                else
                {
                    context.HttpContext.Response.StatusCode = (int)GetUnauthorizedPageStatusCode();
                    context.Result = GetUnauthorizedPageActionResult();
                }

                return;
            }
        }
    }
}