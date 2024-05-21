using BackSideWeb.Controllers.Base;
using BackSideWeb.Models.ViewModel.OperatingData;
using BackSideWeb.Models.ViewModel;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using Microsoft.AspNetCore.Mvc;
using BackSideWeb.Helpers;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using JxBackendService.Model.Util.Export;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 帖子日趋势
    /// </summary>
    public class PostDailyTrendController : BaseSearchGridController<OperationOverviewReportParam>
    {

        protected override string[] PageJavaScripts => new string[]
        {
            "business/postDailyTrend/postDailyTrendSearchParam.min.js",
            "business/postDailyTrend/postDailyTrendSearchService.min.js"
        };

        protected override string ClientServiceName => "postDailyTrendSearchService";

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostDailyTrend;

        public override ActionResult GetGridViewResult(OperationOverviewReportParam requestParam)
        {
            requestParam.EndTime = requestParam.EndTime.AddDays(1);
            var result = MMClientApi.PostApi<OperationOverviewReportParam, PostDailyTrendViewModel>("OperationOverview", "GetPostDailyTrend", requestParam);
            if (result != null)
            {
                var model = new PagedResultModel<PostDailyTrendViewModel>()
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
