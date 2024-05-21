using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.Enums;
using JxBackendService.Attributes.BackSideWeb;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Authenticator;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Authenticator;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class UserManagementController : BaseCRUDController<QueryBWUserInfoParam, CreateBWUserInfoParam, UpdateBWUserInfoParam>
    {
        private readonly Lazy<IBWUserInfoService> _bwUserInfoService;

        private readonly Lazy<IBWRoleInfoService> _bwRoleInfoService;

        private readonly Lazy<IBWAuthenticatorService> _bwAuthenticatorService;

        protected override string[] PageJavaScripts => new string[]
        {
            "business/systemSetting/bwUserManagementService.min.js",
        };

        protected override string ClientServiceName => "bwUserManagementService";

        protected override string ClientEditSingleRowServiceName => "bwUserEditManagementService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.UserManagement;

        public UserManagementController()
        {
            _bwUserInfoService = DependencyUtil.ResolveJxBackendService<IBWUserInfoService>(EnvLoginUser, DbConnectionTypes.Master);
            _bwRoleInfoService = DependencyUtil.ResolveJxBackendService<IBWRoleInfoService>(EnvLoginUser, DbConnectionTypes.Slave);
            _bwAuthenticatorService = DependencyUtil.ResolveJxBackendService<IBWAuthenticatorService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        protected override IActionResult GetInsertView()
        {
            SetRoleSelectListItemsToViewBag(roleId: null);

            return GetEditView(new EditUserInfo());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            EditUserInfo editUserInfo = _bwUserInfoService.Value.GetEditUserInfo(keyContent.ToInt32());
            SetRoleSelectListItemsToViewBag(editUserInfo.RoleID);

            return GetEditView(editUserInfo);
        }

        protected override BaseReturnModel DoInsert(CreateBWUserInfoParam insertModel)
        {
            var returnModel = _bwUserInfoService.Value.Create(insertModel);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            string redirectUrl = Url.Action(nameof(GoogleQRCode), new { keyContent = returnModel.DataModel });

            return new BaseReturnDataModel<object>(ReturnCode.Success, new { url = redirectUrl });
        }

        protected override BaseReturnModel DoUpdate(UpdateBWUserInfoParam updateModel) => _bwUserInfoService.Value.Update(updateModel);

        protected override BaseReturnModel DoDelete(string keyContent) => _bwUserInfoService.Value.Delete(keyContent.ToInt32());

        public override ActionResult GetGridViewResult(QueryBWUserInfoParam queryParam)
        {
            PagedResultModel<UserManagementViewModel> model = _bwUserInfoService.Value.GetPagedBWUserInfos(queryParam);
            bool hasGooglePermission = HasPermission(PermissionKeys.UserManagement, AuthorityTypes.Edit);
            ViewBag.HasGooglePermission = hasGooglePermission;

            return PartialView(model);
        }

        public override ActionResult Index()
        {
            base.Index();

            return View(new QueryBWUserInfoParam());
        }

        [HttpGet]
        [Permission(PermissionKeys.UserManagement, AuthorityTypes.Edit)]
        public IActionResult GoogleQRCode(string keyContent)
        {
            InitPopupEditView("bwGoogleAuthenticatorService");
            SetPageTitle("Google身分验证");
            ViewBag.SubmitUrl = Url.Action(nameof(UpdateGoogleQRCode));

            BaseReturnDataModel<QrCodeViewModel> qrCodeViewModel = _bwAuthenticatorService.Value.GetQrCode(new CreateQrCodeViewModelParam()
            {
                UserId = int.Parse(keyContent),
                IsForcedRefresh = false
            });

            return View(qrCodeViewModel.DataModel);
        }

        public JsonResult UpdateGoogleQRCode(int userId)
        {
            BaseReturnDataModel<QrCodeViewModel> qrCodeViewModel = _bwAuthenticatorService.Value.GetQrCode(new CreateQrCodeViewModelParam()
            {
                UserId = userId,
                IsForcedRefresh = true
            });

            return Json(qrCodeViewModel);
        }

        private void SetRoleSelectListItemsToViewBag(int? roleId)
        {
            List<JxBackendSelectListItem> selectListItems = _bwRoleInfoService.Value.GetRoleSelectListItems();
            selectListItems.SetSelected(roleId.ToNonNullString());

            ViewBag.RoleSelectListItems = selectListItems;
        }

        private IActionResult GetEditView(EditUserInfo model)
        {
            return View("Edit", model);
        }
    }
}