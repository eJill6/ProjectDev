using MS.Core.MM.Models.Entities.OperationOverview;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Repos.interfaces
{
    /// <summary>
    /// 营运数据
    /// </summary>
    public interface IOperationOverviewRepo
    {
        /// <summary>
        ///营运总览
        /// </summary>
        Task<OperationOverview> GetOperationOverview();

        /// <summary>
        /// 营运数据-日营收
        /// </summary>
        Task<PageResultModel<DailyRevenue>> GetDailyRevenue(OperationOverviewReportParam param);

        /// <summary>
        /// 营运数据-月营收
        /// </summary>
        Task<PageResultModel<MonthlyRevenue>> GetMonthlyRevenue(OperationOverviewReportParam param);

        /// <summary>
        /// 营运数据-整点人數
        /// </summary>
        Task<IEnumerable<HourUsers>> GetHourUsers(OperationOverviewReportParam param);
        /// <summary>
        /// 营运数据-日人數
        /// </summary>
        Task<PageResultModel<DailyUsers>> GetDailyUsers(OperationOverviewReportParam param);
        
        /// <summary>
        /// 营运数据-月人數
        /// </summary>
        Task<PageResultModel<MonthlyUsers>> GetMonthlyUsers(OperationOverviewReportParam param);
        
        /// <summary>
        /// 营运数据-帖子总览
        /// </summary>
        Task<PostOverview> GetPostOverview();

        /// <summary>
        /// 帖子_日营收
        /// </summary>
        Task<PageResultModel<PostDailyRevenue>> GetPostDailyRevenue(OperationOverviewReportParam param);
        
        /// <summary>
        /// 帖子_月营收
        /// </summary>
        Task<PageResultModel<PostMonthlyRevenue>> GetPostMonthlyRevenue(OperationOverviewReportParam param);
        
        /// <summary>
        /// 帖子_日趋势
        /// </summary>
        Task<PageResultModel<PostDailyTrend>> GetPostDailyTrend(OperationOverviewReportParam param);
        
        /// <summary>
        /// 帖子_月趋势
        /// </summary>
        Task<PageResultModel<PostMonthlyTrend>> GetPostMonthlyTrend(OperationOverviewReportParam param);
    }
}
