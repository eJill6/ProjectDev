using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.ViewModel.OperatingData;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using Microsoft.AspNetCore.Mvc;
using BackSideWeb.Helpers;
using MS.Core.MMModel.Models.OperationOverview;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 帖子月趋势
    /// </summary>
    public class PostMonthlyTrendController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/postMonthlyTrend/postMonthlyTrendSearchParam.min.js",
            "business/postMonthlyTrend/postMonthlyTrendSearchService.min.js"
        };

        protected override string ClientServiceName => "postMonthlyTrendSearchService";

        public override ActionResult Index()
        {
            base.Index();


            return View();
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostMonthlyTrend;

        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            var result = MMClientApi.PostApi<OperationOverviewReportParam, PostMonthlyTrendViewModel>("OperationOverview", "GetPostMonthlyTrend", requestParam);
            if (result != null)
            {
                var model = new PagedResultModel<PostMonthlyTrendViewModel>()
                {
                    PageNo = result.PageNo,
                    TotalCount = result.TotalCount,
                    PageSize = result.PageSize,
                    TotalPageCount = result.TotalPage,
                    ResultList = result.Data.ToList(),
                };
                return PartialView(model);
            }
            else
                return PartialView(null);
        }
    }
}
