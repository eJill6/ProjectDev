using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.VIP
{
    public class VIPUserChangeLogRep : BaseDbRepository, IVIPUserChangeLogRep
    {
        public VIPUserChangeLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
        }

        /// <summary><see cref="IVIPUserChangeLogRep.GetVIPPointsChangeLogs"/></summary>
        public PagedResultModel<VIPPointsChangeLogModel> GetVIPPointsChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.StartDate, searchParam.EndDate);

            var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                EnvLoginUser.Application,
                SharedAppSettings.PlatformMerchant);

            var distributeQueryParam = new DistributeQueryParam()
            {
                PageNo = searchParam.PageNum,
                PageSize = searchParam.PageSize
            };

            distributeQueryParam.SqlParams.AddRange(
                CreateDistributeSqlParam(VIPChangeLogTypes.Points, dateRange, searchParam.PageNum, searchParam.PageSize, "VIPPointsAccountLog"));

            foreach (PlatformProduct product in platformProductService.GetAll())
            {
                distributeQueryParam.SqlParams.AddRange(
                CreateDistributeSqlParam(VIPChangeLogTypes.Points, dateRange, searchParam.PageNum, searchParam.PageSize, product));
            }

            object param = new
            {
                searchParam.UserID,
                dateRange.InlodbStartDate,
                dateRange.SmallThanInlodbEndDate,
                dateRange.InlodbBakStartDate,
                dateRange.SmallThanInlodbBakEndDate,
            };

            PagedResultModel<VIPPointsChangeLogModel> pagedResultModel = DbHelper.QueryDistributeList<VIPPointsChangeLogModel>(
               distributeQueryParam,
               param,
               (rows) =>
               {
                   return rows.OrderByDescending(o => o.CreateDate).ThenBy(t => t.DataSourceCode).ThenByDescending(t => t.SEQID);
               });

            return pagedResultModel;
        }

        /// <summary><see cref="IVIPUserChangeLogRep.GetVIPFlowChangeLogs"/></summary>
        public PagedResultModel<VIPFlowChangeLogModel> GetVIPFlowChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.StartDate, searchParam.EndDate);

            var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                EnvLoginUser.Application,
                SharedAppSettings.PlatformMerchant);

            var distributeQueryParam = new DistributeQueryParam()
            {
                PageNo = searchParam.PageNum,
                PageSize = searchParam.PageSize
            };

            distributeQueryParam.SqlParams.AddRange(
                CreateDistributeSqlParam(VIPChangeLogTypes.Flow, dateRange, searchParam.PageNum, searchParam.PageSize, "VIPFlowAccountLog"));

            foreach (PlatformProduct product in platformProductService.GetAll())
            {
                distributeQueryParam.SqlParams.AddRange(
                CreateDistributeSqlParam(VIPChangeLogTypes.Flow, dateRange, searchParam.PageNum, searchParam.PageSize, product));
            }

            object param = new
            {
                searchParam.UserID,
                dateRange.InlodbStartDate,
                dateRange.SmallThanInlodbEndDate,
                dateRange.InlodbBakStartDate,
                dateRange.SmallThanInlodbBakEndDate,
            };

            PagedResultModel<VIPFlowChangeLogModel> pagedResultModel = DbHelper.QueryDistributeList<VIPFlowChangeLogModel>(
               distributeQueryParam,
               param,
               (rows) =>
               {
                   return rows.OrderByDescending(o => o.CreateDate).ThenBy(t => t.DataSourceCode).ThenByDescending(t => t.SEQID);
               });

            return pagedResultModel;
        }

        /// <summary><see cref="IVIPUserChangeLogRep.GetVIPAgentScoreChangeLogs"/></summary>
        public PagedResultModel<VIPAgentAccountLogModel> GetVIPAgentScoreChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.StartDate, searchParam.EndDate);

            var distributeQueryParam = new DistributeQueryParam()
            {
                PageNo = searchParam.PageNum,
                PageSize = searchParam.PageSize
            };

            distributeQueryParam.SqlParams.AddRange(
                CreateDistributeSqlParam(VIPChangeLogTypes.AgentScore, dateRange, searchParam.PageNum, searchParam.PageSize, "VIPAgentAccountLog"));

            object param = new
            {
                searchParam.UserID,
                dateRange.InlodbStartDate,
                dateRange.SmallThanInlodbEndDate,
                dateRange.InlodbBakStartDate,
                dateRange.SmallThanInlodbBakEndDate,
            };

            PagedResultModel<VIPAgentAccountLogModel> pagedResultModel = DbHelper.QueryDistributeList<VIPAgentAccountLogModel>(
               distributeQueryParam,
               param,
               (rows) =>
               {
                   return rows.OrderByDescending(o => o.CreateDate).ThenBy(t => t.DataSourceCode).ThenByDescending(t => t.SEQID);
               });

            return pagedResultModel;
        }

        private List<DistributeSqlParam> CreateDistributeSqlParam(VIPChangeLogTypes vipChangeLogType,
            TableSearchDateRange dateRange, int pageNo, int pageSize, string tableName)
        {
            return CreateDistributeSqlParam(vipChangeLogType, dateRange, pageNo, pageSize, tableName, tableName, tableName, null);
        }

        private List<DistributeSqlParam> CreateDistributeSqlParam(VIPChangeLogTypes vipChangeLogType,
            TableSearchDateRange dateRange, int pageNo, int pageSize, PlatformProduct product)
        {
            var tpGameStoredProcedureRep = DependencyUtil.ResolveJxBackendService<ITPGameStoredProcedureRep>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DbConnectionTypes.Slave);

            //後續擴充規模不大的關係, 這邊不拆成工廠注入
            switch (vipChangeLogType)
            {
                case VIPChangeLogTypes.Points:
                    return CreateDistributeSqlParam(
                        vipChangeLogType,
                        dateRange,
                        pageNo,
                        pageSize,
                        product.Value,
                        product.Name,
                        tpGameStoredProcedureRep.VIPPointsProductTableName,
                        "BetMoney");
                case VIPChangeLogTypes.Flow:
                    return CreateDistributeSqlParam(
                        vipChangeLogType,
                        dateRange,
                        pageNo,
                        pageSize,
                        product.Value,
                        product.Name,
                        tpGameStoredProcedureRep.VIPFlowProductTableName,
                        null);
            }

            throw new ArgumentOutOfRangeException();
        }

        private List<DistributeSqlParam> CreateDistributeSqlParam(VIPChangeLogTypes vipChangeLogType,
            TableSearchDateRange dateRange, int pageNo, int pageSize,
            string dataSourceCode, string dataSourceName, string tableName, string addtionalColumns)
        {
            var distributeSqlParams = new List<DistributeSqlParam>();
            var buildVIPChangeLogSqlParam = new BuildVIPChangeLogSqlParam()
            {
                DataSourceCode = dataSourceCode,
                DataSourceName = dataSourceName,                
                PageNo = pageNo,
                PageSize = pageSize,
                TableName = tableName,
                AddtionalColumns = addtionalColumns
            };

            if (dateRange.InlodbStartDate.HasValue && dateRange.SmallThanInlodbEndDate.HasValue)
            {
                buildVIPChangeLogSqlParam.IsQueryCount = false;
                buildVIPChangeLogSqlParam.InlodbType = InlodbType.Inlodb;
                string rowSql = GetSingleVIPLogSQLByFactory(vipChangeLogType, buildVIPChangeLogSqlParam);
                buildVIPChangeLogSqlParam.IsQueryCount = true;
                string countSql = GetSingleVIPLogSQLByFactory(vipChangeLogType, buildVIPChangeLogSqlParam);

                distributeSqlParams.Add(new DistributeSqlParam()
                {
                    RowSql = rowSql,
                    CountSql = countSql
                });
            }

            if (dateRange.InlodbBakStartDate.HasValue && dateRange.SmallThanInlodbBakEndDate.HasValue)
            {
                buildVIPChangeLogSqlParam.IsQueryCount = false;
                buildVIPChangeLogSqlParam.InlodbType = InlodbType.InlodbBak;
                string rowSql = GetSingleVIPLogSQLByFactory(vipChangeLogType, buildVIPChangeLogSqlParam);
                buildVIPChangeLogSqlParam.IsQueryCount = true;
                string countSql = GetSingleVIPLogSQLByFactory(vipChangeLogType, buildVIPChangeLogSqlParam);

                distributeSqlParams.Add(new DistributeSqlParam()
                {
                    RowSql = rowSql,
                    CountSql = countSql
                });
            }

            return distributeSqlParams;
        }

        #region 產生sql語法差異化的private method
        private string GetSingleVIPLogSQLByFactory(VIPChangeLogTypes vipChangeLogType, BuildVIPChangeLogSqlParam param)
        {
            switch (vipChangeLogType)
            {
                case VIPChangeLogTypes.Points:
                    return GetSingleVIPPointsLogSQL(param);
                case VIPChangeLogTypes.Flow:
                    return GetSingleVIPFlowLogSQL(param);
                case VIPChangeLogTypes.AgentScore:
                    return GetSingleVIPAgentAccountLogSQL(param);                    
            }

            throw new ArgumentOutOfRangeException();
        }

        private string GetSingleVIPPointsLogSQL(BuildVIPChangeLogSqlParam param)
        {
            string startDateParamName = null;
            string endDateParamName = null;

            if (param.InlodbType == InlodbType.Inlodb)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbEndDate);
            }
            else if (param.InlodbType == InlodbType.InlodbBak)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbBakStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbBakEndDate);
            }

            string columns = "COUNT(*) AS CNT ";

            if (!param.IsQueryCount)
            {
                string addtionalColumns = null;

                if (!param.AddtionalColumns.IsNullOrEmpty())
                {
                    addtionalColumns = param.AddtionalColumns + ",";
                }

                columns = $@"TOP {param.PageNo * param.PageSize}
                    N'{param.DataSourceCode}' AS {nameof(param.DataSourceCode)},
                    N'{param.DataSourceName}' AS {nameof(param.DataSourceCode)},
                    SEQID,
                    CreateDate,
                    MemoJson,
                    {addtionalColumns}
                    ChangeType,
                    ChangePoints,
                    OldAccumulatePoints,
                    NewAccumulatePoints";
            }

            string sql = $@"
                SELECT {columns}
                FROM {param.InlodbType.Value}.dbo.{param.TableName} WITH(NOLOCK)
                WHERE 
	                USERID = @UserID
	                AND CreateDate >= @{startDateParamName}
	                AND CreateDate < @{endDateParamName} ";

            if (!param.IsQueryCount)
            {
                sql += " ORDER BY CreateDate DESC, SEQID DESC ";
            }

            return sql;
        }

        private string GetSingleVIPFlowLogSQL(BuildVIPChangeLogSqlParam param)
        {
            string startDateParamName = null;
            string endDateParamName = null;

            if (param.InlodbType == InlodbType.Inlodb)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbEndDate);
            }
            else if (param.InlodbType == InlodbType.InlodbBak)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbBakStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbBakEndDate);
            }

            string columns = "COUNT(*) AS CNT ";

            if (!param.IsQueryCount)
            {
                columns = $@"TOP {param.PageNo * param.PageSize}
                    N'{param.DataSourceCode}' AS {nameof(param.DataSourceCode)},
                    N'{param.DataSourceName}' AS {nameof(param.DataSourceCode)},
                    SEQID,
                    CreateDate,
                    MemoJson,
                    ChangeType,
                    ChangeFlowAmount,
                    Multiple
                    OldFlowAccountAmount,
                    NewFlowAccountAmount";
            }

            string sql = $@"
                SELECT {columns}
                FROM {param.InlodbType.Value}.dbo.{param.TableName} WITH(NOLOCK)
                WHERE 
	                USERID = @UserID
	                AND CreateDate >= @{startDateParamName}
	                AND CreateDate < @{endDateParamName} ";

            if (!param.IsQueryCount)
            {
                sql += " ORDER BY CreateDate DESC, SEQID DESC ";
            }

            return sql;
        }

        private string GetSingleVIPAgentAccountLogSQL(BuildVIPChangeLogSqlParam param)
        {
            string startDateParamName = null;
            string endDateParamName = null;

            if (param.InlodbType == InlodbType.Inlodb)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbEndDate);
            }
            else if (param.InlodbType == InlodbType.InlodbBak)
            {
                startDateParamName = nameof(TableSearchDateRange.InlodbBakStartDate);
                endDateParamName = nameof(TableSearchDateRange.SmallThanInlodbBakEndDate);
            }

            string columns = "COUNT(*) AS CNT ";

            if (!param.IsQueryCount)
            {
                columns = $@"TOP {param.PageNo * param.PageSize}
                    N'{param.DataSourceCode}' AS {nameof(param.DataSourceCode)},
                    N'{param.DataSourceName}' AS {nameof(param.DataSourceCode)},
                    SEQID,
                    CreateDate,
                    MemoJson,
                    ChangeType,
                    ChangeAvailableScores,
                    OldAvailableScores,
                    NewAvailableScores,
                    ChangeFreezeScores,
                    OldFreezeScores,
                    NewFreezeScores";
            }

            string sql = $@"
                SELECT {columns}
                FROM {param.InlodbType.Value}.dbo.{param.TableName} WITH(NOLOCK)
                WHERE 
	                USERID = @UserID
	                AND CreateDate >= @{startDateParamName}
	                AND CreateDate < @{endDateParamName} ";

            if (!param.IsQueryCount)
            {
                sql += " ORDER BY CreateDate DESC, SEQID DESC ";
            }

            return sql;
        }        
        #endregion

        private enum VIPChangeLogTypes
        {
            Points,
            Flow,
            AgentScore
        }
    }
}