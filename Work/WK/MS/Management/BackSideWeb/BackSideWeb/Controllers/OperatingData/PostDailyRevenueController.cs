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
    /// 帖子日营收
    /// </summary>
    public class PostDailyRevenueController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/postDailyRevenue/postDailyRevenueSearchParam.min.js",
            "business/postDailyRevenue/postDailyRevenueSearchService.min.js"
        };

        protected override string ClientServiceName => "postDailyRevenueSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostDailyRevenue;

        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            var result = MMClientApi.PostApi<OperationOverviewReportParam, PostDailyRevenueViewModel>("OperationOverview", "GetPostDailyRevenue", requestParam);
            if (result != null)
            {
                var model = new PagedResultModel<PostDailyRevenueViewModel>()
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
