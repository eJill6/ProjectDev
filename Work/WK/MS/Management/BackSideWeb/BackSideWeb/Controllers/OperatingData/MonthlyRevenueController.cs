using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.ViewModel.OperatingData;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using Microsoft.AspNetCore.Mvc;
using BackSideWeb.Helpers;
using Newtonsoft.Json;
using MS.Core.MMModel.Models.OperationOverview;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 月营收
    /// </summary>
    public class MonthlyRevenueController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/monthlyRevenue/monthlyRevenueSearchParam.min.js",
            "business/monthlyRevenue/monthlyRevenueSearchService.min.js"
        };

        protected override string ClientServiceName => "monthlyRevenueSearchService";

        public override ActionResult Index()
        {
            base.Index();


            return View();
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.MonthlyRevenue;

        private const string controller = "OperationOverview";
        private const string requestDataAction = "GetMonthlyRevenue";
        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            var dailyRevenueVmModel = new PagedResultModel<MonthlyRevenueViewModel>();
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            string parame = JsonConvert.SerializeObject(requestParam);
            dailyRevenueVmModel.PageSize = requestParam.PageSize;

            var result = MMClientApi.PostApi<MonthlyRevenue>(controller, requestDataAction, parame);
            if (result != null && result.IsSuccess)
            {
                dailyRevenueVmModel.ResultList = MMHelpers.MapModelList<MonthlyRevenue, MonthlyRevenueViewModel>(result.DataModel.Data.ToList());
                dailyRevenueVmModel.TotalPageCount = result.DataModel.TotalPage;
                dailyRevenueVmModel.PageSize = result.DataModel.PageSize;
                dailyRevenueVmModel.PageNo = result.DataModel.PageNo;
                dailyRevenueVmModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(dailyRevenueVmModel);
        }
    }
}
