using AutoMapper;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Util.Export;
using JxBackendService.Model.ViewModel.OperatingData;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.OperationOverview;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 营运总览
    /// </summary>
    public class OperationOverviewController : BaseSearchGridController<object>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/operatingData/operationOverviewService.min.js",
        };

        protected override string ClientServiceName => "operationOverviewService";

        public override ActionResult GetGridViewResult(object requestParam)
        {
            var result = MMClientApi.GetSingleApiNoParam<OperationOverview>("OperationOverview", "GetOperationOverview");
            var viewModel = MMHelpers.MapModel<OperationOverview, OperationOverviewViewModel>(result.Datas);
            return PartialView(viewModel);
        }

        protected override ExportQueryResultParam ConvertQueryResultToExportParam(ActionResult queryResult)
        {
            var viewResult = queryResult as PartialViewResult;
            var model = viewResult.Model as OperationOverviewViewModel;

            ExportQueryResultParam exportParam = new ExportQueryResultParam
            {
                QueryResult = new List<object> { model },
                QueryResultModelType = typeof(OperationOverviewViewModel),
            };

            return exportParam;
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.OperationOverview;
    }
}
