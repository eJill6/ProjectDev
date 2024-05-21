using BackSideWeb.Controllers;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BackSideWeb.Filters
{
    public class CustomizedAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly Lazy<IBackSideWebUserService> _backSideWebLoginUserService;

        public CustomizedAuthorizeAttribute()
        {
            _backSideWebLoginUserService = DependencyUtil.ResolveService<IBackSideWebUserService>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_backSideWebLoginUserService.Value.GetUser().UserName.IsNullOrEmpty())
            {
                SetUnauthorizedResult(context);

                return;
            }
        }

        protected void SetUnauthorizedResult(AuthorizationFilterContext context)
        {
            var httpContextService = DependencyUtil.ResolveService<IHttpContextService>().Value;

            if (httpContextService.IsAjaxRequest())
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else
            {
                context.Result = new RedirectToActionResult(
                    nameof(AuthorityController.Login),
                    nameof(AuthorityController).RemoveControllerNameSuffix(),
                    routeValues: null);
            }
        }
    }
}