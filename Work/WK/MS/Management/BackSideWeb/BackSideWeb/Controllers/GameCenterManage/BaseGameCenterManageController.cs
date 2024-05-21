using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.Enums;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackSideWeb.Controllers.GameCenterManage
{
    public abstract class BaseGameCenterManageController<T> : BaseSearchGridController<T>
    {
        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        private readonly Lazy<IFrontsideMenuTypeService> _frontsideMenuTypeService;

        protected IFrontsideMenuService FrontsideMenuService => _frontsideMenuService.Value;

        protected IFrontsideMenuTypeService FrontsideMenuTypeService => _frontsideMenuTypeService.Value;

        protected override string[] BaseJavaScripts
        {
            get
            {
                List<string> baseJavaScripts = base.BaseJavaScripts.ToList();
                baseJavaScripts.Add("base/crud/baseReturnModel.min.js");
                baseJavaScripts.Add("business/gameCenterManage/baseGameCenterManageService.js");
                return baseJavaScripts.ToArray();
            }
        }

        public BaseGameCenterManageController()
        {
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
            _frontsideMenuTypeService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuTypeService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        public override ActionResult Index()
        {
            SetLayout(LayoutType.GameCenterManage);
            ViewBag.GameManagePageUrl = Url.Action(nameof(GameManageController.Index), nameof(GameManageController).RemoveControllerNameSuffix());
            ViewBag.GameTypeManagePageUrl = Url.Action(nameof(GameTypeManageController.Index), nameof(GameTypeManageController).RemoveControllerNameSuffix());

            return base.Index();
        }

        [HttpPost]
        public abstract BaseReturnModel Update([FromBody] List<GameCenterUpdateParam> param);

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.GameCenterManage;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string actionName = RouteUtilService.GetActionName();

            bool hasEditPermission = HasPermission(GetPermissionKey(), AuthorityTypes.Edit);
            ViewBag.HasEditPermission = hasEditPermission;

            if (hasEditPermission)
            {
                PageApiUrlSetting.UpdateApiUrl = Url.Action(nameof(Update));
            }
            else if (actionName == nameof(Update))
            {
                SetNoPermissionResult(filterContext);
            }
        }
    }
}