using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.db;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Extensions;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTPGameStoredProcedureRep : BaseDbRepository, ITPGameStoredProcedureRep
    {
        private static readonly string _transferSpSuccessCode = "2";
        private static readonly int _minRecheckOrderMinutes = -3;
        protected static readonly string OrderIdPrefix = "CTL"; //故意使用非品牌名稱(CheerTechLottery)
        protected static readonly string DepositActionCode = "D";
        protected static readonly string WithdrawActionCode = "W";


        public abstract PlatformProduct Product { get; } //先預留之後可能會用到

        public abstract string VIPFlowProductTableName { get; }

        public abstract string VIPPointsProductTableName { get; }

        public BaseTPGameStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        #region abstract
        protected abstract string GetProductInloTotalProfitLossSpName { get; }

        protected abstract string GetProductUserDailyTotalProfitLossSpName { get; }

        protected abstract string RptProfitLossCurrentTableName { get; }

        protected abstract string DWDailyProfitLossTableName { get; }

        protected abstract string SearchUnprocessedMoneyInInfoSpName { get; }

        protected abstract string SearchUnprocessedMoneyOutInfoSpName { get; }

        protected abstract string MoneyInInfoTableName { get; }

        protected abstract string MoneyOutInfoTableName { get; }

        protected abstract string TransferSuccessSpName { get; }

        protected abstract string TransferRollbackSpName { get; }

        protected abstract string TransferInSpName { get; }

        protected abstract string TransferOutSpName { get; }

        protected abstract string AddProfitlossAndPlayInfoSpName { get; }

        protected abstract string DwDailyAvailableScoresColumn { get; }

        protected abstract string DwDailyFreezeScoresColumn { get; }

        protected abstract string PlayInfoTableName { get; }

        protected abstract string ProfitlossTableName { get; }        


        protected virtual List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<TPGamePlayInfoRowModel>();
                //一律設定成identity,讓底層再建立temp table的時候做轉型
                selectColumnInfos.Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.PalyInfoID)).Single().IsIdentity = true;
                return selectColumnInfos;
            }
        }

        protected virtual List<SqlSelectColumnInfo> ProfitLossSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<TPGameProfitLossRowModel>();
                //一律設定成identity,讓底層再建立temp table的時候做轉型
                selectColumnInfos.Where(w => w.AliasName == nameof(TPGameProfitLossRowModel.ProfitLossID)).Single().IsIdentity = true;
                return selectColumnInfos;
            }
        }

        protected virtual Tuple<string, int?> GetIsWinFilter(List<int> isWins)
        {
            string filter = "AND [IsWin] IN @IsWins ";
            return new Tuple<string, int?>(filter, null);
        }

        protected virtual string ProfitLossCompareTimeColumnName => "ProfitLossTime";

        protected virtual TPGameProfitLossRowModelCompareTime CompareTimeProperty => TPGameProfitLossRowModelCompareTime.ProfitLossTime;

        /// <summary>
        /// 預設為非數值
        /// </summary>
        protected virtual bool IsMoneyIdNumeric { get; } = false;
        #endregion

        private string GetMoneyInfoTableKey(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "MoneyInID";
            }
            else
            {
                return "MoneyOutID";
            }
        }

        public virtual string GetTransferSpActionType(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }

        public TPGameSelfProfitLossSearchResult GetSelfReport(TPGameProfitLossSearchParam searchParam)
        {
            string strSql;

            strSql = String.Format($@"

                    IF OBJECT_ID('tempdb..#tmp_allChildren') IS NOT NULL
                        DROP TABLE #tmp_allChildren; 

                    --找出用戶的所有下級
                    SELECT UserID,ParentID,UserName,[Level],Level1UserID,Level1UserName INTO #tmp_allChildren FROM [Inlodb].[dbo].[fn_GetChildren](@UserID, 0, @MaxDirectChild, default) 

                    DECLARE @RegCount INT
                    DECLARE @TZCount INT

                    --在用戶的所有下級之中, 找出在該期間內注冊的用戶數目
                    SELECT @RegCount = Count(*)
                    FROM [Inlodb].[dbo].[Userinfo] as u WITH (NOLOCK)
                    INNER JOIN #tmp_allChildren as child WITH (NOLOCK)
                    ON u.UserID = child.UserID
                    WHERE @DtFrom <= RegTime AND RegTime < DATEADD(DAY, 1, @DtEnd)  

                    --在用戶的所有下級之中, 找出在該期間內投注的用戶數目
                    SELECT @TZCount = COUNT(distinct p.UserID)
                    FROM Inlodb.dbo.PalyInfo AS p WITH (NOLOCK)
                    INNER JOIN #tmp_allChildren AS child WITH (NOLOCK)
                    ON p.UserID = child.UserID
                    WHERE p.IsFactionAward = 1 AND @DtFrom <= p.Notetime AND p.Notetime < DATEADD(DAY, 1, @DtEnd) 

            ");


            if (searchParam.DtFrom == searchParam.DtEnd) //如果只拿一天的資料, 只需要考慮一個表; 否則需要合併DW表
            {
                strSql = strSql + String.Format($@"

                    --找出該日, 用戶的ProfitLossDetail總和
                    SELECT top 1  ZKYProfitLossMoney AS ProfitLoss
                                 ,TZProfitLossMoney AS Bet
                                 ,KYProfitLossMoney AS Prize
                                 ,CZProfitLossMoney AS MoneyIn
                                 ,TXProfitLossMoney AS MoneyOut
                                 ,FDProfitLossMoney AS RebateMoney
                                 ,YJProfitLossMoney AS Commission
                                 ,HBProfitLossMoney AS RedPocket
                                 ,@RegCount AS RegCount
                                 ,@TZCount AS TZCount
                    FROM {InlodbType.InlodbBak}.dbo.[{RptProfitLossCurrentTableName}] WITH (NOLOCK)
                    WHERE UserID = @UserID AND @DtFrom <= RecordDate AND RecordDate <= @DtEnd 

                    ");
            }
            else
            {
                strSql = strSql + String.Format($@"

                    --找出該期間, 用戶的ProfitLossDetail的總和
                    SELECT  ISNULL(SUM(ZKYProfitLossMoney),0) AS ProfitLoss
                            ,ISNULL(SUM(TZProfitLossMoney),0) AS Bet
                            ,ISNULL(SUM(KYProfitLossMoney),0) AS Prize
                            ,ISNULL(SUM(CZProfitLossMoney),0) AS MoneyIn
                            ,ISNULL(SUM(TXProfitLossMoney),0) AS MoneyOut
                            ,ISNULL(SUM(FDProfitLossMoney),0) AS RebateMoney
                            ,ISNULL(SUM(YJProfitLossMoney),0) AS Commission 
                            ,ISNULL(SUM(HBProfitLossMoney),0) AS RedPocket
                            ,@RegCount AS RegCount
                            ,@TZCount AS TZCount
                    FROM (
	                    --找出該期間, 用戶的ProfitLossDetail
                        SELECT RecordDate, ZKYProfitLossMoney, TZProfitLossMoney, KYProfitLossMoney, CZProfitLossMoney, TXProfitLossMoney, FDProfitLossMoney, YJProfitLossMoney, HBProfitLossMoney
                        FROM [{RptProfitLossCurrentTableName}]  WITH (NOLOCK)
                        WHERE UserID = @UserID AND DATEADD(DAY, -4, @DtEnd) <= RecordDate AND RecordDate <= @DtEnd
                        UNION
                        SELECT RecordDate, ZKYProfitLossMoney, TZProfitLossMoney, KYProfitLossMoney, CZProfitLossMoney, TXProfitLossMoney, FDProfitLossMoney, YJProfitLossMoney, HBProfitLossMoney
                        FROM [{DWDailyProfitLossTableName}]  WITH (NOLOCK)
                        WHERE UserID = @UserID AND @DtFrom <= RecordDate AND RecordDate < DATEADD(DAY, -4, @DtEnd)  
                    ) AS A

                    ");
            }

            bool isLottery = (Product == PlatformProduct.Lottery);

            //只有彩票有這兩個欄位，彩票以外都要取代掉
            if (!isLottery)
            {
                strSql = strSql.Replace(", HBProfitLossMoney", "");
                strSql = strSql.Replace(",HBProfitLossMoney AS RedPocket", "");
                strSql = strSql.Replace(",ISNULL(SUM(HBProfitLossMoney),0) AS RedPocket", "");
                strSql = strSql.Replace(", YJProfitLossMoney", "");
                strSql = strSql.Replace(",YJProfitLossMoney AS Commission", "");
                strSql = strSql.Replace(",ISNULL(SUM(YJProfitLossMoney),0) AS Commission", "");
            }

            TPGameSelfProfitLossSearchResult searchResult = DbHelper.QuerySingleOrDefault<TPGameSelfProfitLossSearchResult>(
                strSql,
                new
                {
                    UserID = searchParam.UserId,
                    DtFrom = searchParam.DtFrom,
                    DtEnd = searchParam.DtEnd,
                    MaxDirectChild = 20000
                }
                );

            if (searchResult == null)
            {
                searchResult = new TPGameSelfProfitLossSearchResult()
                {
                    Commission = 0,
                    RedPocket = 0,
                };
            }

            return searchResult;
        }

        public TPGameTeamProfitLossSearchResult GetTeamReport(TPGameTeamProfitLossSearchParam searchParam)
        {
            string strSql, strSource;

            ProfitLossReportSortTypes sortType = ProfitLossReportSortTypes.GetSingle(searchParam.SortType);

            if (sortType == null)
            {
                sortType = ProfitLossReportSortTypes.ProfitLossDesc;
            }

            //找出該期間, 用戶的所有下級的所有ProfitLossDetail 以及所屬團體的Level1UserName, Level1UserID
            //如果只拿一天的資料, 只需要考慮一個表; 否則需要合併DW表
            if (searchParam.DtFrom == searchParam.DtEnd)
            {
                strSource = String.Format($@" 
                            SELECT b.Level1UserID, b.Level1UserName, b.UserID, b.UserName, RecordDate, ZKYProfitLossMoney, TZProfitLossMoney, KYProfitLossMoney, CZProfitLossMoney, TXProfitLossMoney, FDProfitLossMoney, YJProfitLossMoney, HBProfitLossMoney
                            FROM {InlodbType.InlodbBak}.dbo.[{RptProfitLossCurrentTableName}] as rpt WITH (NOLOCK)  
                            INNER JOIN #tmp_allChildren as b
                            ON rpt.UserID = b.UserID
                            WHERE @DtFrom <= RecordDate AND RecordDate <= @DtEnd
                        ");
            }
            else
            {
                strSource = String.Format($@"
                            SELECT b.Level1UserID, b.Level1UserName, b.UserID, b.UserName, RecordDate, ZKYProfitLossMoney, TZProfitLossMoney, KYProfitLossMoney, CZProfitLossMoney, TXProfitLossMoney, FDProfitLossMoney, YJProfitLossMoney, HBProfitLossMoney
                            FROM {InlodbType.InlodbBak}.dbo.[{RptProfitLossCurrentTableName}] as rpt WITH (NOLOCK) 
                            INNER JOIN #tmp_allChildren as b
                            ON rpt.UserID = b.UserID
                            WHERE DATEADD(DAY, -4, @DtEnd) <= RecordDate AND RecordDate <= @DtEnd 
                            UNION 
                            SELECT c.Level1UserID, c.Level1UserName, c.UserID, c.UserName, RecordDate, ZKYProfitLossMoney, TZProfitLossMoney, KYProfitLossMoney, CZProfitLossMoney, TXProfitLossMoney, FDProfitLossMoney, YJProfitLossMoney, HBProfitLossMoney
                            FROM [{DWDailyProfitLossTableName}] as dw WITH (NOLOCK) 
                            INNER JOIN #tmp_allChildren as c
                            ON dw.UserID = c.UserID
                            WHERE @DtFrom <= RecordDate AND RecordDate < DATEADD(DAY, -4, @DtEnd) 
                        ");
            }


            strSql = String.Format($@"
                    IF OBJECT_ID('tempdb..#tmp_allChildren') IS NOT NULL
                        DROP TABLE #tmp_allChildren; 

                    IF OBJECT_ID('tempdb..#tmp_detail') IS NOT NULL
                        DROP TABLE #tmp_detail; 

                    --找出用戶的所有下級
                    SELECT UserID,ParentID,UserName,[Level],Level1UserID,Level1UserName INTO #tmp_allChildren FROM [Inlodb].[dbo].[fn_GetChildren](@UserID, 0, @MaxDirectChild, default) 

                    --找出該期間, 用戶的所有下級團體各自的ProfitLossDetail的總和 
                    SELECT  Level1UserName as 'UserName'
                            ,Level1UserID as 'UserID'
                            ,ISNULL(SUM(ZKYProfitLossMoney),0) ZKYProfitLossMoney
                            ,ISNULL(SUM(TZProfitLossMoney),0) TZProfitLossMoney
                            ,ISNULL(SUM(KYProfitLossMoney),0) KYProfitLossMoney
                            ,ISNULL(SUM(CZProfitLossMoney),0) CZProfitLossMoney
                            ,ISNULL(SUM(TXProfitLossMoney),0) TXProfitLossMoney
                            ,ISNULL(SUM(FDProfitLossMoney),0) FDProfitLossMoney
                            ,ISNULL(SUM(YJProfitLossMoney),0) YJProfitLossMoney
                            ,ISNULL(SUM(HBProfitLossMoney),0) HBProfitLossMoney
                    INTO #tmp_detail
                    FROM (
                        --找出該期間, 用戶的所有下級的所有ProfitLossDetail 以及所屬團體的Level1UserName, Level1UserID
                        {strSource}
                    ) AS A
                    GROUP BY Level1UserID, Level1UserName


                    --找出該期間, 用戶的所有下級團體的ProfitLossDetail的總和 
                    SELECT  ISNULL(SUM(ZKYProfitLossMoney),0) AS ProfitLoss
                            ,ISNULL(SUM(TZProfitLossMoney),0) AS Bet
                            ,ISNULL(SUM(KYProfitLossMoney),0) AS Prize
                            ,ISNULL(SUM(CZProfitLossMoney),0) AS MoneyIn
                            ,ISNULL(SUM(TXProfitLossMoney),0) AS MoneyOut
                            ,ISNULL(SUM(FDProfitLossMoney),0) AS RebateMoney
                            ,ISNULL(SUM(YJProfitLossMoney),0) AS Commission
                            ,ISNULL(SUM(HBProfitLossMoney),0) AS RedPocket
                    FROM #tmp_detail WITH (NOLOCK)

                    --找出該期間, 用戶的部份下級團體各自的ProfitLossDetail的總和 
                    SELECT    UserName
                            , UserID
                            , ZKYProfitLossMoney AS ProfitLoss
                            , TZProfitLossMoney  AS Bet
                            , KYProfitLossMoney  AS Prize
                            , CZProfitLossMoney  AS MoneyIn
                            , TXProfitLossMoney  AS MoneyOut
                            , FDProfitLossMoney  AS RebateMoney
                            , YJProfitLossMoney  AS Commission
                            , HBProfitLossMoney  AS RedPocket
                    FROM #tmp_detail WITH (NOLOCK)
                    ORDER BY { sortType.SqlScript }
                    OFFSET @Offset ROWS
                    FETCH NEXT @Limit ROWS ONLY 
                    ");

            bool isLottery = (Product == PlatformProduct.Lottery);

            //只有彩票有這兩個欄位，彩票以外都要取代掉
            if (!isLottery)
            {
                strSql = strSql.Replace(", YJProfitLossMoney, HBProfitLossMoney", "");
                strSql = strSql.Replace(",ISNULL(SUM(YJProfitLossMoney),0) YJProfitLossMoney", "");
                strSql = strSql.Replace(",ISNULL(SUM(HBProfitLossMoney),0) HBProfitLossMoney", "");
                strSql = strSql.Replace(",ISNULL(SUM(YJProfitLossMoney),0) AS Commission", "");
                strSql = strSql.Replace(",ISNULL(SUM(HBProfitLossMoney),0) AS RedPocket", "");
                strSql = strSql.Replace(", YJProfitLossMoney  AS Commission", "");
                strSql = strSql.Replace(", HBProfitLossMoney  AS RedPocket", "");
            }

            TPGameTeamProfitLossSearchResult searchResult = new TPGameTeamProfitLossSearchResult();

            DbHelper.QueryMultiple(
                strSql,
                new
                {
                    UserID = searchParam.UserId,
                    searchParam.DtFrom,
                    searchParam.DtEnd,
                    searchParam.Offset,
                    searchParam.Limit,
                    MaxDirectChild = 20000
                }, (reader) =>
                {
                    searchResult.Total = reader.ReadSingleOrDefault<TPGameProfitLossDetail>();
                    searchResult.List = reader.Read<TPGameChildrenProfitLossDetail>().ToList();
                });

            if (searchResult.Total == null)
            {
                searchResult.Total = new TPGameProfitLossDetail();
            }

            return searchResult;
        }

        public DateTime GetReportCenterLastModifiedTime()
        {
            string strSql = $@"--找出資料更新時間
                             SELECT Runtime
                             FROM {InlodbType.InlodbBak}.dbo.DW_Daily_JOB_RUNTIME WITH(NOLOCK)
                             WHERE TableName = @RPTable ";

            DateTime searchResult = DbHelper.QuerySingle<DateTime>(
                strSql,
                new
                {
                    RPTable = RptProfitLossCurrentTableName,
                }
                );

            return searchResult;
        }

        public PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat> GetTeamProfitloss(ProGetGameTeamUserTotalProfitLossParam param)
        {
            //先取得團隊人員
            List<ProfitlossUserInfo> level12Users = GetLevel12TeamUsers(param.LoginUserId, param.SearchUserName);
            DailyReportDateRange reportDateRange = GetSearchDailyReportDateRange(param.QueryStartDate, param.QueryEndDate);

            Dictionary<int, TeamUserTotalProfitloss> level12UserIdDic = level12Users
                .ToDictionary(d => d.UserId, d => new TeamUserTotalProfitloss()
                {
                    UserID = d.UserId,
                    UserName = d.UserName,
                    FullUserPaths = d.FullUserPaths,
                    ParentID = d.ParentID,
                    DataType = d.DataType
                });

            var inlodbTeamUserTotalProfitlossList = new List<TeamUserTotalProfitloss>();
            var dailyTeamUserTotalProfitlossList = new List<TeamUserTotalProfitloss>();

            ProfitlossUserInfo rootUserInfo = level12Users.Where(w => w.DataType == (int)ProfitlossReportDataTypes.Self).SingleOrDefault();

            if (rootUserInfo == null)
            {
                return new PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat>(param)
                {
                    TotalCount = 0,
                    AdditionalData = new TeamUserTotalProfitlossStat()
                };
            }

            string rootFullUserPath = rootUserInfo.FullUserPaths;

            //查詢inlodb資料
            if (reportDateRange.InlodbStartTime.HasValue && reportDateRange.InlodbEndTime.HasValue)
            {
                List<UserProfitlossStat> userProfitlossStats = GetProductInloTotalProfitLoss(
                    rootFullUserPath,
                    reportDateRange.InlodbStartTime.Value,
                    reportDateRange.InlodbEndTime.Value.ToQuerySmallThanTime(DatePeriods.Second),
                    param.ExclusiveAfterSaveTime);

                var dataMap = new Dictionary<int, TeamUserTotalProfitloss>();

                foreach (UserProfitlossStat userProfitlossStat in userProfitlossStats)
                {
                    TeamUserTotalProfitloss teamUserTotalProfitloss = null;

                    if (!dataMap.TryGetValue(userProfitlossStat.UserID, out teamUserTotalProfitloss))
                    {
                        teamUserTotalProfitloss = new TeamUserTotalProfitloss()
                        {
                            UserID = userProfitlossStat.UserID,
                            UserName = userProfitlossStat.UserName,
                            FullUserPaths = userProfitlossStat.FullUserPaths,
                            ParentID = userProfitlossStat.ParentID,
                            DataType = (int)ProfitlossReportDataTypes.Team
                        };

                        if (level12UserIdDic.ContainsKey(userProfitlossStat.UserID))
                        {
                            teamUserTotalProfitloss.DataType = level12UserIdDic[userProfitlossStat.UserID].DataType;
                        }

                        dataMap.Add(userProfitlossStat.UserID, teamUserTotalProfitloss);
                    }

                    ConvertProfitlossToColumns(userProfitlossStat, teamUserTotalProfitloss);
                }

                inlodbTeamUserTotalProfitlossList = dataMap.Select(s => s.Value).ToList();
            }

            //查詢日報資料
            if (reportDateRange.DailyStartDate.HasValue && reportDateRange.SmallThanDailyEndDate.HasValue)
            {
                dailyTeamUserTotalProfitlossList = GetProductUserDailyTotalProfitLoss(
                    rootFullUserPath,
                    reportDateRange.DailyStartDate.Value,
                    reportDateRange.SmallThanDailyEndDate.Value);
            }

            //兩個list相加
            List<TeamUserTotalProfitloss> bothSourceList = new List<TeamUserTotalProfitloss>();
            bothSourceList.AddRange(inlodbTeamUserTotalProfitlossList);
            bothSourceList.AddRange(dailyTeamUserTotalProfitlossList);

            List<TeamUserTotalProfitloss> teamUserTotalProfitlossList = bothSourceList
                .GroupBy(g => new { g.UserID, g.UserName, g.FullUserPaths, g.ParentID })
                .Select(s => new TeamUserTotalProfitloss()
                {
                    UserID = s.Key.UserID,
                    UserName = s.Key.UserName,
                    FullUserPaths = s.Key.FullUserPaths,
                    ParentID = s.Key.ParentID,
                    CZProfitLossMoney = s.Sum(m => m.CZProfitLossMoney),
                    TXProfitLossMoney = s.Sum(m => m.TXProfitLossMoney),
                    FDProfitLossMoney = s.Sum(m => m.FDProfitLossMoney),
                    TZProfitLossMoney = s.Sum(m => m.TZProfitLossMoney),
                    KYProfitLossMoney = s.Sum(m => m.KYProfitLossMoney),
                    ZKYProfitLossMoney = s.Sum(m => m.ZKYProfitLossMoney),
                    XJFDProfitLossMoney = s.Sum(m => m.XJFDProfitLossMoney),
                    YJProfitLossMoney = s.Sum(m => m.YJProfitLossMoney),
                    HBProfitLossMoney = s.Sum(m => m.HBProfitLossMoney)
                }).ToList();


            var pagedResult = new PagedResultWithAdditionalData<TeamUserTotalProfitloss, TeamUserTotalProfitlossStat>(param)
            {
                TotalCount = level12Users.Count(),
                AdditionalData = new TeamUserTotalProfitlossStat()
            };

            teamUserTotalProfitlossList.ForEach(f =>
            {
                pagedResult.AdditionalData.TotalCZProfitLossMoney += f.CZProfitLossMoney;
                pagedResult.AdditionalData.TotalTXProfitLossMoney += f.TXProfitLossMoney;
                pagedResult.AdditionalData.TotalFDProfitLossMoney += f.FDProfitLossMoney;
                pagedResult.AdditionalData.TotalTZProfitLossMoney += f.TZProfitLossMoney;
                pagedResult.AdditionalData.TotalKYProfitLossMoney += f.KYProfitLossMoney;
                pagedResult.AdditionalData.TotalZKYProfitLossMoney += f.ZKYProfitLossMoney;
                pagedResult.AdditionalData.TotalXJFDProfitLossMoney += f.XJFDProfitLossMoney;
                pagedResult.AdditionalData.TotalYJProfitLossMoney += f.YJProfitLossMoney;
                pagedResult.AdditionalData.TotalHBProfitLossMoney += f.HBProfitLossMoney;
            });

            var userPathsMap = new Dictionary<int, List<int>>();

            //每一筆統計過的盈虧累加到上級的團隊盈虧內
            foreach (TeamUserTotalProfitloss teamUserTotalProfitloss in teamUserTotalProfitlossList)
            {
                if (!userPathsMap.ContainsKey(teamUserTotalProfitloss.UserID))
                {
                    userPathsMap.Add(teamUserTotalProfitloss.UserID,
                        Regex.Split(teamUserTotalProfitloss.FullUserPaths, "//")
                        .Select(s => s.Replace("/", string.Empty).ToInt32())
                        .Where(w => w > 1)
                        .ToList());
                }

                List<int> userPathsIds = userPathsMap[teamUserTotalProfitloss.UserID];

                int matchCount = 0;

                foreach (int userPathsId in userPathsIds)
                {
                    if (level12UserIdDic.ContainsKey(userPathsId))
                    {
                        matchCount++;
                        TeamUserTotalProfitloss level12UserProfitloss = level12UserIdDic[userPathsId];

                        //DataType = 1 記錄個人
                        //DataType = 2 記錄團隊
                        if ((level12UserProfitloss.DataType == (int)ProfitlossReportDataTypes.Self && level12UserProfitloss.UserID == teamUserTotalProfitloss.UserID) ||
                            level12UserProfitloss.DataType == (int)ProfitlossReportDataTypes.Team)
                        {
                            level12UserProfitloss.CZProfitLossMoney += teamUserTotalProfitloss.CZProfitLossMoney;
                            level12UserProfitloss.TXProfitLossMoney += teamUserTotalProfitloss.TXProfitLossMoney;
                            level12UserProfitloss.FDProfitLossMoney += teamUserTotalProfitloss.FDProfitLossMoney;
                            level12UserProfitloss.TZProfitLossMoney += teamUserTotalProfitloss.TZProfitLossMoney;
                            level12UserProfitloss.KYProfitLossMoney += teamUserTotalProfitloss.KYProfitLossMoney;
                            level12UserProfitloss.ZKYProfitLossMoney += teamUserTotalProfitloss.ZKYProfitLossMoney;
                            level12UserProfitloss.XJFDProfitLossMoney += teamUserTotalProfitloss.XJFDProfitLossMoney;
                            level12UserProfitloss.YJProfitLossMoney += teamUserTotalProfitloss.YJProfitLossMoney;
                            level12UserProfitloss.HBProfitLossMoney += teamUserTotalProfitloss.HBProfitLossMoney;
                        }
                    }

                    //因為只有取得樹狀結構中的level1,2級用戶,所以最多match 2次
                    if (matchCount >= 2)
                    {
                        break;
                    }
                }
            }

            IOrderedEnumerable<TeamUserTotalProfitloss> orderedEnum = level12UserIdDic.Select(s =>
            {
                TeamUserTotalProfitloss teamUserTotalProfitloss = level12UserIdDic[s.Key];

                return new TeamUserTotalProfitloss()
                {
                    DataType = teamUserTotalProfitloss.DataType,
                    UserID = teamUserTotalProfitloss.UserID,
                    UserName = teamUserTotalProfitloss.UserName,
                    CZProfitLossMoney = s.Value.CZProfitLossMoney,
                    TXProfitLossMoney = s.Value.TXProfitLossMoney,
                    FDProfitLossMoney = s.Value.FDProfitLossMoney,
                    TZProfitLossMoney = s.Value.TZProfitLossMoney,
                    KYProfitLossMoney = s.Value.KYProfitLossMoney,
                    ZKYProfitLossMoney = s.Value.ZKYProfitLossMoney,
                    XJFDProfitLossMoney = s.Value.XJFDProfitLossMoney,
                    YJProfitLossMoney = s.Value.YJProfitLossMoney,
                    HBProfitLossMoney = s.Value.HBProfitLossMoney
                };
            }).OrderBy(o => o.DataType);

            if (param.SortModel.Sort == SortOrder.Descending)
            {
                orderedEnum = orderedEnum.ThenByDescending(t => t.GetType().GetProperty(param.SortModel.ColumnName).GetValue(t));
            }
            else
            {
                orderedEnum = orderedEnum.ThenBy(t => t.GetType().GetProperty(param.SortModel.ColumnName).GetValue(t));
            }

            //用帳號再排一次,避免盈虧一樣時順序會有時候不同
            orderedEnum = orderedEnum.ThenBy(t => t.UserName);

            //加入自己
            pagedResult.ResultList = orderedEnum.Where(w => w.DataType == (int)ProfitlossReportDataTypes.Self).ToList();
            //處理team分頁
            pagedResult.ResultList.AddRange(orderedEnum
                .Where(w => w.DataType == (int)ProfitlossReportDataTypes.Team)
                .Skip(param.Offset)
                .Take(param.PageSize));

            //本頁合計
            pagedResult.ResultList.ForEach(f =>
            {
                pagedResult.AdditionalData.PageTotalCZProfitLossMoney += f.CZProfitLossMoney;
                pagedResult.AdditionalData.PageTotalTXProfitLossMoney += f.TXProfitLossMoney;
                pagedResult.AdditionalData.PageTotalFDProfitLossMoney += f.FDProfitLossMoney;
                pagedResult.AdditionalData.PageTotalTZProfitLossMoney += f.TZProfitLossMoney;
                pagedResult.AdditionalData.PageTotalKYProfitLossMoney += f.KYProfitLossMoney;
                pagedResult.AdditionalData.PageTotalZKYProfitLossMoney += f.ZKYProfitLossMoney;
                pagedResult.AdditionalData.PageTotalXJFDProfitLossMoney += f.XJFDProfitLossMoney;
                pagedResult.AdditionalData.PageTotalYJProfitLossMoney += f.YJProfitLossMoney;
                pagedResult.AdditionalData.PageTotalHBProfitLossMoney += f.HBProfitLossMoney;
            });

            return pagedResult;
        }

        private List<ProfitlossUserInfo> GetLevel12TeamUsers(int loginUserId, string searchUserName)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_GetLevel12TeamUsers";
            List<ProfitlossUserInfo> level12Users = DbHelper.QueryList<ProfitlossUserInfo>(sql,
                new
                {
                    loginUserId,
                    searchUserName = searchUserName.ToNVarchar(50)
                },
                CommandType.StoredProcedure);

            return level12Users;
        }

        private DailyReportDateRange GetSearchDailyReportDateRange(DateTime queryStartDate, DateTime querySmallEqaulThanEndDate)
        {
            string sql = $@"
DECLARE @InlodbStartTime DATETIME;
DECLARE @InlodbEndTime DATETIME;
DECLARE @DailyStartDate DATE;
DECLARE @SmallThanDailyEndDate DATE;
EXEC {InlodbType.InlodbBak}.dbo.Pro_GetSearchDailyReportDateRange
    @SearchStartTime = @QueryStartDate,
    @SearchEndTime = @querySmallEqaulThanEndDate,
    @InlodbStartTime = @InlodbStartTime OUTPUT,
    @InlodbEndTime = @InlodbEndTime OUTPUT,
    @DailyStartDate = @DailyStartDate OUTPUT,
    @SmallThanDailyEndDate = @SmallThanDailyEndDate OUTPUT;

SELECT @InlodbStartTime AS InlodbStartTime,
       @InlodbEndTime AS InlodbEndTime,
       @DailyStartDate AS DailyStartDate,
       @SmallThanDailyEndDate AS SmallThanDailyEndDate ";

            DailyReportDateRange dailyReportDateRange = DbHelper
                .QuerySingle<DailyReportDateRange>(sql, new { queryStartDate, querySmallEqaulThanEndDate });
            return dailyReportDateRange;
        }

        private List<UserProfitlossStat> GetProductInloTotalProfitLoss(
            string rootFullUserPaths,
            DateTime queryStartDate,
            DateTime querySmallThanEndDate,
            DateTime? exclusiveAfterSaveTime)
        {
            string sql = $"{InlodbType.InlodbBak}.dbo.{GetProductInloTotalProfitLossSpName}";
            object param = new
            {
                rootFullUserPaths = rootFullUserPaths.ToNVarchar(4000),
                QueryStartDate = queryStartDate,
                QuerySmallThanEndDate = querySmallThanEndDate,
                ExclusiveAfterSaveTime = exclusiveAfterSaveTime
            };

            return DbHelper.QueryList<UserProfitlossStat>(sql, param, CommandType.StoredProcedure);
        }

        private List<TeamUserTotalProfitloss> GetProductUserDailyTotalProfitLoss(
            string rootFullUserPaths,
            DateTime queryStartDate,
            DateTime querySmallThanEndDate)
        {
            string sql = $"{InlodbType.InlodbBak}.dbo.{GetProductUserDailyTotalProfitLossSpName}";
            object param = new
            {
                rootFullUserPaths = rootFullUserPaths.ToNVarchar(4000),
                QueryStartDate = queryStartDate,
                QuerySmallThanEndDate = querySmallThanEndDate
            };

            return DbHelper.QueryList<TeamUserTotalProfitloss>(sql, param, CommandType.StoredProcedure);
        }

        public List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo()
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{SearchUnprocessedMoneyInInfoSpName}";
            return DbHelper.QueryList<TPGameMoneyInInfo>(sql, null, CommandType.StoredProcedure);
        }

        public List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            string tableKey = GetMoneyInfoTableKey(true);
            string processingMoneyInSql = SearchTPGameProcessingMoneyInfo(tableKey, MoneyInInfoTableName);
            return DbHelper.QueryList<TPGameMoneyInInfo>(processingMoneyInSql, new
            {
                ProcessingStatus = TPGameMoneyInOrderStatus.Processing.Value,
                ManualStatus = TPGameMoneyInOrderStatus.Manual.Value
            });
        }

        public List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo()
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{SearchUnprocessedMoneyOutInfoSpName}";
            return DbHelper.QueryList<TPGameMoneyOutInfo>(sql, null, CommandType.StoredProcedure);
        }

        public List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo()
        {
            string tableKey = GetMoneyInfoTableKey(false);
            string processingMoneyInSql = SearchTPGameProcessingMoneyInfo(tableKey, MoneyOutInfoTableName);
            return DbHelper.QueryList<TPGameMoneyOutInfo>(processingMoneyInSql, new
            {
                ProcessingStatus = TPGameMoneyOutOrderStatus.Processing.Value,
                ManualStatus = TPGameMoneyOutOrderStatus.Manual.Value
            });
        }

        private string SearchTPGameProcessingMoneyInfo(string tableKey, string tableName)
        {
            string sql = $@"
-- 撈取設定分鐘前為處理中的訂單
DECLARE @ProcessingDate DATETIME = DATEADD(MINUTE, {_minRecheckOrderMinutes}, GETDATE())
DECLARE	@StartDate DATETIME = @ProcessingDate - 3,
		@EndDate DATETIME =  @ProcessingDate

SELECT  {tableKey}, Amount, OrderID, OrderTime, Handle, 
        HandTime, UserID, UserName, [Status], Memo
FROM    {InlodbType.Inlodb.Value}.dbo.{tableName} WITH(NOLOCK)
WHERE   ([Status] = @ProcessingStatus AND OrderTime >= @StartDate AND OrderTime <= @EndDate) OR 
         [Status] = @ManualStatus 
";
            return sql;
        }

        public bool DoTransferSuccess(bool isMoneyIn, string moneyInfoId, UserScore userScore)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferSuccessSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = moneyInfoId.ToVarchar(32),
                    ActionType = GetTransferSpActionType(isMoneyIn).ToNVarchar(20),
                    userScore.AvailableScores,
                    userScore.FreezeScores,
                    ProductName = Product.Name
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }

        public bool DoTransferRollback(bool isMoneyIn, string moneyInfoId, string msg)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferRollbackSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = moneyInfoId.ToVarchar(32),
                    ProductName = Product.Name,
                    ActionType = GetTransferSpActionType(isMoneyIn),
                    msg = msg.ToNVarchar(1024)
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }

        public bool UpdateMoneyInOrderStatusFromManualToProcessing(string moneyInId)
        {
            return CommonUpdateMoneyOrderStatusFromManualToProcessing(true, moneyInId);            
        }

        public bool UpdateMoneyOutOrderStatusFromManualToProcessing(string moneyOutId)
        {
            return CommonUpdateMoneyOrderStatusFromManualToProcessing(false, moneyOutId);
        }

        private bool CommonUpdateMoneyOrderStatusFromManualToProcessing(bool isMoneyIn, string moneyIdValue)
        {
            string sql = GetChangeOrderStatusFromManualToProcessingSQL(isMoneyIn);
            dynamic moneyId;

            if (IsMoneyIdNumeric)
            {
                moneyId = moneyIdValue.ToInt32();
            }
            else
            {
                moneyId = moneyIdValue.ToVarchar(32);
            }

            return DbHelper.Execute(sql, new
            {
                moneyId,
                ProcessingStatus = TPGameMoneyOutOrderStatus.Processing.Value,
                ManualStatus = TPGameMoneyOutOrderStatus.Manual.Value
            }) > 0;
        }

        private string GetChangeOrderStatusFromManualToProcessingSQL(bool isMoneyIn)
        {
            string tableName;

            if (isMoneyIn)
            {
                tableName = MoneyInInfoTableName;
            }
            else
            {
                tableName = MoneyOutInfoTableName;
            }

            string sql = $@"UPDATE {InlodbType.Inlodb}.dbo.{tableName}
                                SET [Status] = @ProcessingStatus 
                            WHERE {GetMoneyInfoTableKey(isMoneyIn)} = @moneyId 
                                  AND [Status] = @ManualStatus ";
            return sql;
        }

        public string CreateMoneyInOrder(int userID, decimal amount, string tpGameAccount)
        {
            string moneyId = GetTableSequence(MoneyInInfoTableName);
            string orderId = CreateOrderId(true, moneyId, userID, tpGameAccount);
            string productName = Product.Name;
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferInSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    userID,
                    amount,
                    productName,
                    moneyId,
                    orderId
                },
                CommandType.StoredProcedure);
        }

        public string CreateMoneyOutOrder(int userID, decimal amount, string tpGameAccount)
        {
            string moneyId = GetTableSequence(MoneyOutInfoTableName);
            string orderId = CreateOrderId(false, moneyId, userID, tpGameAccount);
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferOutSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    userID,
                    amount,
                    moneyId,
                    orderId
                },
                CommandType.StoredProcedure);
        }

        public BaseReturnModel AddProductProfitLossAndPlayInfo(InsertTPGameProfitlossParam tpGameProfitloss)
        {
            string sql = $@"
EXEC {InlodbType.Inlodb.Value}.dbo.{AddProfitlossAndPlayInfoSpName}
    @UserID = @UserID,
    @ProfitLossTime = @ProfitLossTime,
    @ProfitLossType = @ProfitLossType,
    @ProfitLossMoney = @ProfitLossMoney,
    @WinMoney = @WinMoney,
    @PrizeMoney = @PrizeMoney,
    @IsWin = @IsWin,
    @Memo = @Memo,
    @PalyID = @PalyID,
    @GameType = @GameType,
    @BetTime = @BetTime,
    @AllBetMoney = @AllBetMoney,
    @HighestParentRebateMoney = @HighestParentRebateMoney,
    @GrandParentRebateMoney = @GrandParentRebateMoney,
    @ParentRebateMoney = @ParentRebateMoney,
    @SelfRebateMoney = @SelfRebateMoney,
    @AvailableScores = @AvailableScores,
    @FreezeScores = @FreezeScores ";

            string errorMsg = DbHelper.ExecuteScalar<string>(
                sql,
                tpGameProfitloss);

            BaseReturnModel returnModel;

            if (!errorMsg.IsNullOrEmpty())
            {
                returnModel = new BaseReturnModel(errorMsg);
            }
            else
            {
                returnModel = new BaseReturnModel(ReturnCode.Success);
            }

            return returnModel;
        }

        public PlatformTotalProfitlossStat GetPlatformProfitLoss(SearchPlatformProfitLossParam searchParam)
        {
            DailyReportDateRange reportDateRange = GetSearchDailyReportDateRange(searchParam.StartDate, searchParam.EndDate);
            var platformTotal = new PlatformTotalProfitloss();

            // 平台資料
            string platformUserPath = "/0/";

            //查詢inlodb資料
            if (reportDateRange.InlodbStartTime.HasValue && reportDateRange.InlodbEndTime.HasValue)
            {
                List<UserProfitlossStat> userProfitlossStats =
                    GetProductInloTotalProfitLoss(platformUserPath, reportDateRange.InlodbStartTime.Value, reportDateRange.InlodbEndTime.Value, null)
                    .GroupBy(g => g.ProfitLossType)
                    .Select(s => new UserProfitlossStat()
                    {
                        ProfitLossType = s.Key,
                        TotalProfitLossMoney = s.Sum(m => m.TotalProfitLossMoney),
                        TotalWinMoney = s.Sum(m => m.TotalWinMoney),
                        TotalPrizeMoney = s.Sum(m => m.TotalPrizeMoney),
                    }).ToList();

                foreach (UserProfitlossStat userProfitlossStat in userProfitlossStats)
                {
                    if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.CZ)
                    {
                        platformTotal.Charge += userProfitlossStat.TotalProfitLossMoney;
                    }
                    else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.TX)
                    {
                        platformTotal.Withdraw += userProfitlossStat.TotalProfitLossMoney;
                    }
                    else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.KY)
                    {
                        platformTotal.BetAmount += userProfitlossStat.TotalProfitLossMoney;
                        platformTotal.ProfitLoss += userProfitlossStat.TotalPrizeMoney;
                    }
                    else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.FD)
                    {
                        platformTotal.Rebate += userProfitlossStat.TotalProfitLossMoney + userProfitlossStat.TotalWinMoney;
                    }
                    else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.TX)
                    {
                        platformTotal.Withdraw += userProfitlossStat.TotalWinMoney;
                    }
                }
            }

            //查詢日報資料
            if (reportDateRange.DailyStartDate.HasValue && reportDateRange.SmallThanDailyEndDate.HasValue)
            {
                List<TeamUserTotalProfitloss> dailyTeamUserTotalProfitlossList = GetProductUserDailyTotalProfitLoss(
                    platformUserPath,
                    reportDateRange.DailyStartDate.Value,
                    reportDateRange.SmallThanDailyEndDate.Value)
                    .GroupBy(g => new { g.CZProfitLossMoney, g.TXProfitLossMoney, g.TZProfitLossMoney, g.FDProfitLossMoney, g.KYProfitLossMoney })
                    .Select(s => new TeamUserTotalProfitloss()
                    {
                        CZProfitLossMoney = s.Sum(m => m.CZProfitLossMoney),
                        TXProfitLossMoney = s.Sum(m => m.TXProfitLossMoney),
                        TZProfitLossMoney = s.Sum(m => m.TZProfitLossMoney),
                        FDProfitLossMoney = s.Sum(m => m.FDProfitLossMoney),
                        KYProfitLossMoney = s.Sum(m => m.KYProfitLossMoney),
                    }).ToList();

                dailyTeamUserTotalProfitlossList.ForEach(f =>
                {
                    platformTotal.Charge += f.CZProfitLossMoney;
                    platformTotal.Withdraw += f.TXProfitLossMoney;
                    platformTotal.BetAmount += f.TZProfitLossMoney;
                    platformTotal.Rebate += f.FDProfitLossMoney;
                    platformTotal.ProfitLoss += f.KYProfitLossMoney;
                });
            }

            PlatformScoreStat platformScoreStat = GetLastestPlatformScoreStat();

            var profitlossStat = new PlatformTotalProfitlossStat()
            {
                MoneyIn = platformTotal.Charge,
                MoneyOut = platformTotal.Withdraw,
                AllMoney = platformTotal.Charge - platformTotal.Withdraw,
                BetMoney = platformTotal.BetAmount,
                AllPctMoney = platformTotal.Rebate,
                ProfitLoss = platformTotal.ProfitLoss,
                AllProfitLoss = platformTotal.BetAmount - platformTotal.Rebate - platformTotal.ProfitLoss,
                SumAvailableScores = platformScoreStat.SumAvailableScores,
                SumFreezeScores = platformScoreStat.SumFreezeScores
            };

            return profitlossStat;
        }

        /// <summary>
        /// 寫入登入紀錄表
        /// </summary>
        public void CreateLoginHistory(TPGameCreateLoginHistoryParam tpgameCreateLoginHistoryParam)
        {
            //todo 不要用拼接字串的方式組DB物件,要改成抽象的方式處理
            string sql = $@"
                DECLARE @LoginAddress NVARCHAR(200);
                SET @LoginAddress = {InlodbType.Inlodb.Value}.[dbo].[fn_GetIPArea](@LoginIp, @IPVersion, @IPNumber);
						 
				DECLARE @LoginHistory_SEQID VARCHAR(32);
				EXEC [dbo].[Pro_GetSequenceIdentity]
				@SequenceName = N'SEQ_{Product.Value}LoginHistory_SEQID',
				@SEQID = @LoginHistory_SEQID  OUTPUT;

                INSERT dbo.[{Product.Value}LoginHistory] (SEQID, {Product.Value}Type, UserId, UserName, LoginTime, LoginIp, LoginAddress) 
                Values(@LoginHistory_SEQID, @Type, @UserID, @UserName, GETDATE(), @LoginIp, @LoginAddress);
            ";

            var param = new
            {
                tpgameCreateLoginHistoryParam.UserID,
                UserName = tpgameCreateLoginHistoryParam.UserName.ToNVarchar(50),
                LoginIp = tpgameCreateLoginHistoryParam.Ipinformation.DestinationIP.ToVarchar(128),
                IPVersion = tpgameCreateLoginHistoryParam.Ipinformation.DestinationIPVersionNumber,
                IPNumber = tpgameCreateLoginHistoryParam.Ipinformation.DestinationIPNumberString.ToVarchar(128),
                tpgameCreateLoginHistoryParam.Type
            };
            DbHelper.Execute(sql, param);
        }

        /// <summary>
        /// 前台取得盈虧明細
        /// </summary>
        public List<TPGameProfitLoss> GetTPGameProfitLossDeal(SearchTPGameProfitLossParam searchTPGameProfitLossDealParam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  TOP " + searchTPGameProfitLossDealParam.PageSize + " * FROM ");
            strSql.Append($@"(
                SELECT ROW_NUMBER() OVER (ORDER BY a.BetTime  DESC) AS RowNumber, 
                    a.ProfitLossID, 
                    a.UserID, 
                    a.bettime AS ProfitLossTime, 
                    a.ProfitLossType,
                    a.ProfitLossMoney,
                    a.WinMoney,
                    a.PrizeMoney, 
                    a.Memo 
                FROM {InlodbType.Inlodb}.dbo.{ProfitlossTableName} a WITH(NOLOCK) 
                WHERE a.UserID=@UserID AND a.bettime BETWEEN @start AND @end");

            if (searchTPGameProfitLossDealParam.ProfitLossType == ProfitLossTypeName.FD ||
                searchTPGameProfitLossDealParam.ProfitLossType == ProfitLossTypeName.KY ||
                searchTPGameProfitLossDealParam.ProfitLossType == ProfitLossTypeName.CZ ||
                searchTPGameProfitLossDealParam.ProfitLossType == ProfitLossTypeName.TX)
            {
                strSql.Append(" AND a.ProfitLossType=@ProfitLossType");
            }

            strSql.Append(" ) A WHERE RowNumber > " + searchTPGameProfitLossDealParam.PageIndex);
            return DbHelper.QueryList<TPGameProfitLoss>(strSql.ToString(), new
            {
                searchTPGameProfitLossDealParam.UserID,
                start = searchTPGameProfitLossDealParam.StartTime,
                end = searchTPGameProfitLossDealParam.EndTime,
                ProfitLossType = searchTPGameProfitLossDealParam.ProfitLossType
            });
        }

        /// <summary>站台共用的投注紀錄</summary>
        public PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter> GetPlayInfoList(SearchTPGamePlayInfoParam searchParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.StartTime, searchParam.EndTime);
            List<int> isWins = searchParam.IsWins;
            int? isFactionAward = null;

            if (isWins == null)
            {
                isWins = new List<int>();
            }

            if (searchParam.IsWin.HasValue && !isWins.Contains(searchParam.IsWin.Value))
            {
                isWins.Add(searchParam.IsWin.Value);
            }

            string commonFilter = null;

            if (searchParam.UserID.HasValue)
            {
                commonFilter += $"AND UserID = @UserId ";
            }
            else if (!searchParam.UserName.IsNullOrEmpty())
            {
                commonFilter += $"AND UserID = (SELECT TOP 1 UserID FROM {InlodbType.Inlodb}.dbo.UserInfo WITH(NOLOCK) WHERE UserName = @UserName) ";
            }

            if (isWins.AnyAndNotNull())
            {
                var filterTuple = GetIsWinFilter(isWins);
                commonFilter += filterTuple.Item1;
                isFactionAward = filterTuple.Item2;
            }

            searchParam.InitSortModelsBySortField();

            if (!searchParam.SortModels.AnyAndNotNull())
            {
                searchParam.SortModels = new List<SortModel>()
                {
                    new SortModel()
                    {
                        ColumnName = nameof(TPGamePlayInfoRowModel.BetTime), //可以用別名排序, 所以直接指定model欄位即可
                        Sort = SortOrder.Descending
                    }
                };
            }

            //加上第二排序避免時間一樣時分頁會有問題
            searchParam.SortModels.Add(new SortModel()
            {
                ColumnName = nameof(TPGamePlayInfoRowModel.PalyInfoID), //可以用別名排序, 所以直接指定model欄位即可
                Sort = SortOrder.Descending
            });


            TPGamePlayInfoFooter footer = new TPGamePlayInfoFooter();
            var selectColumns = PlayInfoSelectColumnInfos;
            string betMoneyColumnName = selectColumns.Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.BetMoney)).Single().ColumnName;
            string betTimeColumnName = selectColumns.Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.BetTime)).Single().ColumnName;
            var tpGamePlayInfoStatModels = new List<TPGamePlayInfoStatModel>();

            string inlodbFilters = null;
            string inlodbBakFilters = null;

            if (dateRange.InlodbStartDate.HasValue && dateRange.SmallThanInlodbEndDate.HasValue)
            {
                inlodbFilters = $"{betTimeColumnName} >= @{nameof(dateRange.InlodbStartDate)} " +
                    $"AND {betTimeColumnName} < @{nameof(dateRange.SmallThanInlodbEndDate)} " + commonFilter;
            }

            if (dateRange.InlodbBakStartDate.HasValue && dateRange.SmallThanInlodbBakEndDate.HasValue)
            {
                inlodbBakFilters = $"{betTimeColumnName} >= @{nameof(dateRange.InlodbBakStartDate)} " +
                    $"AND {betTimeColumnName} < @{nameof(dateRange.SmallThanInlodbBakEndDate)} " + commonFilter;
            }

            PagedResultModel<TPGamePlayInfoRowModel> pagedResult = GetCrossDbQueryList<TPGamePlayInfoRowModel>(new JxCrossDbQueryParam()
            {
                BasicTableName = PlayInfoTableName,
                SelectColumnInfos = PlayInfoSelectColumnInfos,
                StatColumns = $"SUM({betMoneyColumnName}) AS TotalBetMoney, SUM(WinMoney) AS TotalWinMoney ",
                InlodbFilters = inlodbFilters,
                InlodbBakFilters = inlodbBakFilters,
                OrderBy = searchParam.ToOrderByText(),
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize,
                Parameters = new
                {
                    UserId = searchParam.UserID,
                    UserName = searchParam.UserName.ToNVarchar(50),
                    isWins,
                    isFactionAward,
                    searchParam.PageNo,
                    searchParam.PageSize,
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                }
            },
            (gridReader) =>
            {
                TPGamePlayInfoStatModel statModel = gridReader.ReadSingle<TPGamePlayInfoStatModel>();

                if (statModel != null)
                {
                    tpGamePlayInfoStatModels.Add(statModel);
                }
            });

            if (tpGamePlayInfoStatModels.Any())
            {
                footer.TotalStat.TotalBetMoney = tpGamePlayInfoStatModels.Sum(s => s.TotalBetMoney);
                footer.TotalStat.TotalWinMoney = tpGamePlayInfoStatModels.Sum(s => s.TotalWinMoney);
            }

            //本頁合計
            footer.PageStat.TotalBetMoney = pagedResult.ResultList.Sum(s => s.BetMoney);
            footer.PageStat.TotalWinMoney = pagedResult.ResultList.Sum(s => s.WinMoney);

            return new PagedResultWithAdditionalData<TPGamePlayInfoRowModel, TPGamePlayInfoFooter>(searchParam)
            {
                ResultList = pagedResult.ResultList,
                TotalCount = pagedResult.TotalCount,
                AdditionalData = footer
            };
        }

        /// <summary>
        /// 取得單筆注單
        /// </summary>
        public TPGamePlayInfoRowModel GetSinglePlayInfo(int userId, string playInfoId)
        {
            SqlSelectColumnInfo palyInfoIdColumnInfo = PlayInfoSelectColumnInfos.Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.PalyInfoID)).Single();

            string playInfoIdColumnName = palyInfoIdColumnInfo.ColumnName;
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("UserID", userId);

            if (palyInfoIdColumnInfo.ColumnDbType.HasValue)
            {
                dynamicParameters.Add("playInfoId", playInfoId, palyInfoIdColumnInfo.ColumnDbType);
            }
            else
            {
                dynamicParameters.Add("playInfoId", playInfoId.ToVarchar(32));
            }

            string sql = $@"SELECT {string.Join(",", PlayInfoSelectColumnInfos.Select(s => s.FullColumnName))}
                FROM {InlodbType.Inlodb}.dbo.{PlayInfoTableName} WITH(NOLOCK) WHERE UserID = @userId AND {playInfoIdColumnName} = @playInfoId ";

            return DbHelper.QuerySingleOrDefault<TPGamePlayInfoRowModel>(sql, dynamicParameters);
        }

        /// <summary>站台共用的盈虧紀錄</summary>
        public PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn> GetUserProfitLossDetails(CommonSearchTPGameProfitLossParam searchParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.StartTime, searchParam.EndTime);
            string commonFilter = null;

            //第三方沒有契約分紅,故這邊不排除契約分紅TYPE
            if (!searchParam.ProfitLossType.IsNullOrEmpty())
            {
                commonFilter += "AND ProfitLossType = @ProfitLossType ";
            }

            if (searchParam.SearchUserID.HasValue)
            {
                commonFilter += "AND UserID = @SearchUserID ";
            }

            searchParam.InitSortModelsBySortField();

            if (!searchParam.SortModels.AnyAndNotNull())
            {
                searchParam.SortModels = new List<SortModel>()
                {
                    new SortModel()
                    {
                        ColumnName = nameof(TPGameProfitLossRowModel.BetTime), //可以用別名排序, 所以直接指定model欄位即可
                        Sort = SortOrder.Descending
                    }
                };
            }

            //加上第二排序避免時間一樣時分頁會有問題
            searchParam.SortModels.Add(new SortModel()
            {
                ColumnName = nameof(TPGameProfitLossRowModel.ProfitLossID), //可以用別名排序, 所以直接指定model欄位即可
                Sort = SortOrder.Descending
            });

            var statColumn = new ProfitLossStatColumn();
            List<SqlSelectColumnInfo> selectColumns = ProfitLossSelectColumnInfos;
            string statColumns = null;
            string statGroupByColumns = null;

            if (searchParam.IsCalculateStat)
            {
                statColumns = $"ProfitLossType, SUM(ProfitlossMoney) AS TotalProfitlossMoney, SUM(WinMoney) AS TotalWinMoney, SUM(PrizeMoney) AS TotalPrizeMoney ";
                statGroupByColumns = "ProfitLossType";
            }

            List<BasicUserProfitlossStat> userProfitlossStats = null;
            string inlodbFilters = null;
            string inlodbBakFilters = null;

            if (dateRange.InlodbStartDate.HasValue && dateRange.SmallThanInlodbEndDate.HasValue)
            {
                inlodbFilters = $"{ProfitLossCompareTimeColumnName} >= @{nameof(dateRange.InlodbStartDate)} " +
                    $"AND {ProfitLossCompareTimeColumnName} < @{nameof(dateRange.SmallThanInlodbEndDate)} " + commonFilter;
            }

            if (dateRange.InlodbBakStartDate.HasValue && dateRange.SmallThanInlodbBakEndDate.HasValue)
            {
                inlodbBakFilters = $"{ProfitLossCompareTimeColumnName} >= @{nameof(dateRange.InlodbBakStartDate)} " +
                    $"AND {ProfitLossCompareTimeColumnName} < @{nameof(dateRange.SmallThanInlodbBakEndDate)} " + commonFilter;
            }

            PagedResultModel<TPGameProfitLossRowModel> pagedResult = GetCrossDbQueryList<TPGameProfitLossRowModel>(new JxCrossDbQueryParam()
            {
                BasicTableName = ProfitlossTableName,
                SelectColumnInfos = selectColumns,
                StatColumns = statColumns,
                StatGroupByColumns = statGroupByColumns,
                InlodbFilters = inlodbFilters,
                InlodbBakFilters = inlodbBakFilters,
                OrderBy = searchParam.ToOrderByText(),
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize,
                Parameters = new
                {
                    searchParam.SearchUserID,
                    ProfitLossType = searchParam.ProfitLossType.ToNVarchar(50),
                    searchParam.PageNo,
                    searchParam.PageSize,
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                }
            },
            (gridReader) =>
            {
                if (userProfitlossStats == null)
                {
                    userProfitlossStats = gridReader.Read<BasicUserProfitlossStat>().ToList();
                }
                else
                {
                    var tempList = gridReader.Read<BasicUserProfitlossStat>().ToList();

                    foreach (BasicUserProfitlossStat stat in tempList)
                    {
                        BasicUserProfitlossStat sourceStat = userProfitlossStats.SingleOrDefault(w => w.ProfitLossType == stat.ProfitLossType);

                        if (sourceStat == null)
                        {
                            sourceStat = new BasicUserProfitlossStat()
                            {
                                ProfitLossType = stat.ProfitLossType
                            };

                            userProfitlossStats.Add(sourceStat);
                        }

                        sourceStat.TotalProfitLossMoney += stat.TotalProfitLossMoney;
                        sourceStat.TotalWinMoney += stat.TotalWinMoney;
                        sourceStat.TotalPrizeMoney += stat.TotalPrizeMoney;
                    }
                }
            });

            if (userProfitlossStats.AnyAndNotNull())
            {
                foreach (BasicUserProfitlossStat userProfitlossStat in userProfitlossStats)
                {
                    ConvertProfitlossToColumns(userProfitlossStat, statColumn);
                }
            }

            pagedResult.ResultList.ForEach(f =>
            {
                object value = f.GetType().GetProperty(CompareTimeProperty.Value).GetValue(f);

                if (value != null)
                {
                    f.CompareTimeText = Convert.ToDateTime(value).ToFormatDateTimeString();
                }
            });

            return new PagedResultWithAdditionalData<TPGameProfitLossRowModel, ProfitLossStatColumn>(searchParam)
            {
                ResultList = pagedResult.ResultList,
                TotalCount = pagedResult.TotalCount,
                AdditionalData = statColumn
            };
        }

        public PagedResultModel<TPGameMoneyInInfo> GetMoneyInInfoList(SearchTPGameMoneyInfoParam searchParam)
        {
            return SearchMoneyInfoList<TPGameMoneyInInfo>(searchParam, MoneyInInfoTableName);
        }

        public PagedResultModel<TPGameMoneyOutInfo> GetMoneyOutInfoList(SearchTPGameMoneyInfoParam searchParam)
        {
            return SearchMoneyInfoList<TPGameMoneyOutInfo>(searchParam, MoneyOutInfoTableName);
        }

        private PagedResultModel<T> SearchMoneyInfoList<T>(SearchTPGameMoneyInfoParam searchParam, string basicTableName) where T : BaseTPGameMoneyInfo, new()
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.SearchOrderStartDate, searchParam.SearchOrderEndDate);

            string commonFilter = null;

            if (!searchParam.UserName.IsNullOrEmpty())
            {
                commonFilter += $"AND UserID = (SELECT TOP 1 UserID FROM {InlodbType.Inlodb}.dbo.UserInfo WITH(NOLOCK) WHERE UserName = @UserName) ";
            }

            if (searchParam.OrderStatus.HasValue)
            {
                commonFilter += "AND[Status] = @OrderStatus ";
            }

            searchParam.InitSortModelsBySortField();

            if (!searchParam.SortModels.AnyAndNotNull())
            {
                searchParam.SortModels = new List<SortModel>()
                {
                    new SortModel()
                    {
                        ColumnName = nameof(BaseTPGameMoneyInfo.OrderTime),
                        Sort = SortOrder.Descending
                    }
                };
            }

            List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<T>();
            //一律設定成identity,讓底層再建立temp table的時候做轉型
            string moneyIdColumnName = new T().GetPrimaryKeyColumnName();
            selectColumnInfos.Where(w => w.AliasName == moneyIdColumnName).Single().IsIdentity = true;

            return GetCrossDbQueryList<T>(new JxCrossDbQueryParam()
            {
                BasicTableName = basicTableName,
                SelectColumnInfos = selectColumnInfos,
                InlodbFilters = $"OrderTime >= @{nameof(dateRange.InlodbStartDate)} AND OrderTime < @{nameof(dateRange.SmallThanInlodbEndDate)} " + commonFilter,
                InlodbBakFilters = $"OrderTime >= @{nameof(dateRange.InlodbBakStartDate)} AND OrderTime < @{nameof(dateRange.SmallThanInlodbBakEndDate)} " + commonFilter,
                OrderBy = searchParam.ToOrderByText(),
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize,
                Parameters = new
                {
                    UserName = searchParam.UserName.ToNVarchar(50),
                    searchParam.OrderStatus,
                    searchParam.PageNo,
                    searchParam.PageSize,
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                }
            });
        }

        public void ConvertProfitlossToColumns(BasicUserProfitlossStat userProfitlossStat, ProfitLossStatColumn profitlossStatColumn)
        {
            //彩票規則有點不同,故獨立判斷
            if (Product == PlatformProduct.Lottery)
            {
                if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.YJ)
                {
                    profitlossStatColumn.YJProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                    profitlossStatColumn.ZKYProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.FD)
                {
                    profitlossStatColumn.XJFDProfitLossMoney += userProfitlossStat.TotalWinMoney;
                    profitlossStatColumn.FDProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                    profitlossStatColumn.ZKYProfitLossMoney += userProfitlossStat.TotalWinMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.KY)
                {
                    profitlossStatColumn.ZKYProfitLossMoney += userProfitlossStat.TotalWinMoney;
                    profitlossStatColumn.KYProfitLossMoney += userProfitlossStat.TotalPrizeMoney;
                    profitlossStatColumn.TZProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.TX)
                {
                    profitlossStatColumn.TXProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.CZ)
                {
                    profitlossStatColumn.CZProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.HB)
                {
                    profitlossStatColumn.HBProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
            }
            else
            {
                if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.FD)
                {
                    profitlossStatColumn.YJProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                    profitlossStatColumn.XJFDProfitLossMoney += userProfitlossStat.TotalWinMoney;
                    profitlossStatColumn.FDProfitLossMoney += userProfitlossStat.TotalProfitLossMoney + userProfitlossStat.TotalWinMoney;
                    profitlossStatColumn.ZKYProfitLossMoney += userProfitlossStat.TotalWinMoney + userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.KY)
                {
                    profitlossStatColumn.ZKYProfitLossMoney += userProfitlossStat.TotalWinMoney;
                    profitlossStatColumn.KYProfitLossMoney += userProfitlossStat.TotalPrizeMoney;
                    profitlossStatColumn.TZProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.TX)
                {
                    profitlossStatColumn.TXProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
                else if (userProfitlossStat.ProfitLossType == ProfitLossTypeName.CZ)
                {
                    profitlossStatColumn.CZProfitLossMoney += userProfitlossStat.TotalProfitLossMoney;
                }
            }
        }

        protected virtual string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value;
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }

        protected string ConvertToActionCode(bool isDeposit)
        {
            string actionCode = DepositActionCode;

            if (!isDeposit)
            {
                actionCode = WithdrawActionCode;
            }

            return actionCode;
        }

        private PlatformScoreStat GetLastestPlatformScoreStat()
        {
            string sql = $@"
SELECT TOP 1 {DwDailyAvailableScoresColumn} AS SumAvailableScores,
             {DwDailyFreezeScoresColumn} AS SumFreezeScores
FROM         {InlodbType.InlodbBak.Value}.dbo.DW_Daily_AvailableScores WITH(NOLOCK)
ORDER BY     CreateDate DESC ";

            return DbHelper.QuerySingle<PlatformScoreStat>(sql, null);
        }
    }
}
