using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.OperationOverview;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.Models.Models;
using MS.Core.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Repos
{
    public class OperationOverviewRepo : BaseInlodbRepository,IOperationOverviewRepo
    {
        public OperationOverviewRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
           
        }
        public async Task<OperationOverview> GetOperationOverview()
        {
            string querySql = @"
--累積直另外撈
DECLARE @TotalUsers INT = ISNULL(
    (SELECT TOP 1 Amount
     FROM MMBusinessDataStatisticsReport WITH (NOLOCK)
     WHERE DateType = 2 AND AggregateType = 8
     ORDER BY StatisticsTime DESC),
    0
);

DECLARE @Merchant INT = ISNULL(
    (SELECT TOP 1 Amount
     FROM MMBusinessDataStatisticsReport WITH (NOLOCK)
     WHERE DateType = 2 AND AggregateType = 7
     ORDER BY StatisticsTime DESC),
    0
);

SELECT 
    @Merchant AS MerchantCount,
    @TotalUsers AS TotalUsers,
    PaidUsersCount,
    DepositsReceivedAmount,
    DepositsReceivedCount,
    SquareUnlockEarnings,
    AgencyUnlockEarnings,
    OfficialPlatformEarnings,
    SquareRefundAmount,
    AgencyRefundAmount,
    OfficialRefundAmount,
    SquareRefundCount,
    AgencyRefundCount,
    OfficialRefundCount,
    CASE WHEN @TotalUsers > 0 THEN PaidUsersCount * 100.0 / @TotalUsers ELSE NULL END AS PaymentRate,
    SquareUnlockEarnings + AgencyUnlockEarnings + OfficialPlatformEarnings AS TotalRevenue,
    CASE WHEN @TotalUsers > 0 THEN (SquareUnlockEarnings + AgencyUnlockEarnings + OfficialPlatformEarnings) * 1.0 / @TotalUsers ELSE NULL END AS ARPU,
    CASE WHEN PaidUsersCount > 0 THEN (SquareUnlockEarnings + AgencyUnlockEarnings + OfficialPlatformEarnings) * 1.0 / PaidUsersCount ELSE NULL END AS ARPPU
FROM (
    SELECT 
    SUM(CASE WHEN AggregateType = 9 THEN Amount ELSE 0 END) AS PaidUsersCount,
    SUM(CASE WHEN AggregateType = 10 THEN Amount ELSE 0 END) AS DepositsReceivedAmount,
    SUM(CASE WHEN AggregateType = 11 THEN Amount ELSE 0 END) AS DepositsReceivedCount,
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS SquareUnlockEarnings,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS AgencyUnlockEarnings,
    SUM(CASE WHEN AggregateType = 14 THEN Amount ELSE 0 END) AS OfficialPlatformEarnings,
    SUM(CASE WHEN AggregateType = 15 THEN Amount ELSE 0 END) AS SquareRefundAmount,
    SUM(CASE WHEN AggregateType = 16 THEN Amount ELSE 0 END) AS AgencyRefundAmount,
    SUM(CASE WHEN AggregateType = 17 THEN Amount ELSE 0 END) AS OfficialRefundAmount,
    SUM(CASE WHEN AggregateType = 18 THEN Amount ELSE 0 END) AS SquareRefundCount,
    SUM(CASE WHEN AggregateType = 19 THEN Amount ELSE 0 END) AS AgencyRefundCount,
    SUM(CASE WHEN AggregateType = 20 THEN Amount ELSE 0 END) AS OfficialRefundCount
    FROM MMBusinessDataStatisticsReport WITH (NOLOCK)
    WHERE DateType = 2 AND AggregateType IN (9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20)
) AS SubQueryAlias;
";
            var component = ReadDb.QueryTable<OperationOverview>();
            return await ReadDb.QueryFirstAsync<OperationOverview>(querySql);
        }
        /// <summary>
        /// 日营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<DailyRevenue>> GetDailyRevenue(OperationOverviewReportParam param)
        {

            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);

            const string querySql = @"
                                     DROP TABLE IF EXISTS #TEM_DailyRevenue
                                     SELECT 
		                                    CONVERT(VARCHAR(10),StatisticsTime,120) AS [date]
	                                       ,SUM(CASE WHEN AggregateType=2  THEN [Amount] ELSE 0 END) AS DAU
	                                       ,SUM(CASE WHEN AggregateType=9  THEN [Amount] ELSE 0 END) AS PU
	                                       ,SUM(CASE WHEN AggregateType=14 THEN [Amount] ELSE 0 END)+SUM(CASE WHEN AggregateType=12 THEN [Amount] ELSE 0 END)+SUM(CASE WHEN AggregateType=13 THEN [Amount] ELSE 0 END) AS Revenue 
	                                       ,SUM(CASE WHEN AggregateType=10 THEN [Amount] ELSE 0 END) AS DepositAmount
	                                       ,SUM(CASE WHEN AggregateType=11 THEN [Amount] ELSE 0 END) AS DepositCount
                                     INTO #TEM_DailyRevenue
                                     FROM MMBusinessDataStatisticsReport WITH(NOLOCK) 
                                     WHERE [DateType]=2 AND AggregateType IN(2,9,10,11,12,13,14)  AND  
                                     StatisticsTime>=@BeginTime AND StatisticsTime<@EndTime 
                                     GROUP BY CONVERT(VARCHAR(10),StatisticsTime,120)
                                     SET @TotalCount=(SELECT COUNT(*) FROM #TEM_DailyRevenue)
                                     SELECT                                            
	                                     [date]
	                                    ,[DAU]
	                                    ,[PU]
	                                    ,[Revenue]
	                                    ,(CASE WHEN [DAU]>0  THEN ([PU]/[DAU])*100 ELSE 0 END ) AS PayingRate
	                                    ,(CASE WHEN [DAU]>0  THEN [Revenue]/[DAU]  ELSE 0 END ) AS ARPU
	                                    ,(CASE WHEN [PU] >0  THEN [Revenue]/[PU]   ELSE 0 END ) AS ARPPU
	                                    ,[DepositAmount]
	                                    ,[DepositCount]
                                    FROM (
                                    SELECT *,ROW_NUMBER() OVER(ORDER BY [date] ) AS RowNumber  FROM #TEM_DailyRevenue
                                    ) AS T WHERE T.RowNumber BETWEEN @StartNo AND @EndNo ";
            
            var result = await ReadDb.QueryAsync<DailyRevenue>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return  new PageResultModel<DailyRevenue>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };

        }
        /// <summary>
        /// 月营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MonthlyRevenue>> GetMonthlyRevenue(OperationOverviewReportParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);

            const string querySql = @"
                                     DROP TABLE IF EXISTS #TEM_MonthlyRevenue
                                     SELECT
		                                     CONVERT(VARCHAR(7),StatisticsTime,120) AS [Month]
	                                        ,SUM(CASE WHEN [AggregateType]=2  THEN [Amount] ELSE 0 END) AS MAU
	                                        ,SUM(CASE WHEN [AggregateType]=9  THEN [Amount] ELSE 0 END) AS PU
	                                        ,SUM(CASE WHEN [AggregateType]=14 THEN [Amount] ELSE 0 END)+SUM(CASE WHEN AggregateType=12 THEN [Amount] ELSE 0 END)+SUM(CASE WHEN AggregateType=13 THEN [Amount] ELSE 0 END) AS TotalRevenue 
	                                        ,SUM(CASE WHEN [AggregateType]=10 THEN [Amount] ELSE 0 END) AS DepositAmount
	                                        ,SUM(CASE WHEN [AggregateType]=11 THEN [Amount] ELSE 0 END) AS DepositCount
                                     INTO #TEM_MonthlyRevenue
                                     FROM dbo.[MMBusinessDataStatisticsReport] WITH(NOLOCK)
                                     WHERE [DateType]=3 AND [AggregateType] IN (2,9,10,11,12,13,14) AND [StatisticsTime]>=@BeginTime AND [StatisticsTime]<@EndTime 
                                     GROUP BY CONVERT(VARCHAR(7),StatisticsTime,120)

                                     SET @TotalCount=(SELECT COUNT(*) FROM #TEM_MonthlyRevenue)

                                     SELECT                                            
	                                      [Month]
	                                     ,[MAU]
	                                     ,[PU]
	                                     ,[TotalRevenue]
	                                     ,(CASE WHEN [MAU]>0  THEN ([PU]/[MAU])*100 ELSE 0 END ) AS PayingRate
	                                     ,(CASE WHEN [MAU]>0  THEN [TotalRevenue]/[MAU]  ELSE 0 END ) AS ARPU
	                                     ,(CASE WHEN [PU] >0  THEN [TotalRevenue]/[PU]   ELSE 0 END ) AS ARPPU
	                                     ,[DepositAmount]
	                                     ,[DepositCount]
                                     FROM (
                                     SELECT *,ROW_NUMBER() OVER(ORDER BY [Month] ) AS RowNumber  FROM #TEM_MonthlyRevenue
                                     ) AS T WHERE T.RowNumber BETWEEN @StartNo AND @EndNo ";

            var result = await ReadDb.QueryAsync<MonthlyRevenue>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MonthlyRevenue>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        /// <summary>
        /// 获取整点人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HourUsers>> GetHourUsers(OperationOverviewReportParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);

            const string querySql = @"SELECT
		                                    CONVERT(VARCHAR(13),StatisticsTime,120) AS [Date]
			                               ,SUM(Amount) AS [VisitorsNumber]
		                              FROM MMBusinessDataStatisticsReport WITH(NOLOCK)
		                              WHERE StatisticsTime>=@BeginTime AND StatisticsTime<@EndTime AND AggregateType=4 AND DateType=1
		                              GROUP BY CONVERT(VARCHAR(13),StatisticsTime,120)";
            var result = await ReadDb.QueryAsync<HourUsers>(querySql, parameters);
            return result;
        }
        /// <summary>
        /// 日人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<DailyUsers>> GetDailyUsers(OperationOverviewReportParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);

            const string querySql = @"
                                     DROP TABLE IF EXISTS #TEM_DailyUsers
                                     SELECT 
	                                       CONVERT(VARCHAR(10),StatisticsTime,120) AS [date]
	                                      ,SUM(CASE WHEN AggregateType=3  THEN [Amount] ELSE 0 END) AS PV
	                                      ,SUM(CASE WHEN AggregateType=2  THEN [Amount] ELSE 0 END) AS DAU
	                                      ,SUM(CASE WHEN AggregateType=5  THEN [Amount] ELSE 0 END) AS PCU
	                                      ,SUM(CASE WHEN AggregateType=6  THEN [Amount] ELSE 0 END) AS ACU
                                          ,SUM(CASE WHEN AggregateType=1  THEN [Amount] ELSE 0 END) AS DUN
                                     INTO #TEM_DailyUsers
                                     FROM dbo.[MMBusinessDataStatisticsReport] WITH(NOLOCK)
                                     WHERE [DateType]=2 AND AggregateType IN(1,2,3,5,6)  AND  
                                     StatisticsTime>=@BeginTime AND StatisticsTime<@EndTime
                                     GROUP BY CONVERT(VARCHAR(10),StatisticsTime,120)
                                     SET @TotalCount=(SELECT COUNT(*) FROM #TEM_DailyUsers)
                                     SELECT                                            
	                                     [Date]
	                                    ,[PV]
	                                    ,[DAU]
	                                    ,[PCU]
	                                    ,ROUND([ACU],0) AS [ACU]
	                                    ,[DUN]
                                    FROM (
                                    SELECT *,ROW_NUMBER() OVER(ORDER BY [Date]) AS RowNumber  FROM #TEM_DailyUsers
                                    ) AS T WHERE T.RowNumber BETWEEN @StartNo AND @EndNo ";

            var result = await ReadDb.QueryAsync<DailyUsers>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<DailyUsers>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        /// <summary>
        /// 月人数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MonthlyUsers>> GetMonthlyUsers(OperationOverviewReportParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);

            const string querySql = @"
                                     DROP TABLE IF EXISTS #TEM_MonthlyUsers
                                     SELECT 
	                                       CONVERT(VARCHAR(7),StatisticsTime,120) AS [Month]
	                                      ,SUM(CASE WHEN AggregateType=3  THEN [Amount] ELSE 0 END) AS PV
	                                      ,SUM(CASE WHEN AggregateType=2  THEN [Amount] ELSE 0 END) AS MAU
                                     INTO #TEM_MonthlyUsers
                                     FROM dbo.[MMBusinessDataStatisticsReport] WITH(NOLOCK)
                                     WHERE [DateType]=3 AND [AggregateType] IN (2,3) AND [StatisticsTime]>=@BeginTime AND [StatisticsTime]<@EndTime 
                                     GROUP BY CONVERT(VARCHAR(7),StatisticsTime,120)
                                     SET @TotalCount=(SELECT COUNT(*) FROM #TEM_MonthlyUsers)
                                     SELECT                                            
	                                      [Month]
	                                     ,[PV]
	                                     ,[MAU]
                                     FROM (
                                     SELECT *,ROW_NUMBER() OVER(ORDER BY [Month] ) AS RowNumber  FROM #TEM_MonthlyUsers
                                     ) AS T WHERE T.RowNumber BETWEEN @StartNo AND @EndNo ";

            var result = await ReadDb.QueryAsync<MonthlyUsers>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MonthlyUsers>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        public async Task<PostOverview> GetPostOverview()
        {
            string querySql = @"
DECLARE @MaxDate DATETIME = (
SELECT MAX(StatisticsTime)
FROM MMPostStatisticsReport WITH (NOLOCK)
WHERE DateType = 2
);
SELECT 
 (SquareDisplayed+AgencyDisplayed+OfficialDisplayed) AS TotalPost,*
FROM(
    SELECT 
    SUM(CASE WHEN AggregateType = 1 THEN Amount ELSE 0  END) AS SquareDisplayed,
    SUM(CASE WHEN AggregateType = 2 THEN Amount ELSE 0  END) AS SquareUnderReview,
    SUM(CASE WHEN AggregateType = 3 THEN Amount ELSE 0  END) AS SquareNotApproved,
    SUM(CASE WHEN AggregateType = 4 THEN Amount ELSE 0  END) AS SquareUnlockedCount,
    SUM(CASE WHEN AggregateType = 5 THEN Amount ELSE 0  END) AS AgencyDisplayed,
    SUM(CASE WHEN AggregateType = 6 THEN Amount ELSE 0  END) AS AgencyUnderReview,
    SUM(CASE WHEN AggregateType = 7 THEN Amount ELSE 0  END) AS AgencyNotApproved,
    SUM(CASE WHEN AggregateType = 8 THEN Amount ELSE 0  END) AS AgencyUnlockedCount,
    SUM(CASE WHEN AggregateType = 9 THEN Amount ELSE 0  END) AS OfficialDisplayed,
    SUM(CASE WHEN AggregateType = 10 THEN Amount ELSE 0 END) AS OfficialUnderReview,
    SUM(CASE WHEN AggregateType = 11 THEN Amount ELSE 0 END) AS OfficialNotApproved,
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS OfficialReservedCount,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS OfficialReserveCancelCount
    FROM MMPostStatisticsReport WITH (NOLOCK)
    WHERE DateType = 2 AND AggregateType IN (1,2,3,4,5,6,7, 8, 9, 10, 11, 12,13) AND StatisticsTime=@MaxDate
	) AS SubQueryAlias
";
            return await ReadDb.QueryFirstAsync<PostOverview>(querySql);
        }
        /// <summary>
        /// 帖子日营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<PostDailyRevenue>> GetPostDailyRevenue(OperationOverviewReportParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@PageNo", param.PageNo);
            parameters.Add("@PageSize", param.PageSize);

            const string querySql = @"
DROP TABLE IF EXISTS #TempMMBusinessDataStatisticsReport;

SELECT
    CONVERT(DATE, StatisticsTime) AS [Date],
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS  SquareUnlockRevenue,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS  AgencyUnlockRevenue,
    SUM(CASE WHEN AggregateType = 14 THEN Amount ELSE 0 END) AS  OfficialPlatformRevenue,
    SUM(CASE WHEN AggregateType = 15 THEN Amount ELSE 0 END) AS  SquareRefundAmount,
    SUM(CASE WHEN AggregateType = 16 THEN Amount ELSE 0 END) AS  AgencyRefundAmount,
    SUM(CASE WHEN AggregateType = 17 THEN Amount ELSE 0 END) AS  OfficialRefundAmount
INTO #TempMMBusinessDataStatisticsReport
FROM MMBusinessDataStatisticsReport AS MPR WITH (NOLOCK)
WHERE DateType = 2
    AND AggregateType IN (12,13,14,15,16,17)
    AND StatisticsTime >= @BeginTime
    AND StatisticsTime < @EndTime
GROUP BY CONVERT(DATE, StatisticsTime);

SET @TotalCount =(SELECT COUNT(1) FROM #TempMMBusinessDataStatisticsReport)

SELECT 	(SquareUnlockRevenue+AgencyUnlockRevenue+OfficialPlatformRevenue) AS TotalRevenue,
(SquareRefundAmount+AgencyRefundAmount+OfficialRefundAmount) AS TotalRefund,
* FROM #TempMMBusinessDataStatisticsReport
ORDER BY [Date]
OFFSET (@PageNo - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;";

            var result = await ReadDb.QueryAsync<PostDailyRevenue>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<PostDailyRevenue>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        /// <summary>
        ///  帖子月营收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<PostMonthlyRevenue>> GetPostMonthlyRevenue(OperationOverviewReportParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@PageNo", param.PageNo);
            parameters.Add("@PageSize", param.PageSize);

            const string querySql = @"
DROP TABLE IF EXISTS #TempMMBusinessDataStatisticsReport;

SELECT
    CONVERT(DATE, StatisticsTime) AS [Month],
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS  SquareUnlockRevenue,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS  AgencyUnlockRevenue,
    SUM(CASE WHEN AggregateType = 14 THEN Amount ELSE 0 END) AS  OfficialPlatformRevenue,
    SUM(CASE WHEN AggregateType = 15 THEN Amount ELSE 0 END) AS  SquareRefundAmount,
    SUM(CASE WHEN AggregateType = 16 THEN Amount ELSE 0 END) AS  AgencyRefundAmount,
    SUM(CASE WHEN AggregateType = 17 THEN Amount ELSE 0 END) AS  OfficialRefundAmount
INTO #TempMMBusinessDataStatisticsReport
FROM MMBusinessDataStatisticsReport AS MPR WITH (NOLOCK)
WHERE DateType = 3
    AND AggregateType IN (12,13,14,15,16,17)
    AND StatisticsTime >= @BeginTime
    AND StatisticsTime < @EndTime
GROUP BY CONVERT(DATE, StatisticsTime);

SET @TotalCount =(SELECT COUNT(1) FROM #TempMMBusinessDataStatisticsReport)

SELECT 	(SquareUnlockRevenue+AgencyUnlockRevenue+OfficialPlatformRevenue) AS TotalRevenue,
(SquareRefundAmount+AgencyRefundAmount+OfficialRefundAmount) AS TotalRefund,
* FROM #TempMMBusinessDataStatisticsReport
ORDER BY [Month]
OFFSET (@PageNo - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;";

            var result = await ReadDb.QueryAsync<PostMonthlyRevenue>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<PostMonthlyRevenue>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        /// <summary>
        /// 帖子日趋势
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<PostDailyTrend>> GetPostDailyTrend(OperationOverviewReportParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@PageNo", param.PageNo);
            parameters.Add("@PageSize", param.PageSize);

            string querySql = @"
DROP TABLE IF EXISTS #TempMMPostStatisticsReport;

SELECT
    CONVERT(DATE, StatisticsTime) AS [Date],
    SUM(CASE WHEN AggregateType = 1 THEN Amount ELSE 0 END) AS  SquareDisplayed,
    SUM(CASE WHEN AggregateType = 2 THEN Amount ELSE 0 END) AS  SquareUnderReview,
    SUM(CASE WHEN AggregateType = 3 THEN Amount ELSE 0 END) AS  SquareNotApproved,
    SUM(CASE WHEN AggregateType = 4 THEN Amount ELSE 0 END) AS  SquareUnlockedCount,
    SUM(CASE WHEN AggregateType = 5 THEN Amount ELSE 0 END) AS  AgencyDisplayed,
    SUM(CASE WHEN AggregateType = 6 THEN Amount ELSE 0 END) AS  AgencyUnderReview,
    SUM(CASE WHEN AggregateType = 7 THEN Amount ELSE 0 END) AS  AgencyNotApproved,
    SUM(CASE WHEN AggregateType = 8 THEN Amount ELSE 0 END) AS  AgencyUnlockedCount,
    SUM(CASE WHEN AggregateType = 9 THEN Amount ELSE 0 END) AS  OfficialDisplayed,
    SUM(CASE WHEN AggregateType = 10 THEN Amount ELSE 0 END) AS  OfficialUnderReview,
    SUM(CASE WHEN AggregateType = 11 THEN Amount ELSE 0 END) AS  OfficialNotApproved,
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS  OfficialReservedCount,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS OfficialReserveCancelCount
INTO #TempMMPostStatisticsReport
FROM MMPostStatisticsReport AS MPR WITH (NOLOCK)
WHERE DateType = 2
    AND AggregateType IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13)
    AND StatisticsTime >= @BeginTime
    AND StatisticsTime < @EndTime
GROUP BY CONVERT(DATE, StatisticsTime);

SET @TotalCount =(SELECT COUNT(1) FROM #TempMMPostStatisticsReport)

SELECT 	(SquareDisplayed+AgencyDisplayed+OfficialDisplayed) AS TotalPostsInDisplay,* FROM #TempMMPostStatisticsReport
ORDER BY [Date]
OFFSET (@PageNo - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
";

            var result = await ReadDb.QueryAsync<PostDailyTrend>(querySql,parameters);
            var totalCount = parameters.Get<int>("@TotalCount");
            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);
            return new PageResultModel<PostDailyTrend>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
        /// <summary>
        /// 帖子月趋势
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<PostMonthlyTrend>> GetPostMonthlyTrend(OperationOverviewReportParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@BeginTime", param.BeginTime);
            parameters.Add("@EndTime", param.EndTime);
            parameters.Add("@PageNo", param.PageNo);
            parameters.Add("@PageSize", param.PageSize);

            string querySql = @"
DROP TABLE IF EXISTS #TempMMPostStatisticsReport;

SELECT
    CONVERT(DATE, StatisticsTime) AS [Month],
    SUM(CASE WHEN AggregateType = 1 THEN Amount ELSE 0 END) AS  SquareDisplayed,
    SUM(CASE WHEN AggregateType = 2 THEN Amount ELSE 0 END) AS  SquareUnderReview,
    SUM(CASE WHEN AggregateType = 3 THEN Amount ELSE 0 END) AS  SquareNotApproved,
    SUM(CASE WHEN AggregateType = 4 THEN Amount ELSE 0 END) AS  SquareUnlockedCount,
    SUM(CASE WHEN AggregateType = 5 THEN Amount ELSE 0 END) AS  AgencyDisplayed,
    SUM(CASE WHEN AggregateType = 6 THEN Amount ELSE 0 END) AS  AgencyUnderReview,
    SUM(CASE WHEN AggregateType = 7 THEN Amount ELSE 0 END) AS  AgencyNotApproved,
    SUM(CASE WHEN AggregateType = 8 THEN Amount ELSE 0 END) AS  AgencyUnlockedCount,
    SUM(CASE WHEN AggregateType = 9 THEN Amount ELSE 0 END) AS  OfficialDisplayed,
    SUM(CASE WHEN AggregateType = 10 THEN Amount ELSE 0 END) AS  OfficialUnderReview,
    SUM(CASE WHEN AggregateType = 11 THEN Amount ELSE 0 END) AS  OfficialNotApproved,
    SUM(CASE WHEN AggregateType = 12 THEN Amount ELSE 0 END) AS  OfficialReservedCount,
    SUM(CASE WHEN AggregateType = 13 THEN Amount ELSE 0 END) AS OfficialReserveCancelCount
INTO #TempMMPostStatisticsReport
FROM MMPostStatisticsReport AS MPR WITH (NOLOCK)
WHERE DateType = 3
    AND AggregateType IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13)
    AND StatisticsTime >= @BeginTime
    AND StatisticsTime < @EndTime
GROUP BY CONVERT(DATE, StatisticsTime);

SET @TotalCount =(SELECT COUNT(1) FROM #TempMMPostStatisticsReport)

SELECT 	(SquareDisplayed+AgencyDisplayed+OfficialDisplayed) AS TotalPostsInDisplay,* FROM #TempMMPostStatisticsReport
ORDER BY [Month]
OFFSET (@PageNo - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
";

            var result = await ReadDb.QueryAsync<PostMonthlyTrend>(querySql, parameters);
            var totalCount = parameters.Get<int>("@TotalCount");
            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);
            return new PageResultModel<PostMonthlyTrend>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
    }
}
