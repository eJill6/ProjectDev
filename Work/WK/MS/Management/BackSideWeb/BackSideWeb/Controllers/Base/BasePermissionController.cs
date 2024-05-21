using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackSideWeb.Controllers.Base
{
    public abstract class BasePermissionController : BaseAuthController
    {
        private readonly Lazy<INoPermissionActionService> _noPermissionActionService;

        public BasePermissionController()
        {
            _noPermissionActionService = DependencyUtil.ResolveService<INoPermissionActionService>();
        }

        protected bool HasPermission(PermissionKeys permissionKey) => HasPermission(permissionKey, AuthorityTypes.Read);

        protected bool HasPermission(PermissionKeys permissionKey, AuthorityTypes authorityType)
        {
            var backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>().Value;

            return backSideWebUserService.HasPermission(permissionKey, authorityType);
        }

        protected abstract PermissionKeys GetPermissionKey();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            PermissionKeys permissionKey = GetPermissionKey();
            ViewBag.PermissionKeyDetail = PermissionKeyDetail.GetSingle(permissionKey);

            if (!HasPermission(permissionKey))
            {
                SetNoPermissionResult(filterContext);

                return;
            }
        }

        protected void SetNoPermissionResult(ActionExecutingContext filterContext)
        {
            var httpContextService = DependencyUtil.ResolveService<IHttpContextService>().Value;

            if (httpContextService.IsAjaxRequest())
            {
                filterContext.Result = _noPermissionActionService.Value.GetNoPermissionJsonResult();
            }
            else
            {
                filterContext.Result = _noPermissionActionService.Value.GetRedirectToNoPermissionPage();
            }
        }
    }
}