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
    /// <summary>
    /// 后台官方投诉
    /// </summary>
    public class AdminOfficialReportController : ApiControllerBase
    {
        /// <inheritdoc cref="IPostService"/>
        private readonly IPostService _service;

        public AdminOfficialReportController(IPostService service,
            ILogger logger) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// 官方贴投诉审核
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
                    return await _service.OfficialReportEdit(param);
                }
                return new BaseReturnModel(ReturnCode.DataIsNotExist);
            }, model));
        }
    }
}