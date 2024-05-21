using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.ViewModel.OperatingData;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using Microsoft.AspNetCore.Mvc;
using BackSideWeb.Helpers;
using Newtonsoft.Json;
using MS.Core.MMModel.Models.OperationOverview;
using System.Collections.Generic;
using BackSideWeb.Models.Enums;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 日人数
    /// </summary>
    public class DailyUsersController : BaseSearchGridController<OperatingDataSearchParameterViewModel>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/dailyUsers/dailyUsersSearchParam.min.js",
            "business/dailyUsers/dailyUsersSearchService.min.js"
        };

        protected override string ClientServiceName => "dailyUsersSearchService";

        public override ActionResult Index()
        {
            base.Index();


            return View();
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.DailyUsers;

        private const string controller = "OperationOverview";
        private const string requestDataAction = "GetDailyUsers";
        private const string requestDataActionHour = "GetHourUsers";
        public override ActionResult GetGridViewResult(OperatingDataSearchParameterViewModel requestParam)
        {
            var dailyRevenueVmModel = new PagedResultModel<DailyUsersViewModel>();
            requestParam.BeginTime = Convert.ToDateTime(requestParam.BeginTime).ToString("yyyy-MM-dd");
            requestParam.EndTime = Convert.ToDateTime(requestParam.EndTime).AddDays(1).ToString("yyyy-MM-dd");
            string parame = JsonConvert.SerializeObject(requestParam);
            dailyRevenueVmModel.PageSize = requestParam.PageSize;

            var result = MMClientApi.PostApi<DailyUsers>(controller, requestDataAction, parame);
            if (result != null && result.IsSuccess)
            {
                dailyRevenueVmModel.ResultList = MMHelpers.MapModelList<DailyUsers, DailyUsersViewModel>(result.DataModel.Data.ToList());
                dailyRevenueVmModel.TotalPageCount = result.DataModel.TotalPage;
                dailyRevenueVmModel.PageSize = result.DataModel.PageSize;
                dailyRevenueVmModel.PageNo = result.DataModel.PageNo;
                dailyRevenueVmModel.TotalCount = result.DataModel.TotalCount;
            }
            return PartialView(dailyRevenueVmModel);
        }

        public ActionResult Detail(string keyContent)
        {
            //SetPageTitle($" {keyContent} 整点人数");
            SetLayout(LayoutType.Base);
            var requestParam = new OperatingDataSearchParameterViewModel();
            requestParam.BeginTime = DateTime.Parse(keyContent).ToString("yyyy-MM-dd");
            requestParam.EndTime = DateTime.Parse(keyContent).AddDays(1).ToString("yyyy-MM-dd");
            string parame = JsonConvert.SerializeObject(requestParam);
            var result = MMClientApi.PostListData<HourUsers>(controller, requestDataActionHour, parame);
            var dataList = new List<HourUsersViewModel>();
            for (int i = 0; i < 24; i++)
            {
                var hourUser = new HourUsersViewModel();
                hourUser.Date = DateTime.Parse($"{keyContent} 00:00:00").AddHours(i);
               
                if (result.IsSuccess && result.DataModel!=null && result.DataModel.Any())
                {
                    var data = result.DataModel;
                    var dataModel = data.Find(c => int.Parse(c.Date.Split(" ")[1]) == hourUser.Date.Hour);
                    if (dataModel != null)
                    {
                        hourUser.VisitorsNumber = dataModel.VisitorsNumber.ToString();
                    }
                    else
                        hourUser.VisitorsNumber = "-";
                }
                else
                    hourUser.VisitorsNumber = "-";

                dataList.Add(hourUser);
            }
            return View(dataList);
        }
    }
}
