using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    public class AdminReportController : ApiControllerBase
    {

        /// <inheritdoc cref="IPostService"/>
        private readonly IPostService _service;

        public AdminReportController(IPostService service,
            ILogger logger) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// 设诉记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminReportList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminReportListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.ReportSearch(model);
                if (result.IsSuccess)
                {
                    result.SetCode(ReturnCode.Success);
                    return result;
                }
                result.SetModel(result);
                return result;
            }, model));
        }

        /// <summary>
        /// 投诉审核
        /// </summary>
        /// <param name="reportId">投诉Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{reportId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(string reportId, AdminReportData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var report = await _service.ReportDetail(reportId);
                if (report != null &&
                    report.IsSuccess &&
                    report.DataModel != null &&
                    report.DataModel.Status == ReviewStatus.UnderReview)
                {
                    param.Id = reportId;
                    param.ExamineMan = User?.Nickname();
                    return await _service.ReportEdit(param);
                }
                return new BaseReturnModel(ReturnCode.DataIsNotExist);
            }, model));
        }



        /// <summary>
        /// 投诉詳情
        /// </summary>
        /// <param name="reportId">投诉Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{reportId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminReportDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string reportId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.ReportDetail(param);
            }, reportId));
        }
    }
}
