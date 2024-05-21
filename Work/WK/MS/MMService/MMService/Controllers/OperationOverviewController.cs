using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.MM.Models.Entities.OperationOverview;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    public class OperationOverviewController : ApiControllerBase
    {
        /// <summary>
        /// 营运数据
        /// </summary>
        private readonly IOperationOverviewService _service;
        private readonly IMapper _mapper = null;
        public OperationOverviewController(IOperationOverviewService service, IMapper mapper, ILogger logger) : base(logger)
        {
            _service = service;
            _mapper = mapper;
        }
        /// <summary>
        ///营运总览
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOperationOverview()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<OperationOverview>();
                var query = await _service.GetOperationOverview();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<OperationOverview>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, String.Empty));
        }

        /// <summary>
        /// 营运数据-日营收
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<DailyRevenue>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyRevenue(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<DailyRevenue>>();
                var query = await _service.GetDailyRevenue(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }

        /// <summary>
        /// 营运数据-月营收
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MonthlyRevenue>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyRevenue(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MonthlyRevenue>>();
                var query = await _service.GetMonthlyRevenue(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }

        /// <summary>
        /// 营运数据-整点人数
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HourUsers>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHourUsers(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<HourUsers>>();
                var query = await _service.GetHourUsers(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }

        /// <summary>
        /// 营运数据-日人數
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<DailyUsers>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyUsers(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<DailyUsers>>();
                var query = await _service.GetDailyUsers(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
        /// <summary>
        /// 营运数据-月人數
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MonthlyUsers>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyUsers(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MonthlyUsers>>();
                var query = await _service.GetMonthlyUsers(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
        /// <summary>
        ///帖子总览
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPostOverview()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PostOverview>();
                var query = await _service.GetPostOverview();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<PostOverview>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, String.Empty));
        }
        /// <summary>
        /// 帖子_日营收
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<PostDailyRevenue>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostDailyRevenue(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostDailyRevenue>>();
                var query = await _service.GetPostDailyRevenue(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
        /// <summary>
        /// 帖子_月营收
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<PostMonthlyRevenue>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostMonthlyRevenue(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostMonthlyRevenue>>();
                var query = await _service.GetPostMonthlyRevenue(param);
                if (query.IsSuccess)
                {
                    result.DataModel = query.DataModel;
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
        /// <summary>
        /// 帖子_日趋势
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<PostDailyTrend>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostDailyTrend(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostDailyTrend>>();
                var query = await _service.GetPostDailyTrend(param);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<PageResultModel<PostDailyTrend>>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
        /// <summary>
        /// 帖子_月趋势
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<PostMonthlyTrend>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostMonthlyTrend(OperationOverviewReportParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostMonthlyTrend>>();
                var query = await _service.GetPostMonthlyTrend(param);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<PageResultModel<PostMonthlyTrend>>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, param));
        }
    }
}
