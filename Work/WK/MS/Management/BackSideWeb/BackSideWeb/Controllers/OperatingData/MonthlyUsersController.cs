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
    /// 月人数
    /// </summary>
    public class MonthlyUsersController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/monthlyUsers/monthlyUsersSearchParam.min.js",
            "business/monthlyUsers/monthlyUsersSearchService.min.js"
        };

        protected override string ClientServiceName => "monthlyUsersSearchService";

        public override ActionResult Index()
        {
            base.Index();


            return View();
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.MonthlyUsers;
        private const string controller = "OperationOverview";
        private const string requestDataAction = "GetMonthlyUsers";
        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            var dailyRevenueVmModel = new PagedResultModel<MonthlyUsersViewModel>();
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            string parame = JsonConvert.SerializeObject(requestParam);
            dailyRevenueVmModel.PageSize = requestParam.PageSize;

            var result = MMClientApi.PostApi<MonthlyUsers>(controller, requestDataAction, parame);
            if (result != null && result.IsSuccess)
            {
                dailyRevenueVmModel.ResultList = MMHelpers.MapModelList<MonthlyUsers, MonthlyUsersViewModel>(result.DataModel.Data.ToList());
                dailyRevenueVmModel.TotalPageCount = result.DataModel.TotalPage;
                dailyRevenueVmModel.PageSize = result.DataModel.PageSize;
                dailyRevenueVmModel.PageNo = result.DataModel.PageNo;
                dailyRevenueVmModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(dailyRevenueVmModel);
        }
    }
}
