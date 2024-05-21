using Dapper;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.TransferRecord;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseCMoneyInfoRep<T> : BaseDbRepository<T>, IBaseCMoneyInfoRep<T> where T : class
    {
        private static readonly int _minRecheckOrderMinutes = -1;

        protected abstract string SequenceName { get; }

        protected abstract int ProcessingStatusValue { get; }

        public BaseCMoneyInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public string CreateMoneyID() => GetSequenceIdentity(SequenceName);

        public List<T> GetProcessingOrders3DaysAgo()
        {
            string sql = $@"
                -- 撈取設定分鐘前為處理中的訂單
                DECLARE @ProcessingDate DATETIME = DATEADD(MINUTE, {_minRecheckOrderMinutes}, GETDATE())
                DECLARE	@StartDate DATETIME = DATEADD(DAY, -3, @ProcessingDate),
		                @EndDate DATETIME =  @ProcessingDate

                {GetAllQuerySQL(InlodbType.Inlodb)}
                WHERE
                    DealType = @ProcessingStatus AND
                    OrderTime >= @StartDate AND
                    OrderTime <= @EndDate ";

            return DbHelper.QueryList<T>(sql, new
            {
                ProcessingStatus = ProcessingStatusValue
            });
        }

        public PagedResultModel<TransferRecordViewModel> GetPlatformTransferRecord(QueryPlatformTransferRecordParam param)
        {
            param.PageNo = 1;
            param.PageSize = int.MaxValue;

            var distributeQueryParam = new DistributeQueryParam()
            {
                PageNo = param.PageNo,
                PageSize = param.PageSize
            };

            TableSearchDateRange dateRange = GetTableSearchDateRange(param.StartDate, param.QueryEndDate);

            distributeQueryParam.SqlParams.AddRange(CreateTransferRecordSqlParam(dateRange,
                (inlodbType, isQueryStat) => GetQueryTransferRecordSQL(inlodbType, param, isQueryStat)));

            DbString productCode = null;

            if (param.PlatformProduct != null)
            {
                productCode = param.PlatformProduct.Value.ToVarchar(10);
            }

            PagedResultModel<TransferRecordViewModel> pagedResultModel = DbHelper.QueryDistributeList<TransferRecordViewModel>(
                distributeQueryParam,
                new
                {
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                    param.UserID,
                    param.DealType,
                    ProductCode = productCode,
                },
                (rows) => rows
                    .OrderByDescending(t => t.OrderTime)
                    .ThenByDescending(t => t.OrderID)
                );

            return pagedResultModel;
        }

        private List<DistributeSqlParam> CreateTransferRecordSqlParam(
            TableSearchDateRange dateRange, Func<InlodbType, bool, string> GetMoneyInInfosSql)
        {
            var distributeSqlParams = new List<DistributeSqlParam>();

            Action<InlodbType> addDistributeSqlParamsJob = (inlodbType) =>
            {
                bool isQueryStat = false;
                string rowSql = GetMoneyInInfosSql.Invoke(inlodbType, isQueryStat);

                isQueryStat = true;
                string statSql = GetMoneyInInfosSql.Invoke(inlodbType, isQueryStat);

                distributeSqlParams.Add(new DistributeSqlParam()
                {
                    RowSql = rowSql,
                    StatSql = statSql
                });
            };

            if (dateRange.InlodbStartDate.HasValue && dateRange.SmallThanInlodbEndDate.HasValue)
            {
                addDistributeSqlParamsJob.Invoke(InlodbType.Inlodb);
            }

            if (dateRange.InlodbBakStartDate.HasValue && dateRange.SmallThanInlodbBakEndDate.HasValue)
            {
                addDistributeSqlParamsJob.Invoke(InlodbType.InlodbBak);
            }

            return distributeSqlParams;
        }

        private string GetQueryTransferRecordSQL(InlodbType inlodbType, QueryPlatformTransferRecordParam param, bool isQueryStat)
        {
            string startDateParamName = null;
            string endDateParamName = null;

            if (inlodbType == InlodbType.Inlodb)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbEndDate);
            }
            else if (inlodbType == InlodbType.InlodbBak)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbBakStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbBakEndDate);
            }

            string columns;

            if (!isQueryStat)
            {
                columns = $@"
                    TOP {param.PageNo * param.PageSize}
                        OrderID,
                        UserID,
                        Amount,
                        OrderTime,
                        UpdateDate AS HandTime,
                        DealType AS Status,
                        Memo ";
            }
            else
            {
                columns = " COUNT(1) AS TotalCount";
            }

            string sql = $@"
                SELECT {columns}
			    FROM {inlodbType.Value}.dbo.{ModelUtil.GetTableName(typeof(T))} WITH(NOLOCK)
                WHERE
                    OrderTime >= @{startDateParamName}
	                AND OrderTime < @{endDateParamName}
                ";

            if (param.UserID.HasValue)
            {
                sql += " AND UserId = @UserId ";
            }

            if (param.DealType.HasValue)
            {
                sql += " AND DealType = @DealType ";
            }

            if (param.PlatformProduct != null)
            {
                sql += " AND ProductCode = @ProductCode ";
            }

            if (!isQueryStat)
            {
                sql += " ORDER BY OrderTime DESC, OrderID DESC ";
            }

            return sql;
        }
    }
}