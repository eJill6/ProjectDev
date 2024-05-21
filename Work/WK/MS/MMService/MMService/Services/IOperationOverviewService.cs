using MS.Core.MM.Models.Entities.OperationOverview;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Services
{
    /// <summary>
    /// 营运数据
    /// </summary>
    public interface IOperationOverviewService
    {
        /// <summary>
        ///营运总览
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<OperationOverview>> GetOperationOverview();

        /// <summary>
        /// 营运数据-日营收
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<PageResultModel<DailyRevenue>>> GetDailyRevenue(OperationOverviewReportParam param);

        /// <summary>
        /// 营运数据-月营收
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<PageResultModel<MonthlyRevenue>>> GetMonthlyRevenue(OperationOverviewReportParam param);
        /// <summary>
        /// 营运数据-整点人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<IEnumerable<HourUsers>>> GetHourUsers(OperationOverviewReportParam param);
        /// <summary>
        /// 营运数据-日人數
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<DailyUsers>>> GetDailyUsers(OperationOverviewReportParam param);

        /// <summary>
        /// 营运数据-月人數
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<MonthlyUsers>>> GetMonthlyUsers(OperationOverviewReportParam param);

        /// <summary>
        /// 营运数据-帖子总览
        /// </summary>
        Task<BaseReturnDataModel<PostOverview>> GetPostOverview();

        /// <summary>
        /// 帖子_日营收
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<PostDailyRevenue>>> GetPostDailyRevenue(OperationOverviewReportParam param);

        /// <summary>
        /// 帖子_月营收
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<PostMonthlyRevenue>>> GetPostMonthlyRevenue(OperationOverviewReportParam param);

        /// <summary>
        /// 帖子_日趋势
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<PostDailyTrend>>> GetPostDailyTrend(OperationOverviewReportParam param);

        /// <summary>
        /// 帖子_月趋势
        /// </summary>
        Task<BaseReturnDataModel<PageResultModel<PostMonthlyTrend>>> GetPostMonthlyTrend(OperationOverviewReportParam param);
    }
}
