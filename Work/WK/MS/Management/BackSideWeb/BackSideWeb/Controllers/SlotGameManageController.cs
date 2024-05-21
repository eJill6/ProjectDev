using BackSideWeb.Controllers.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using TencentCloud.Dlc.V20210125.Models;

namespace BackSideWeb.Controllers
{
    public class SlotGameManageController : BaseCRUDController<SlotGameManageQueryParam, SlotGameManageCreateParam, SlotGameManageUpdateParam>
    {
        private readonly Lazy<ISlotGameManageService> _slotGameManageService;

        private readonly Lazy<ISlotGameManageReadService> _slotGameManageReadService;

        protected override string ClientServiceName => "imgConvertToAesCRUDService";

        protected override string ClientEditSingleRowServiceName => "slotGameEditSingleRowService";

        protected override string[] PageJavaScripts => new string[]
        {
            "base/crud/imgConvertToAesCRUDService.min.js"
        };

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.SlotGameManage;

        public SlotGameManageController()
        {
            _slotGameManageService = DependencyUtil.ResolveJxBackendService<ISlotGameManageService>(EnvLoginUser, DbConnectionTypes.Master);
            _slotGameManageReadService = DependencyUtil.ResolveJxBackendService<ISlotGameManageReadService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected override BaseReturnModel DoInsert(SlotGameManageCreateParam insertModel)
            => _slotGameManageService.Value.Create(insertModel);

        protected override BaseReturnModel DoUpdate(SlotGameManageUpdateParam updateModel)
            => _slotGameManageService.Value.Update(updateModel);

        protected override BaseReturnModel DoDelete(string keyContent)
            => _slotGameManageService.Value.Delete(keyContent.ToInt32());

        public override ActionResult Index()
        {
            SetProductSelectListItems(defaultText: SelectItemElement.All);
            SetIsActiveSelectListItems();

            return base.Index();
        }

        public override ActionResult GetGridViewResult(SlotGameManageQueryParam queryParam)
        {
            PagedResultModel<SlotGameManageModel> model = _slotGameManageReadService.Value.GetPagedModel(queryParam);

            return PartialView(model);
        }

        protected override IActionResult GetInsertView()
        {
            SetProductSelectListItems();
            SetIsActiveSelectListItems();

            return GetEditView(new BaseSlotGameManageParam());
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            BaseSlotGameManageParam model = _slotGameManageReadService.Value.GetManageParam(keyContent.ToInt32());
            SetProductSelectListItems(model.ThirdPartyCode);
            SetIsActiveSelectListItems(model.IsActive);

            return GetEditView(model);
        }

        private void SetProductSelectListItems(string? thirdPartyCode = null, string? defaultText = null)
        {
            List<JxBackendSelectListItem> productSelectListItems = _slotGameManageReadService
                .Value
                .GetProductSelectListItems(defaultText ?? SelectItemElement.PlzChoice);

            productSelectListItems.SetSelected(thirdPartyCode.ToNonNullString());

            ViewBag.ProductSelectListItems = productSelectListItems;
        }

        private void SetIsActiveSelectListItems(bool? isActive = null)
        {
            List<JxBackendSelectListItem> isActiveSelectListItems = _slotGameManageReadService.Value.GetActionSelectListItems();
            isActiveSelectListItems.SetSelected(isActive.ToNonNullString());

            ViewBag.IsActiveSelectListItems = isActiveSelectListItems;
        }

        private IActionResult GetEditView(BaseSlotGameManageParam model)
        {
            return View("Edit", model);
        }
    }
}