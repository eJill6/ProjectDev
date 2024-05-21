using MS.Core.MM.Models.Entities.OperationOverview;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Services
{
    public class OperationOverviewService : MMBaseService,IOperationOverviewService
    {
        private readonly IOperationOverviewRepo _operationOverviewRepo;
        /// <summary>
        /// 营运数据
        /// </summary>
        public OperationOverviewService(ILogger logger, IOperationOverviewRepo operationOverviewRepo) : base(logger)
        {
            _operationOverviewRepo = operationOverviewRepo;
        }
        /// <summary>
        ///营运总览
        /// </summary>
        public async Task<BaseReturnDataModel<OperationOverview>> GetOperationOverview()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<OperationOverview>();
                result.DataModel = await _operationOverviewRepo.GetOperationOverview();
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }
        /// <summary>
        /// 营运数据-日营收
        /// </summary>
        public async Task<BaseReturnDataModel<PageResultModel<DailyRevenue>>> GetDailyRevenue(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<DailyRevenue>>();
                result.DataModel = await _operationOverviewRepo.GetDailyRevenue(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 营运数据-月营收
        /// </summary>
        public async Task<BaseReturnDataModel<PageResultModel<MonthlyRevenue>>> GetMonthlyRevenue(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MonthlyRevenue>>();
                result.DataModel = await _operationOverviewRepo.GetMonthlyRevenue(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 整点人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<IEnumerable<HourUsers>>> GetHourUsers(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<HourUsers>>();
                result.DataModel = await _operationOverviewRepo.GetHourUsers(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 日人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<DailyUsers>>> GetDailyUsers(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<DailyUsers>>();
                result.DataModel = await _operationOverviewRepo.GetDailyUsers(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 月人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<MonthlyUsers>>> GetMonthlyUsers(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MonthlyUsers>>();
                result.DataModel = await _operationOverviewRepo.GetMonthlyUsers(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        public async Task<BaseReturnDataModel<PostOverview>> GetPostOverview()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PostOverview>();
                result.DataModel = await _operationOverviewRepo.GetPostOverview();
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }
        /// <summary>
        /// 帖子日营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<PostDailyRevenue>>> GetPostDailyRevenue(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostDailyRevenue>>();
                result.DataModel = await _operationOverviewRepo.GetPostDailyRevenue(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 帖子月营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<PostMonthlyRevenue>>> GetPostMonthlyRevenue(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostMonthlyRevenue>>();
                result.DataModel = await _operationOverviewRepo.GetPostMonthlyRevenue(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        public async Task<BaseReturnDataModel<PageResultModel<PostDailyTrend>>> GetPostDailyTrend(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostDailyTrend>>();
                result.DataModel = await _operationOverviewRepo.GetPostDailyTrend(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        public async Task<BaseReturnDataModel<PageResultModel<PostMonthlyTrend>>> GetPostMonthlyTrend(OperationOverviewReportParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<PostMonthlyTrend>>();
                result.DataModel = await _operationOverviewRepo.GetPostMonthlyTrend(param);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
    }
}
