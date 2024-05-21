using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class AllTPGameStatRep : BaseDbRepository, IAllTPGameStatRep
    {
        public AllTPGameStatRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<TotalUserScore> GetTotalUserScores(List<PlatformProduct> nonSelfPlatformProducts, int userId)
        {
            var sql = new StringBuilder();

            sql.AppendLine($@"
                DROP TABLE IF EXISTS #UserProductScore
                DROP TABLE IF EXISTS #AllUserIDs

                CREATE TABLE #UserProductScore(
	                UserID INT NOT NULL,
	                ProductCode VARCHAR(10) NOT NULL,
	                AvailableScores DECIMAL(18, 4) NOT NULL,
	                FreezeScores DECIMAL(18, 4) NOT NULL)

                INSERT INTO #UserProductScore(
	                UserID,
	                ProductCode,
	                AvailableScores,
	                FreezeScores)
                SELECT
	                UserID,
	                '{PlatformProduct.Lottery}',
	                ISNULL(AvailableScores, 0),
	                ISNULL(FreezeScores, 0)
                FROM UserInfo WITH(NOLOCK)
                WHERE
	                UserID = @UserID

                SELECT
	                UserID
                INTO #AllUserIDs
                FROM #UserProductScore --主帳戶一定會有完整UserID ");

            foreach (PlatformProduct nonSelfPlatformProduct in nonSelfPlatformProducts)
            {
                ITPGameUserInfoService userInfoService = GetTPGameUserInfoService(nonSelfPlatformProduct);

                List<string> availableScoreColumnNames = userInfoService.AllAvailableScoresColumnNames;

                sql.AppendLine($@"
                    INSERT INTO #UserProductScore(
	                    UserID,
	                    ProductCode,
	                    AvailableScores,
	                    FreezeScores)
                    SELECT
                        UserID,
                        '{nonSelfPlatformProduct.Value}' AS ProductCode,
                        {string.Join(" + ", availableScoreColumnNames.Select(s => $"ISNULL({s}, 0)"))},
	                    ISNULL(FreezeScores, 0)
                    FROM {InlodbType.Inlodb}.dbo.{ModelUtil.GetTableName(userInfoService.GetUserInfoType())} WITH(NOLOCK)
                    WHERE UserID IN(SELECT UserID FROM #AllUserIDs)
                ");
            }

            sql.AppendLine(@"
                SELECT
                    UserID,
                    SUM(AvailableScores) AS TotalAvailableScores,
                    SUM(FreezeScores) AS TotalFreezeScores
                FROM #UserProductScore
                GROUP BY UserID ");

            return DbHelper.QueryList<TotalUserScore>(sql.ToString(), new { userId });
        }

        /// <summary>
        /// 瀑布式/分頁式載入的全遊戲投注資料
        /// </summary>
        public PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> GetPagedAllPlayInfo(SearchAllPagedPlayInfoParam searchParam)
        {
            int fetchCount = searchParam.PageSize;
            fetchCount *= searchParam.PageNo; //因為會從每張表符合條件的第一筆往後取

            var sql = new StringBuilder();

            sql.AppendLine($@"
                CREATE TABLE #PlayInfoRow(
                    UserID INT,
                    ProductCode VARCHAR(10),
                    ProductName NVARCHAR(50),
                    PlayInfoID VARCHAR(32),
                    BetTime DATETIME,
                    SaveTime DATETIME,
                    ProfitLossTime DATETIME,
                    BetMoney DECIMAL(18, 4),
                    AllBetMoney DECIMAL(18, 4),
                    WinMoney DECIMAL(18, 4),
                    GameType NVARCHAR(50),
                    Memo NVARCHAR(500),
                    PlayID NVARCHAR(50),
                    BetResultType INT)

                CREATE TABLE #PlayInfoStat(
                    ProductCode VARCHAR(10),
                    TotalCount INT,
                    TotalBetMoney DECIMAL(18, 4),
                    TotalAllBetMoney DECIMAL(18, 4),
                    TotalWinMoney DECIMAL(18, 4))");

            TableSearchDateRange tableSearchDateRange = GetTableSearchDateRange(searchParam.QueryStartDate, searchParam.QueryEndDate);

            #region 要塞每個產品的資料到TEMP

            foreach (PlatformProduct platformProduct in searchParam.PlatformProducts)
            {
                var tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                    platformProduct,
                    SharedAppSettings.PlatformMerchant,
                    EnvLoginUser,
                    DBConnectionType).Value;

                Dictionary<string, SqlSelectColumnInfo> aliasColumnMap = tpGameStoredProcedureRep.PlayInfoSelectColumnInfos.ToDictionary(d => d.AliasName);

                //一定會有的欄位
                string betMoneyColumnName = aliasColumnMap[nameof(TPGamePlayInfoRowModel.BetMoney)].ColumnName;
                string winMoneyColumnName = aliasColumnMap[nameof(TPGamePlayInfoRowModel.WinMoney)].ColumnName;
                //string betTimeColumnName = aliasColumnMap[nameof(TPGamePlayInfoRowModel.BetTime)].ColumnName;
                string playIdColumnName = aliasColumnMap[nameof(TPGamePlayInfoRowModel.PlayID)].ColumnName;
                string saveTimeColumnName = aliasColumnMap[nameof(TPGamePlayInfoRowModel.SaveTime)].ColumnName;

                //不一定會有的欄位
                string memoColumnName = GetColumnNameByAlias(aliasColumnMap, nameof(TPGamePlayInfoRowModel.Memo), "NULL");
                string gameTypeColumnName = GetColumnNameByAlias(aliasColumnMap, nameof(TPGamePlayInfoRowModel.GameType), "NULL");
                string allBetMoneyColumnName = GetColumnNameByAlias(aliasColumnMap, nameof(TPGamePlayInfoRowModel.AllBetMoney), betMoneyColumnName);
                string playInfoIdColumnName = GetColumnNameByAlias(aliasColumnMap, nameof(TPGamePlayInfoRowModel.PlayInfoID), playIdColumnName);

                string tableName = tpGameStoredProcedureRep.PlayInfoTableName;
                //string commonFilters = "UserID = @userId ";
                string commonFilters = "1 = 1 ";
                string lastKeyFilter = null;
                string gameCodeFilter = null;

                if (!searchParam.GameCode.IsNullOrEmpty())
                {
                    gameCodeFilter = $@"
                        AND {gameTypeColumnName} = @GameCode ";
                }

                //查詢inlodb的資料
                if (tableSearchDateRange.InlodbStartDate.HasValue && tableSearchDateRange.SmallThanInlodbEndDate.HasValue)
                {
                    string inlodbStatFilters = commonFilters + $"AND {saveTimeColumnName} >= @InlodbStartDate " +
                        $"AND {saveTimeColumnName} < @SmallThanInlodbEndDate " + gameCodeFilter;

                    string inlodbRowFilters = inlodbStatFilters + lastKeyFilter;

                    sql.AppendLine(GetSelectPlayInfoSql(
                        platformProduct,
                        $"{InlodbType.Inlodb}.dbo.{tableName}",
                        aliasColumnMap,
                        fetchCount,
                        inlodbStatFilters,
                        inlodbRowFilters));
                }

                //查詢inlodb_bak的資料
                if (tableSearchDateRange.InlodbBakStartDate.HasValue && tableSearchDateRange.SmallThanInlodbBakEndDate.HasValue)
                {
                    string inlodbBakStatFilters = commonFilters + $"AND {saveTimeColumnName} >= @InlodbBakStartDate " +
                        $"AND {saveTimeColumnName} < @SmallThanInlodbBakEndDate " + gameCodeFilter;

                    string inlodbBakRowFilters = inlodbBakStatFilters + lastKeyFilter;

                    sql.AppendLine(GetSelectPlayInfoSql(
                        platformProduct,
                        $"{InlodbType.InlodbBak}.dbo.{tableName}",
                        aliasColumnMap,
                        fetchCount,
                        inlodbBakStatFilters,
                        inlodbBakRowFilters));
                }
            }

            #endregion 要塞每個產品的資料到TEMP

            sql.AppendLine(@"
                SELECT
                    SUM(TotalCount)
                FROM #PlayInfoStat

                SELECT
                    UserID,
                    ProductCode,
                    PlayInfoID,
                    BetTime,
                    ProfitLossTime,
                    BetMoney,
                    AllBetMoney,
                    WinMoney,
                    GameType,
                    Memo,
                    PlayID,
                    BetResultType
                FROM #PlayInfoRow
                ORDER BY SaveTime DESC, PlayInfoID DESC, ProductCode

                SELECT
                    SUM(TotalBetMoney) AS TotalBetMoney,
                    SUM(TotalAllBetMoney) AS TotalAllBetMoney,
                    SUM(TotalWinMoney) AS TotalWinMoney
                FROM #PlayInfoStat
            ");

            object sqlParam = new
            {
                //searchParam.UserID,
                tableSearchDateRange.InlodbStartDate,
                tableSearchDateRange.SmallThanInlodbEndDate,
                tableSearchDateRange.InlodbBakStartDate,
                tableSearchDateRange.SmallThanInlodbBakEndDate,
                GameCode = searchParam.GameCode.ToNVarchar(200),
            };

            var pagedResult = new PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel>()
            {
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize
            };

            DbHelper.QueryMultiple(sql.ToString(), sqlParam, (gridReader) =>
            {
                pagedResult.TotalCount = gridReader.ReadSingle<int>();
                pagedResult.ResultList = gridReader.Read<AllGamePlayInfoRowModel>().ToList();
                pagedResult.AdditionalData = gridReader.ReadSingle<TPGamePlayInfoStatModel>();
            });

            List<AllGamePlayInfoRowModel> resultList = pagedResult.ResultList;

            pagedResult.ResultList = resultList
                .Skip((searchParam.PageNo - 1) * searchParam.PageSize)
                .Take(searchParam.PageSize)
                .ToList();

            return pagedResult;
        }

        private string GetSelectPlayInfoSql(PlatformProduct platformProduct, string fullTableName, Dictionary<string, SqlSelectColumnInfo> columnInfoMap,
            int fetchCount, string statFilters, string rowFilters)
        {
            //一定會有的欄位
            string betMoneyColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.BetMoney)].ColumnName;
            string winMoneyColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.WinMoney)].ColumnName;
            string betTimeColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.BetTime)].ColumnName;
            string profitLossTimeColumnName = columnInfoMap[nameof(TPGamePlayInfoRowModel.ProfitLossTime)].ColumnName;
            string playIdColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.PlayID)].ColumnName;
            string betResultTypeColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.BetResultType)].ColumnName;
            string userIdColumnName = columnInfoMap[nameof(AllGamePlayInfoRowModel.UserId)].ColumnName;

            //不一定會有的欄位
            string memoColumnName = GetColumnNameByAlias(columnInfoMap, nameof(AllGamePlayInfoRowModel.Memo), "NULL");
            string gameTypeColumnName = GetColumnNameByAlias(columnInfoMap, nameof(AllGamePlayInfoRowModel.GameType), "NULL");
            string allBetMoneyColumnName = GetColumnNameByAlias(columnInfoMap, nameof(AllGamePlayInfoRowModel.AllBetMoney), betMoneyColumnName);
            string playInfoIdColumnName = GetColumnNameByAlias(columnInfoMap, nameof(AllGamePlayInfoRowModel.PlayInfoID), playIdColumnName);

            var sql = new StringBuilder();

            sql.AppendLine($@"
                INSERT INTO #PlayInfoStat(
                    ProductCode,
                    TotalCount,
                    TotalBetMoney,
                    TotalAllBetMoney,
                    TotalWinMoney)
                SELECT
                    '{platformProduct.Value}' AS ProductCode,
                    COUNT(1) AS TotalCount,
                    SUM({betMoneyColumnName}) AS TotalBetMoney,
                    SUM({allBetMoneyColumnName}) AS TotalAllBetMoney,
                    SUM({winMoneyColumnName}) AS TotalWinMoney
                FROM {fullTableName} WITH(NOLOCK)
                WHERE
                    {statFilters} ");

            //有筆數資料才做明細資料查詢
            sql.AppendLine($@"
                IF EXISTS(SELECT TOP 1 1 FROM #PlayInfoStat WHERE ProductCode = '{platformProduct.Value}' AND TotalCount > 0)
                BEGIN
                    INSERT INTO #PlayInfoRow(
                        UserID,
                        ProductCode,
                        PlayInfoID,
                        BetTime,
                        ProfitLossTime,
                        BetMoney,
                        AllBetMoney,
                        WinMoney,
                        GameType,
                        Memo,
                        PlayID,
                        BetResultType)
                    SELECT TOP({fetchCount})
                        {userIdColumnName} AS UserID,
                        '{platformProduct.Value}' AS ProductCode,
                        {playInfoIdColumnName} AS PlayInfoID,
                        {betTimeColumnName} AS BetTime,
                        {profitLossTimeColumnName} AS ProfitLossTime,
                        {betMoneyColumnName} AS BetMoney,
                        {allBetMoneyColumnName} AS AllBetMoney,
                        {winMoneyColumnName} AS WinMoney,
                        {gameTypeColumnName} AS GameType,
                        {memoColumnName} AS Memo,
                        {playIdColumnName} AS PlayID,
                        {betResultTypeColumnName} AS BetResultType
                    FROM {fullTableName} WITH(NOLOCK)
                    WHERE
                        {rowFilters}
                    ORDER BY SaveTime DESC
                END;");

            return sql.ToString();
        }

        private string GetColumnNameByAlias(Dictionary<string, SqlSelectColumnInfo> columnInfoMap, string aliasName, string defaultColumnName)
        {
            if (columnInfoMap.TryGetValue(aliasName, out SqlSelectColumnInfo sqlSelectColumnInfo))
            {
                return sqlSelectColumnInfo.ColumnName;
            }

            return defaultColumnName;
        }

        private ITPGameUserInfoService GetTPGameUserInfoService(PlatformProduct product)
        {
            ITPGameUserInfoService tpGameUserInfoService = DependencyUtil
                .ResolveJxBackendService<ITPGameUserInfoService>(product, SharedAppSettings.PlatformMerchant, EnvLoginUser, DBConnectionType).Value;

            return tpGameUserInfoService;
        }
    }
}