using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.db;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.Report;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base
{
    public class DbHelperSQL : BaseDbHelperSQL
    {
        protected override Type SqlConnectionType => typeof(SqlConnection);

        public DbHelperSQL(string connectionString) : base(connectionString)
        {
        }

        public PagedResultModel<T> PagedSqlQuery<T>(PagedSqlQueryParamsModel param)
        {
            using (SqlConnection conn = GetSqlConnection() as SqlConnection)
            {
                return BasePagedSqlQuery<T>(conn, param);
            }
        }

        public PagedResultModel<T> MultiplePagedSqlQuery<T>(MultiplePagedSqlQueryParam param) where T : class
        {
            return MultiplePagedSqlQuery<T>(param, null);
        }

        public PagedResultModel<T> MultiplePagedSqlQuery<T>(MultiplePagedSqlQueryParam param, Action<SqlMapper.GridReader> saveStatJob) where T : class
        {
            var pagedResult = new PagedResultModel<T>()
            {
                PageNo = param.PageNo,
                PageSize = param.PageSize,
                ResultList = new List<T>()
            };

            int singleTableFetchRowCount = param.PageNo * param.PageSize;

            //組合union all sql
            var sql = new StringBuilder();

            if (!param.SingleTableQueryParams.AnyAndNotNull())
            {
                return pagedResult;
            }

            //先建立空的 temp table
            SingleTableQueryParam firstTableQueryParam = param.SingleTableQueryParams.First();
            string realTableColumnNames = string.Join(",", firstTableQueryParam.SelectColumnInfos.Select(s => $"{s.FullColumnName} AS {s.AliasName}"));
            string tempTableColumnNames = string.Join(",", firstTableQueryParam.SelectColumnInfos.Select(s => s.AliasName));

            string createTempTableColumnNames = string.Join(",", firstTableQueryParam.SelectColumnInfos.Select(s =>
            {
                string createTempTableColumnName;

                if (s.IsIdentity)
                {
                    createTempTableColumnName = $"CONVERT(VARCHAR(32), {s.FullColumnName})"; //一律轉為VARCHAR(32)
                }
                else
                {
                    createTempTableColumnName = s.FullColumnName;
                }

                createTempTableColumnName += $" AS {s.AliasName}";
                return createTempTableColumnName;
            }));

            sql.AppendLine($@"
                SELECT TOP 0 {createTempTableColumnNames}
                INTO #AllPagedData
                FROM {firstTableQueryParam.FullTableName} WITH(NOLOCK)
                WHERE 1 = 2 ");

            foreach (SingleTableQueryParam singleTableQueryParam in param.SingleTableQueryParams)
            {
                if (singleTableQueryParam.Filters.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("Filters is not allow empty");
                }

                //先取得單表合計
                string statColumns = singleTableQueryParam.StatColumns;
                string statSql;
                int totalCount = 0;

                //這邊要拆分兩種統計, 若沒有GROUP BY 則可以一起統計, 否則就要另拆統計結果的list
                if (singleTableQueryParam.StatGroupByColumns.IsNullOrEmpty())
                {
                    if (!singleTableQueryParam.StatColumns.IsNullOrEmpty())
                    {
                        statColumns = "," + singleTableQueryParam.StatColumns;
                    }

                    statSql = $@"
                        SELECT COUNT(1) AS TotalCount {statColumns}
                        INTO #Stats
                        FROM {singleTableQueryParam.FullTableName} WITH(NOLOCK)
                        WHERE {singleTableQueryParam.Filters};

                        SELECT TOP 1 TotalCount FROM #Stats
                        SELECT * FROM #Stats "; //因為還要拆解別名,故這邊直接使用*號
                }
                else
                {
                    statSql = $@"
                        DECLARE @CNT INT
                        SELECT @CNT = COUNT(1)
                        FROM {singleTableQueryParam.FullTableName} WITH(NOLOCK)
                        WHERE {singleTableQueryParam.Filters};

                        SELECT @CNT AS TotalCount;

                        IF @CNT > 0
                        BEGIN
                            SELECT {statColumns.TrimStart(",")}
                            FROM {singleTableQueryParam.FullTableName} WITH(NOLOCK)
                            WHERE {singleTableQueryParam.Filters}
                            GROUP BY {singleTableQueryParam.StatGroupByColumns};
                        END;";
                }

                QueryMultiple(statSql, param.Parameters,
                    (gridReader) =>
                    {
                        //取得總筆數
                        totalCount = gridReader.ReadSingle<int>();

                        if (totalCount > 0 && saveStatJob != null)
                        {
                            saveStatJob.Invoke(gridReader);
                        }
                    });

                if (totalCount == 0)
                {
                    continue;
                }

                pagedResult.TotalCount += totalCount;
                List<SqlSelectColumnInfo> selectColumns = singleTableQueryParam.SelectColumnInfos;

                sql.AppendLine($"INSERT INTO #AllPagedData ({tempTableColumnNames})");
                sql.AppendLine($"SELECT TOP({singleTableFetchRowCount}) {realTableColumnNames} ");
                sql.AppendLine($"FROM {singleTableQueryParam.FullTableName} WITH(NOLOCK) ");
                sql.AppendLine($"WHERE {singleTableQueryParam.Filters}");
                sql.AppendLine(singleTableQueryParam.OrderBy);
            }

            sql.AppendLine($@"
                SELECT {tempTableColumnNames}
                FROM #AllPagedData
                {firstTableQueryParam.SummaryTempTableOrderBy}
                OFFSET {param.GetComputedOffset()} ROWS FETCH NEXT {param.PageSize} ROWS ONLY ");

            pagedResult.ResultList = QueryList<T>(sql.ToString(), param.Parameters).ToList();

            return pagedResult;
        }

        private PagedResultModel<T> BasePagedSqlQuery<T>(SqlConnection conn, PagedSqlQueryParamsModel param)
        {
            param.Parameters = param.Parameters.ConvertValidDynamicParameters();
            PagedResultModel<T> pagedResultModel = new PagedResultModel<T>();

            string totalCountSql = $@"{param.PreSearchSql}
                SELECT COUNT(1) FROM (SELECT 1 AS SEED FROM {param.SqlBody}) temp ";
            pagedResultModel.TotalCount = Convert.ToInt32(conn.ExecuteScalar(totalCountSql, param.Parameters));

            if (param.MaxSearchRowCount > 0 && pagedResultModel.TotalCount > param.MaxSearchRowCount)
            {
                throw new OverMaxSearchRowCountException();
            }

            pagedResultModel.PageNo = param.PageNo;
            pagedResultModel.PageSize = param.PageSize;

            if (pagedResultModel.TotalCount > 0)
            {
                string pagedSql = $@"{param.PreSearchSql}
                        SELECT {param.SelectColumns} FROM {param.SqlBody} {param.OrderBy}";

                pagedSql += $@" OFFSET {param.GetComputedOffset()} ROWS FETCH NEXT {param.PageSize} ROWS ONLY;";
                pagedResultModel.ResultList = conn.Query<T>(pagedSql, param.Parameters).ToList();
            }

            return pagedResultModel;
        }

        //public PagedResultWithAdditionalData<T, AddtionalDataType> PagedSqlQuery<T, AddtionalDataType>(PagedSqlQueryParamsModel param,
        //    BaseSearchParam queryAdditionalDataParam)
        //{
        //    return PagedSqlQuery<T, AddtionalDataType>(param, queryAdditionalDataParam);
        //}

        public PagedResultWithAdditionalData<T, AddtionalDataType> PagedSqlQuery<T, AddtionalDataType>(PagedSqlQueryParamsModel param,
            BaseSearchParam queryAdditionalDataParam)
        {
            using (SqlConnection conn = GetSqlConnection() as SqlConnection)
            {
                PagedResultModel<T> pagedResultModel = BasePagedSqlQuery<T>(conn, param);

                var returnModel = pagedResultModel.CloneWithoutResult<T, T, AddtionalDataType>();
                returnModel.ResultList = pagedResultModel.ResultList;

                if (returnModel.TotalCount > 0)
                {
                    string pagedSql = $@"{param.PreSearchSql}
                        SELECT {param.SelectColumns} FROM {param.SqlBody} {param.OrderBy}
                        OFFSET {param.GetComputedOffset()} ROWS FETCH NEXT {param.PageSize} ROWS ONLY;";

                    returnModel.ResultList = conn.Query<T>(pagedSql, param.Parameters).ToList();

                    if (queryAdditionalDataParam != null)
                    {
                        returnModel.AdditionalData = conn.QuerySingleOrDefault<AddtionalDataType>(queryAdditionalDataParam.Sql,
                            queryAdditionalDataParam.Parameters);
                    }
                }

                return returnModel;
            }
        }

        public void QueryMultiple(string sql, object param, Action<SqlMapper.GridReader> doActionAfterGetResults)
        {
            QueryMultiple(sql, param, CommandType.Text, doActionAfterGetResults);
        }

        public void QueryMultiple(string sql, object param, CommandType commandType, Action<SqlMapper.GridReader> doActionAfterGetResults)
        {
            param = param.ConvertValidDynamicParameters();

            using (SqlConnection conn = GetSqlConnection() as SqlConnection)
            {
                SqlMapper.GridReader gridReader = conn.QueryMultiple(sql, param, commandType: commandType);
                doActionAfterGetResults.Invoke(gridReader);
            }
        }

        public PagedResultModel<RowType> QueryDistributeList<RowType>(DistributeQueryParam distributeQueryParam, object param,
            Func<List<RowType>, IOrderedEnumerable<RowType>> getOrderedEnumerable)
        {
            PagedResultWithAdditionalData<RowType, BaseStatModel> pagedResultWithAdditionalData = QueryDistributeList<RowType, BaseStatModel>(
                distributeQueryParam,
                param,
                getOrderedEnumerable,
                null);

            return new PagedResultModel<RowType>()
            {
                PageNo = pagedResultWithAdditionalData.PageNo,
                PageSize = pagedResultWithAdditionalData.PageSize,
                ResultList = pagedResultWithAdditionalData.ResultList,
                TotalCount = pagedResultWithAdditionalData.TotalCount
            };
        }

        public PagedResultWithAdditionalData<RowType, StatType> QueryDistributeList<RowType, StatType>(DistributeQueryParam distributeQueryParam, object param,
            Func<List<RowType>, IOrderedEnumerable<RowType>> getOrderedEnumerable, Func<List<StatType>, StatType> getCustomizedStatJob)
            where StatType : BaseStatModel, new()
        {
            var allRowList = new List<RowType>();
            var stats = new List<StatType>();

            if (distributeQueryParam.SqlParams.AnyAndNotNull())
            {
                string allRowSql = string.Join(Environment.NewLine, distributeQueryParam.SqlParams.Select(s => s.RowSql));
                string statSql = string.Join(" UNION ALL \n", distributeQueryParam.SqlParams.Select(s => s.StatSql));

                QueryMultiple(allRowSql, param, (gridReader) =>
                {
                    while (!gridReader.IsConsumed)
                    {
                        List<RowType> readList = gridReader.Read<RowType>().ToList();
                        allRowList.AddRange(readList);
                    }
                });

                stats = QueryList<StatType>(statSql, param);
            }

            StatType finalStat = null;

            if (getCustomizedStatJob != null)
            {
                finalStat = getCustomizedStatJob.Invoke(stats);
            }
            else
            {
                finalStat = new StatType()
                {
                    TotalCount = stats.Sum(s => s.TotalCount)
                };
            }

            IEnumerable<RowType> orderedEnumerable;

            if (getOrderedEnumerable != null)
            {
                orderedEnumerable = getOrderedEnumerable.Invoke(allRowList);
            }
            else
            {
                orderedEnumerable = allRowList;
            }

            List<RowType> resultList = orderedEnumerable.Skip(distributeQueryParam.GetComputedOffset())
                .Take(distributeQueryParam.PageSize)
                .ToList();

            var pagedResultModel = new PagedResultWithAdditionalData<RowType, StatType>()
            {
                PageNo = distributeQueryParam.PageNo,
                PageSize = distributeQueryParam.PageSize,
                ResultList = resultList,
                TotalCount = finalStat.TotalCount,
                AdditionalData = finalStat
            };

            return pagedResultModel;
        }
    }

    public class DistributeQueryParam : BasePagedParamsModel
    {
        public List<DistributeSqlParam> SqlParams { get; set; } = new List<DistributeSqlParam>();
    }

    public class DistributeSqlParam
    {
        public string RowSql { get; set; }

        public string StatSql { get; set; }
    }
}