using BackSideWeb.Controllers.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using JxBackendService.Common.Util;

namespace BackSideWeb.Controllers
{
    public class RoleManagementController : BaseCRUDController<QueryBWRoleInfoParam, CreateRoleInfoParam, UpdateRolePermissionParam>
    {
        private readonly Lazy<IBWRoleInfoService> _bwRoleInfoService;

        protected override string[] PageJavaScripts => new string[]
        {
            "business/systemSetting/bwRoleManagementService.min.js",
        };

        protected override string ClientServiceName => "bwRoleManagementService";

        protected override string ClientEditSingleRowServiceName => "bwRoleEditPermissionService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.RoleManagement;

        public RoleManagementController()
        {
            _bwRoleInfoService = DependencyUtil.ResolveJxBackendService<IBWRoleInfoService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        protected override IActionResult GetInsertView()
        {
            return GetEditView(new EditRolePermission());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            EditRolePermission rolePermission = _bwRoleInfoService.Value.GetEditRoleInfo(keyContent.ToInt32());

            return GetEditView(rolePermission);
        }

        protected override BaseReturnModel DoDelete(string keyContent) => _bwRoleInfoService.Value.Delete(keyContent.ToInt32());

        public override ActionResult GetGridViewResult(QueryBWRoleInfoParam queryParam)
        {
            PagedResultModel<RoleManagementViewModel> model = _bwRoleInfoService.Value.GetPagedBWRoleInfos(queryParam);

            return PartialView(model);
        }

        public override ActionResult Index()
        {
            base.Index();

            return View(new QueryBWRoleInfoParam());
        }

        protected override BaseReturnModel DoInsert(CreateRoleInfoParam insertModel)
        {
            BaseReturnDataModel<int> returnModel = _bwRoleInfoService.Value.Create(insertModel);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            string redirectUrl = Url.Action(nameof(UpdateView), new { keyContent = returnModel.DataModel });

            return new BaseReturnDataModel<object>(ReturnCode.Success, new { url = redirectUrl });
        }

        protected override BaseReturnModel DoUpdate(UpdateRolePermissionParam updateModel)
        {
            return _bwRoleInfoService.Value.SaveRolePermission(updateModel);
        }

        private IActionResult GetEditView(EditRolePermission model)
        {
            return View("Edit", model);
        }
    }
}