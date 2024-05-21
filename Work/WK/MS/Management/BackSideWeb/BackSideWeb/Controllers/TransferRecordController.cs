using BackSideWeb.Controllers.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.TransferRecord;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel.TransferRecord;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class TransferRecordController : BaseSearchGridController<SearchTransferRecordParam>
    {
        private readonly Lazy<ITransferRecordService> _allTPTransferRecordService;

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.TransferRecord;

        protected override bool IsRefreshFrequencySetting => true;

        protected override bool IsAutoSearchAfterPageLoaded => false;

        protected override string[] PageJavaScripts => new string[]
        {
            "business/transferRecord/transferRecordService.min.js",
        };

        protected override string ClientServiceName => "transferRecordService";

        public TransferRecordController()
        {
            _allTPTransferRecordService = DependencyUtil.ResolveJxBackendService<ITransferRecordService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected override List<string> SetExportAdditionalData(SearchTransferRecordParam queryParam)
        {
            return new List<string>
            {
                $"{DisplayElement.OrderTime} : {queryParam.StartDate.ToFormatDateString()} 至 {queryParam.EndDate.ToFormatDateString()}"
            };
        }

        public override ActionResult GetGridViewResult(SearchTransferRecordParam param)
        {
            PagedResultModel<TransferRecordViewModel> model = _allTPTransferRecordService.Value.GetTransferRecord(param);

            return PartialView(model);
        }

        public override ActionResult Index()
        {
            base.Index();

            ViewBag.ProductSelectListItems = _allTPTransferRecordService.Value.GetProductSelectListItems();

            ViewBag.OrderStatusSelectListItems = TransferRecordOrderStatus
                .GetSelectListItems(hasBlankOption: true, defaultValue: null, defaultDisplayText: CommonElement.All);

            ViewBag.TransferTypeSelectListItems = SearchTransferType.GetSelectListItems(
                SearchTransferType.GetAll(),
                hasBlankOption: true,
                defaultValue: null,
                defaultDisplayText: CommonElement.All,
                (type) => SearchTransferType.GetSingle(type).GameTransferTypeName);

            return View(new SearchTransferRecordParam());
        }
    }
}