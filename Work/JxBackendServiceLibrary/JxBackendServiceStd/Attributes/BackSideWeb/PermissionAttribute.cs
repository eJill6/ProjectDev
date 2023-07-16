using JxBackendService.Attributes.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.User;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace JxBackendService.Attributes.BackSideWeb
{
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly INoPermissionActionService _noPermissionActionService;

        private readonly PermissionKeys? _permissionKey;

        private readonly AuthorityTypes _authorityType = AuthorityTypes.Read;

        public PermissionAttribute(PermissionKeys permissionKey) : this(permissionKey, AuthorityTypes.Read)
        {
        }

        public PermissionAttribute(PermissionKeys permissionKey, AuthorityTypes authorityType)
        {
            _permissionKey = permissionKey;
            _authorityType = authorityType;
            _noPermissionActionService = DependencyUtil.ResolveService<INoPermissionActionService>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.HasAllowAnonymous();

            if (hasAllowAnonymous)
            {
                return;
            }

            var backSideWebLoginUserService = DependencyUtil.ResolveServiceForModel<IBackSideWebUserService>(JxApplication.BackSideWeb);
            BackSideWebUser backSideWebUser = backSideWebLoginUserService.GetUser();

            if (backSideWebUser == null)
            {
                context.Result = _noPermissionActionService.GetRedirectToLoginPage();

                return;
            }

            bool hasPermission = false;

            if (_permissionKey.HasValue)
            {
                hasPermission = backSideWebLoginUserService.HasPermission(_permissionKey.Value, _authorityType);
            }

            if (!hasPermission)
            {
                var httpContextService = DependencyUtil.ResolveService<IHttpContextService>();

                if (httpContextService.IsAjaxRequest())
                {
                    context.Result = _noPermissionActionService.GetNoPermissionJsonResult();

                    return;
                }
                else
                {
                    context.Result = _noPermissionActionService.GetRedirectToNoPermissionPage();

                    return;
                }
            }
        }
    }
}