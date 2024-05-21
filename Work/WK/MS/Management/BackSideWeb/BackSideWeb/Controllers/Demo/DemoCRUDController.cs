using BackSideWeb.Controllers.Base;
using JxBackendService.Attributes.BackSideWeb;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Demo;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Controllers.Demo
{
    public class DemoCRUDController : BaseCRUDController<QueryFrontsideMenuParam, DemoEditParam, DemoEditParam>
    {
        private readonly Lazy<IUserInfoRelatedService> _userInfoRelatedService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        protected override string[] PageJavaScripts => new string[]
        {
            "business/demo/demoSearchParam.min.js",
            "business/demo/demoCRUDService.min.js"
        };

        protected override string ClientServiceName => "demoCRUDService";

        protected override string ClientEditSingleRowServiceName => "demoCrudEditSingleRowService"; //為了檔案上傳需要重寫蒐集formData

        public DemoCRUDController()
        {
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Slave);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        [Permission(PermissionKeys.DemoSearch, AuthorityTypes.Read)]
        public JsonResult GetScore(int userId)
        {
            decimal availableScores = _userInfoRelatedService.Value.GetUserAvailableScores(userId);

            return Json(new UserScore() { AvailableScores = availableScores });
        }

        public override ActionResult Index()
        {
            base.Index();

            List<JxBackendSelectListItem> frontsideMenuTypeSettingSelectListItems = FrontsideMenuTypeSetting
                .GetSelectListItems(hasBlankOption: true, defaultDisplayText: CommonElement.All, defaultValue: null);

            ViewBag.TypeSelectListItems = frontsideMenuTypeSettingSelectListItems.CastByJson<List<SelectListItem>>();

            return View(new QueryFrontsideMenuParam());
        }

        public override ActionResult GetGridViewResult(QueryFrontsideMenuParam searchParam)
        {
            ViewBag.HasEditPermission = HasPermission(PermissionKeys.DemoCRUD, AuthorityTypes.Edit);
            PagedResultModel<QueryFrontsideMenuModel> model = _frontsideMenuService.Value.GetPagedFrontsideMenu(searchParam);

            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.DemoCRUD;

        [Permission(PermissionKeys.DemoCRUD, AuthorityTypes.Edit)]
        public IActionResult EditStyle2()
        {
            InitPopupEditView(ClientServiceName);
            SetPageTitle("左右雙欄編輯");
            SetPageActType(ActTypes.Update);

            return View(new DemoEditParam());
        }

        protected override BaseReturnModel DoInsert(DemoEditParam insertModel)
        {
            if (insertModel.DemoFile != null && insertModel.DemoFile.Length > 0)
            {
                byte[] bytes = insertModel.DemoFile.ToBytes();
                //do save file;
            }

            //do insert data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(DemoEditParam updateModel)
        {
            //do update data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            //do delete data here
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override IActionResult GetInsertView()
        {
            return GetEditView(new DemoEditParam());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            //pass view model to view here
            return GetEditView(new DemoEditParam() { No = keyContent.ToInt32(), Sort = 100 });
        }

        private IActionResult GetEditView(DemoEditParam model)
        {
            //此範例為新增與修改共用同一個view
            return View("Edit", model);
        }
    }
}