using BackSideWeb.Controllers.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class HotGameManageController : BaseCRUDController<HotGameManageQueryParam, HotGameManageCreateParam, HotGameManageUpdateParam>
    {
        private readonly Lazy<IHotGameManageService> _hotGameManageService;

        private readonly Lazy<IHotGameManageReadService> _hotGameManageReadService;

        protected override string ClientServiceName => "imgConvertToAesCRUDService";

        protected override string ClientEditSingleRowServiceName => "hotGameEditSingleRowService";

        protected override string[] PageJavaScripts => new string[]
        {
            "base/crud/imgConvertToAesCRUDService.min.js"
        };

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.HotGameManage;

        public HotGameManageController()
        {
            _hotGameManageService = DependencyUtil.ResolveJxBackendService<IHotGameManageService>(EnvLoginUser, DbConnectionTypes.Master);
            _hotGameManageReadService = DependencyUtil.ResolveJxBackendService<IHotGameManageReadService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected override BaseReturnModel DoInsert(HotGameManageCreateParam insertModel)
            => _hotGameManageService.Value.Create(insertModel);

        protected override BaseReturnModel DoUpdate(HotGameManageUpdateParam updateModel)
            => _hotGameManageService.Value.Update(updateModel);

        protected override BaseReturnModel DoDelete(string keyContent)
            => _hotGameManageService.Value.Delete(keyContent.ToInt32());

        public override ActionResult GetGridViewResult(HotGameManageQueryParam queryParam)
        {
            PagedResultModel<HotGameManageModel> model = _hotGameManageReadService.Value.GetPagedModel(queryParam);

            return PartialView(model);
        }

        protected override IActionResult GetInsertView()
        {
            SetSelectListItems();

            return GetEditView(new BaseHotGameManageParam());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            FrontsideMenu data = _hotGameManageReadService.Value.GetSingle(keyContent.ToInt32());
            BaseHotGameManageParam model = data.CastByJson<BaseHotGameManageParam>();
            SetSelectListItems(model.ProductCode, model.GameCode, model.IsActive);

            return GetEditView(model);
        }

        private void SetSelectListItems(string? productCode = null, string? gameCode = null, bool? isActive = null)
        {
            List<JxBackendSelectListItem> productSelectListItems = _hotGameManageReadService.Value.GetProductSelectListItems();
            productSelectListItems.SetSelected(productCode.ToNonNullString());

            ViewBag.ProductSelectListItems = productSelectListItems;

            List<JxBackendSelectListItem> isActiveSelectListItems = _hotGameManageReadService.Value.GetActionSelectListItems();
            isActiveSelectListItems.SetSelected(isActive.ToNonNullString());

            ViewBag.IsActiveSelectListItems = isActiveSelectListItems;

            PlatformProduct product = PlatformProduct.GetSingle(productCode);
            List<JxBackendSelectListItem> gameCodeSelectListItems = _hotGameManageReadService.Value.GetHotGameSubGames(product);
            gameCodeSelectListItems.SetSelected(gameCode.ToNonNullString());

            ViewBag.GameCodeSelectListItems = gameCodeSelectListItems;
        }

        private IActionResult GetEditView(BaseHotGameManageParam model)
        {
            return View("Edit", model);
        }
    }
}