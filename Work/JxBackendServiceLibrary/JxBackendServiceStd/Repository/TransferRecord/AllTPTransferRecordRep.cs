using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.TransferRecord;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.TransferRecord;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.TransferRecord
{
    public class AllTPTransferRecordRep : BaseDbRepository, IAllTPTransferRecordRep
    {
        public AllTPTransferRecordRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>
        /// 全遊戲轉賬記錄資料
        /// </summary>
        public PagedResultModel<TransferRecordViewModel> GetAllTPTransferRecord(
            QueryTPTransferRecordParam param, List<SearchTransferType> searchTransferTypes, string platformName)
        {
            var pagedResult = new PagedResultModel<TransferRecordViewModel>()
            {
                PageNo = param.PageNo,
                PageSize = param.PageSize
            };

            if (!param.UserID.HasValue && param.ProductCode.IsNullOrEmpty())
            {
                //需输入用户ID或指定产品查询
                return pagedResult;
            }

            int fetchCount = param.PageNo * param.PageSize;

            var sql = new StringBuilder();

            sql.AppendLine($@"
                CREATE TABLE #TransferRecordRow(
                    ProductCode NVARCHAR(10),
                    TransferSource NVARCHAR(10),
                    TransferTarget NVARCHAR(10),
                    TransferType NVARCHAR(5),
                    OrderID NVARCHAR(50),
                    UserID INT,
                    Amount DECIMAL(18, 4),
                    OrderTime DATETIME,
                    HandTime DATETIME,
                    Status INT,
                    Memo NVARCHAR(1024))

                CREATE TABLE #TransferRecordStat(
                    TableName NVARCHAR(50),
                    TotalCount INT)
            ");

            TableSearchDateRange tableSearchDateRange = GetTableSearchDateRange(param.StartDate, param.QueryEndDate);

            foreach (PlatformProduct product in param.PlatformProducts)
            {
                foreach (SearchTransferType transferType in searchTransferTypes)
                {
                    string commonFilters = GetWhereString(param.UserID, param.OrderStatus);

                    //查詢inlodb的資料
                    if (tableSearchDateRange.InlodbStartDate.HasValue && tableSearchDateRange.SmallThanInlodbEndDate.HasValue)
                    {
                        string inlodbFilters = commonFilters +
                            $"AND OrderTime >= @InlodbStartDate " +
                            $"AND OrderTime < @SmallThanInlodbEndDate ";

                        sql.AppendLine(GetSelectTransferRecordSql(
                            product,
                            transferType,
                            InlodbType.Inlodb,
                            fetchCount,
                            inlodbFilters,
                            platformName));
                    }

                    //查詢inlodb_bak的資料
                    if (tableSearchDateRange.InlodbBakStartDate.HasValue && tableSearchDateRange.SmallThanInlodbBakEndDate.HasValue)
                    {
                        string inlodbBakStatFilters = commonFilters +
                            $"AND OrderTime >= @InlodbBakStartDate " +
                            $"AND OrderTime < @SmallThanInlodbBakEndDate ";

                        sql.AppendLine(GetSelectTransferRecordSql(
                            product,
                            transferType,
                            InlodbType.InlodbBak,
                            fetchCount,
                            inlodbBakStatFilters,
                            platformName));
                    }
                }
            }

            sql.AppendLine($@"
                SELECT
                    SUM(TotalCount)
                FROM #TransferRecordStat

                SELECT
                    TransferSource,
                    TransferTarget,
                    TransferType,
                    OrderID,
                    UserID,
                    Amount,
                    OrderTime,
                    HandTime,
                    Status,
                    Memo
                FROM #TransferRecordRow
                ORDER BY OrderTime DESC, OrderID DESC
            ");

            object sqlParam = new
            {
                tableSearchDateRange.InlodbStartDate,
                tableSearchDateRange.SmallThanInlodbEndDate,
                tableSearchDateRange.InlodbBakStartDate,
                tableSearchDateRange.SmallThanInlodbBakEndDate,
                param.UserID,
                param.OrderStatus,
            };

            DbHelper.QueryMultiple(sql.ToString(), sqlParam, (gridReader) =>
            {
                pagedResult.TotalCount = gridReader.ReadSingle<int>();
                pagedResult.ResultList = gridReader.Read<TransferRecordViewModel>().ToList();
            });

            return pagedResult;
        }

        private string GetWhereString(int? userId, short? orderStatus)
        {
            var whereString = new StringBuilder(" WHERE 1 = 1 ");

            if (userId.HasValue)
            {
                whereString.Append(" AND UserId = @UserId ");
            }

            if (orderStatus.HasValue)
            {
                whereString.Append(" AND Status = @OrderStatus ");
            }

            return whereString.ToString();
        }

        private string GetSelectTransferRecordSql(PlatformProduct product, SearchTransferType transferType,
            InlodbType inlodbType, int fetchCount, string whereString, string platformName)
        {
            string tableName = null;
            string transferSource = null;
            string transferTarget = null;
            var tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DBConnectionType);

            if (transferType == SearchTransferType.In)
            {
                tableName = tpGameStoredProcedureRep.MoneyInInfoTableName;
                transferSource = platformName;
                transferTarget = product.Name;
            }
            else if (transferType == SearchTransferType.Out)
            {
                tableName = tpGameStoredProcedureRep.MoneyOutInfoTableName;
                transferSource = product.Name;
                transferTarget = platformName;
            }
            else
            {

            }

            string fullTableName = $"{inlodbType}.dbo.{tableName}";

            return $@"
                INSERT INTO #TransferRecordStat(
                    TableName,
                    TotalCount)
                SELECT
                    '{fullTableName}' AS TableName,
                    COUNT(1) AS TotalCount
                FROM {fullTableName} WITH(NOLOCK)
                {whereString}                 

                IF EXISTS(SELECT TOP 1 1 FROM #TransferRecordStat WHERE TableName = '{fullTableName}' AND TotalCount > 0)
                BEGIN
                    INSERT INTO #TransferRecordRow(
                        ProductCode,
                        TransferSource,
                        TransferTarget,
                        TransferType,
                        OrderID,
                        UserID,
                        Amount,
                        OrderTime,
                        HandTime,
                        Status,
                        Memo)
                    SELECT TOP({fetchCount})
                        '{product.Value}' AS ProductCode,
                        '{transferSource}' AS TransferSource,
                        '{transferTarget}' AS TransferTarget,
                        '{transferType.Value}' AS TransferType,
                        OrderID,
                        UserID,
                        Amount,
                        OrderTime,
                        HandTime,
                        Status,
                        Memo
                    FROM {fullTableName} WITH(NOLOCK)
                    {whereString}                    
                    ORDER BY OrderTime DESC, OrderID DESC
                END
            ";
        }
    }
}