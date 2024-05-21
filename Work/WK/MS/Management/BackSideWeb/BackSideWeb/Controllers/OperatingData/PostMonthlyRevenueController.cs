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
    /// 帖子月营收
    /// </summary>
    public class PostMonthlyRevenueController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/postMonthlyRevenue/postMonthlyRevenueSearchParam.min.js",
            "business/postMonthlyRevenue/postMonthlyRevenueSearchService.min.js"
        };

        protected override string ClientServiceName => "postMonthlyRevenueSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostMonthlyRevenue;

        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            var result = MMClientApi.PostApi<OperationOverviewReportParam, PostMonthlyRevenueViewModel>("OperationOverview", "GetPostMonthlyRevenue", requestParam);
            if (result != null)
            {
                var model = new PagedResultModel<PostMonthlyRevenueViewModel>()
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
