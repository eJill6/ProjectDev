using BackSideWeb.Controllers.Base;
using JxBackendService.Attributes.BackSideWeb;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers.Demo
{
    public class DemoController : BaseSearchGridController<QueryFrontsideMenuParam>
    {
        private readonly Lazy<IUserInfoRelatedService> _userInfoRelatedService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        protected override bool IsRefreshFrequencySetting => true;

        protected override bool IsAutoSearchAfterPageLoaded => false;

        public DemoController()
        {
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Slave);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        [Permission(PermissionKeys.DemoSearch, AuthorityTypes.Edit)]
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

            ViewBag.TypeSelectListItems = frontsideMenuTypeSettingSelectListItems;

            return View(new QueryFrontsideMenuParam());
        }

        public override ActionResult GetGridViewResult(QueryFrontsideMenuParam searchParam)
        {
            PagedResultModel<QueryFrontsideMenuModel> model = _frontsideMenuService.Value.GetPagedFrontsideMenu(searchParam);

            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.DemoSearch;

        public ActionResult Detail()
        {
            InitPopupReadView();
            SetPageTitle("詳情");

            return View();
        }
    }
}