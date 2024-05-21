using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Util.Export;
using JxBackendService.Model.ViewModel.OperatingData;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MMModel.Models.OperationOverview;

namespace BackSideWeb.Controllers.OperatingData
{
    /// <summary>
    /// 帖子总览
    /// </summary>
    public class PostOverviewController : BaseSearchGridController<object>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/operatingData/postOverviewService.min.js",
        };
        protected override string ClientServiceName => "postOverviewService";

        public override ActionResult GetGridViewResult(object requestParam)
        {
            var result = MMClientApi.GetSingleApiNoParam<PostOverview>("OperationOverview", "GetPostOverview");
            var viewModel = MMHelpers.MapModel<PostOverview, PostOverviewViewMdel>(result.Datas);
            return PartialView(viewModel);
        }
        protected override ExportQueryResultParam ConvertQueryResultToExportParam(ActionResult queryResult)
        {
            var viewResult = queryResult as PartialViewResult;
            var model = viewResult.Model as PostOverviewViewMdel;

            ExportQueryResultParam exportParam = new ExportQueryResultParam
            {
                QueryResult = new List<object> { model },
                QueryResultModelType = typeof(PostOverviewViewMdel),
            };

            return exportParam;
        }
        protected override PermissionKeys GetPermissionKey() => PermissionKeys.PostOverview;
    }
}
