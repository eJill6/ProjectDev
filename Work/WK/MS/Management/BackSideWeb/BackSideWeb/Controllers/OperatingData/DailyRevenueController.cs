using AutoMapper;
using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using BackSideWeb.Models.ViewModel.OperatingData;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.OperationOverview;
using Newtonsoft.Json;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 日营收
    /// </summary>
    public class DailyRevenueController : BaseSearchGridController<OperatingDataSearchParameterViewModel>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/dailyRevenue/dailyRevenueSearchParam.min.js",
            "business/dailyRevenue/dailyRevenueService.min.js"
        };

        protected override string ClientServiceName => "dailyRevenueService";

        public override ActionResult Index()
        {
            base.Index();


            return View(new OperatingDataSearchParameterViewModel());
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.DailyRevenue;

        private const string controller = "OperationOverview";
        private const string requestDataAction = "GetDailyRevenue";
        public override ActionResult GetGridViewResult(OperatingDataSearchParameterViewModel requestParam)
        {
            var dailyRevenueVmModel = new PagedResultModel<DailyRevenueViewModel>();
            requestParam.BeginTime = Convert.ToDateTime(requestParam.BeginTime).ToString("yyyy-MM-dd");
            requestParam.EndTime = Convert.ToDateTime(requestParam.EndTime).AddDays(1).ToString("yyyy-MM-dd");
            string parame = JsonConvert.SerializeObject(requestParam);
            dailyRevenueVmModel.PageSize = requestParam.PageSize;
            var result = MMClientApi.PostApi<DailyRevenue>(controller, requestDataAction, parame);
            if (result != null && result.IsSuccess)
            {
                dailyRevenueVmModel.ResultList = MMHelpers.MapModelList<DailyRevenue, DailyRevenueViewModel>(result.DataModel.Data.ToList());
                dailyRevenueVmModel.TotalPageCount = result.DataModel.TotalPage;
                dailyRevenueVmModel.PageSize = result.DataModel.PageSize;
                dailyRevenueVmModel.PageNo = result.DataModel.PageNo;
                dailyRevenueVmModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(dailyRevenueVmModel);
        }

        //public override 
    }
}
