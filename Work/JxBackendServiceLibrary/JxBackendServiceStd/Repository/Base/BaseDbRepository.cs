using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.db;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Repository.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseDbRepository : BaseDbRepository<object>
    {
        public BaseDbRepository(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<long> BaseCreate(object model, string tableName)
        {
            throw new InvalidOperationException("不支援此方法");
        }
    }

    public abstract class BaseDbRepository<T> : IBaseDbRepository<T> where T : class
    {
        private static readonly string _defaultCreateDateColumnName = "CreateDate";

        private static readonly string _defaultUpdateDateColumnName = "UpdateDate";

        private static readonly string _defaultCreateUserColumnName = "CreateUser";

        private static readonly string _defaultUpdateUserColumnName = "UpdateUser";

        private static readonly int _inloDbBakMoveDataPreDays = 4;

        private static readonly int _inloDbBakClearDataMonths = 3;

        private static readonly int _batchQueryParamCount = 2000;

        private readonly EnvironmentUser _envLoginUser;

        protected DbConnectionTypes DBConnectionType { get; private set; }

        private readonly Lazy<DbHelperSQL> _dbHelper;

        protected DbHelperSQL DbHelper => _dbHelper.Value;

        protected EnvironmentUser EnvLoginUser => _envLoginUser;

        public BaseDbRepository(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            _envLoginUser = envLoginUser;
            DBConnectionType = dbConnectionType;

            //有解密運算,故使用lazy避免初始化就做了不必要的運算(不一定服務內的每個REP都會在同個request用到)
            _dbHelper = new Lazy<DbHelperSQL>(() =>
            {
                string connectionString = GetConnectionString(envLoginUser.Application, dbConnectionType);
                return new DbHelperSQL(connectionString);
            });
        }

        private IDbConnection GetDbConnection()
        {
            return _dbHelper.Value.GetSqlConnection();
        }

        #region Create

        #region ad hoc version

        //[Obsolete("配合DBA規範, 請改用CreateByProcedure")]
        //public bool Create(T model)
        //{
        //    return Create(model, true, out long identity);
        //}

        //[Obsolete("配合DBA規範, 請改用CreateByProcedure")]
        //public bool Create(T model, bool isAddDbOperationLog, out long identity)
        //{
        //    return Create(null, model, isAddDbOperationLog, false, out identity);
        //}

        //[Obsolete("配合DBA規範, 請改用CreateByProcedure")]
        //public bool Create(T model, bool isAddDbOperationLog, bool isSystemJob, out long identity)
        //{
        //    return Create(null, model, isAddDbOperationLog, isSystemJob, out identity);
        //}

        //[Obsolete("配合DBA規範, 請改用CreateByProcedure")]
        //public bool Create(string tableName, T model, bool isAddDbOperationLog, bool isSystemJob, out long identity)
        //{
        //    return BaseCreate(false, tableName, model, isAddDbOperationLog, isSystemJob, out identity);
        //}

        //[Obsolete("配合DBA規範, 請改用CreateListByProcedure")]
        //public bool CreateList(IList<T> models)
        //{
        //    foreach (var model in models)
        //    {
        //        Create(model);
        //    }

        //    return true;
        //}

        #endregion ad hoc version

        #region sp version

        public BaseReturnDataModel<long> CreateByProcedure(T model)
        {
            return BaseCreate(model, null);
        }

        public BaseReturnDataModel<long> CreateByProcedure(T model, string tableName)
        {
            return BaseCreate(model, tableName);
        }

        public bool CreateListByProcedure(IList<T> models)
        {
            foreach (var model in models)
            {
                CreateByProcedure(model);
            }

            return true;
        }

        #endregion sp version

        #endregion Create

        protected virtual BaseReturnDataModel<long> BaseCreate(T model, string tableName)
        {
            DateTime now = DateTime.Now;
            //效能關係改為先判斷有沒有值再取得目前user name
            model.SetPropertyValue(_defaultCreateDateColumnName, now);
            model.SetPropertyValue(_defaultUpdateDateColumnName, now);
            model.SetPropertyValueWhenNull(_defaultCreateUserColumnName, () => EnvLoginUser.LoginUser.UserId.ToString());
            model.SetPropertyValueWhenNull(_defaultUpdateUserColumnName, () => EnvLoginUser.LoginUser.UserId.ToString());

            var returnDataModel = new BaseReturnDataModel<long>();
            IDbConnection conn = GetDbConnection();
            returnDataModel.DataModel = conn.SaveByProcedure(ActTypes.Insert, tableName, model);

            //if (isSaveByProcedure)
            //{
            //    identity = conn.OwmsSaveByProcedure(ActTypes.Insert, tableName, model);
            //}
            //else
            //{
            //    identity = conn.OwmsInsert(tableName, model);
            //}

            return returnDataModel;
        }

        public void BulkCopy(InlodbType inlodbType, DataTable dataTable)
        {
            dataTable.TableName = $"{inlodbType}.dbo.{GetTableName()}";
            DbHelper.BulkCopy(dataTable, SqlBulkCopyOptions.Default);
        }

        #region Update

        #region ad hoc version

        //[Obsolete("配合DBA規範, 請改用UpdateByProcedure")]
        //public bool Update(T model, bool isAddDbOperationLog = true, T operationModel = null)
        //{
        //    return BaseUpdate(false, model, isAddDbOperationLog, false, operationModel);
        //}

        //[Obsolete("配合DBA規範, 請改用UpdateByProcedure")]
        //public bool Update(T model, bool isSystemJob, bool isAddDbOperationLog)
        //{
        //    return BaseUpdate(false, model, isAddDbOperationLog, isSystemJob, null);
        //}

        #endregion ad hoc version

        #region sp version

        public bool UpdateByProcedure(T model)
        {
            return BaseUpdate(model, false);
        }

        public bool UpdateByProcedure(T model, bool isSystemJob)
        {
            return BaseUpdate(model, isSystemJob);
        }

        #endregion sp version

        #endregion Update

        private bool BaseUpdate(T model, bool isSystemJob)
        {
            //T beforeSaveModel = null;

            //if (isAddDbOperationLog)
            //{
            //    beforeSaveModel = GetSingleByKey(model, DbConnTypes.ReadWrite, true);
            //}

            model.SetPropertyValue(_defaultUpdateDateColumnName, DateTime.Now);

            if (isSystemJob)
            {
                model.SetPropertyValueWhenNull(_defaultUpdateUserColumnName, () => GlobalVariables.SystemOperator);
            }
            else
            {
                model.SetPropertyValueWhenNull(_defaultUpdateUserColumnName, () => EnvLoginUser.LoginUser.UserId.ToString());
            }

            IDbConnection conn = GetDbConnection();
            bool isSuccess = conn.SaveByProcedure(ActTypes.Update, model) > 0;
            //if (isSaveByProcedure)
            //{
            //    isSuccess = sqlConnection.OwmsSaveByProcedure(ActTypes.Update, model) > 0;
            //}
            //else
            //{
            //    isSuccess = sqlConnection.OwmsUpdate(model);
            //}

            return isSuccess;
        }

        //[Obsolete("配合DBA規範, 請改用UpdateListByProcedure")]
        //public bool UpdateList(IList<T> models)
        //{
        //    foreach (var model in models)
        //    {
        //        if (!Update(model, false))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public bool UpdateListByProcedure(IList<T> models)
        {
            foreach (var model in models)
            {
                if (!UpdateByProcedure(model))
                {
                    return false;
                }
            }

            return true;
        }

        //[Obsolete("配合DBA規範, 請改用DeleteByProcedure")]
        //public bool Delete(T model, bool isAddDbOperationLog = true)
        //{
        //    return BaseDelete(false, model, isAddDbOperationLog);
        //}

        public bool DeleteByProcedure(T model)
        {
            return BaseDelete(model);
        }

        private bool BaseDelete(T model)
        {
            //T beforeSaveModel = GetSingleByKey(model, DbConnTypes.ReadWrite);

            IDbConnection conn = GetDbConnection();
            bool isSuccess = conn.SaveByProcedure(ActTypes.Delete, model) > 0;
            //if (isSaveByProcedure)
            //{
            //    isSuccess = sqlConnection.SaveByProcedure(ActTypes.Delete, model) > 0;
            //}
            //else
            //{
            //    isSuccess = sqlConnection.OwmsDelete(model);
            //}

            return isSuccess;
        }

        //[Obsolete("配合DBA規範, 請改用DeleteListByProcedure")]
        //public bool DeleteList(IList<T> models)
        //{
        //    foreach (var model in models)
        //    {
        //        if (!Delete(model))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public bool DeleteListByProcedure(IList<T> models)
        {
            foreach (var model in models)
            {
                if (!DeleteByProcedure(model))
                {
                    return false;
                }
            }

            return true;
        }

        protected string GetAllQuerySQL(InlodbType inlodbType)
        {
            return GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                DbType = inlodbType
            });
        }

        protected string GetAllQuerySQL(InlodbType inlodbType, List<string> columnNameFilters)
        {
            return GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                DbType = inlodbType,
                ColumnNameFilters = columnNameFilters
            });
        }

        protected string GetAllQuerySQL(InlodbType inlodbType, int? topRow, List<string> columnNameFilters)
        {
            return GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                DbType = inlodbType,
                TopRow = topRow,
                ColumnNameFilters = columnNameFilters
            });
        }

        protected string GetAllQuerySQL(InlodbType inlodbType, int? topRow)
        {
            return GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                DbType = inlodbType,
                TopRow = topRow
            });
        }

        protected string GetAllQuerySQL(InlodbType inlodbType, string tableName)
        {
            return GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                DbType = inlodbType,
                TableName = tableName,
            });
        }

        protected string GetAllQuerySQL(GetAllQuerySQLParam param)
        {
            Type type = typeof(T);

            string topExpression = string.Empty;

            if (param.TopRow.HasValue)
            {
                topExpression = $"TOP({param.TopRow.Value})";
            }

            List<string> properties = ModelUtil.GetPropertiesNameOfType(type);

            if (param.ColumnNameFilters.AnyAndNotNull())
            {
                properties = properties.Where(w => param.ColumnNameFilters.Any(f => f.Equals(w, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            string columnNames = string.Join(",",
                ModelUtil.GetAllColumnInfos<T>(properties, param.IsAppendTableNameToColumn, param.TableName).Select(s => s.FullColumnName));

            string sql = $"SELECT {topExpression} {columnNames} {GetFromTableSQL(param.DbType, param.TableName)} ";

            return sql;
        }

        public string GetFromTableSQL(InlodbType inlodbType)
        {
            return GetFromTableSQL(inlodbType, null);
        }

        public string GetFromTableSQL(InlodbType inlodbType, string tableName)
        {
            if (tableName.IsNullOrEmpty())
            {
                tableName = GetTableNameFromModelType();
            }

            return $" FROM [{inlodbType.Value}].[dbo].[{tableName}] WITH(NOLOCK) ";
        }

        protected string GetTableNameFromModelType() => typeof(T).Name;

        protected PagedSqlQueryParamsModel CreateAllColumnsPagedSqlQueryParams()
        {
            return CreateAllColumnsPagedSqlQueryParams(null, null, null, null, null, null);
        }

        protected PagedSqlQueryParamsModel CreateAllColumnsPagedSqlQueryParams(string whereString,
            object parameters, BasePagingRequestParam requestParam)
        {
            return CreateAllColumnsPagedSqlQueryParams(null, string.Empty, null, whereString, parameters, requestParam);
        }

        protected PagedSqlQueryParamsModel CreateAllColumnsPagedSqlQueryParams(string tableAlias, string whereString,
            object parameters, BasePagingRequestParam requestParam)
        {
            return CreateAllColumnsPagedSqlQueryParams(null, tableAlias, null, whereString, parameters, requestParam);
        }

        protected PagedSqlQueryParamsModel CreateAllColumnsPagedSqlQueryParams(string tableName, string tableAlias, List<string> properties, string whereString,
            object parameters, BasePagingRequestParam requestParam)
        {
            return CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam()
            {
                TableName = tableName,
                TableAlias = tableAlias,
                Properties = properties,
                WhereString = whereString,
                Parameters = parameters,
                RequestParam = requestParam
            });
        }

        protected PagedSqlQueryParamsModel CreateAllColumnsPagedSqlQueryParams(BuildPagedSqlQueryParam buildParam)
        {
            Type type = typeof(T);

            if (buildParam.Properties == null)
            {
                buildParam.Properties = ModelUtil.GetPropertiesNameOfType(type);
            }

            var selectColumns = new StringBuilder();
            for (int i = 0; i < buildParam.Properties.Count; i++)
            {
                if (i > 0)
                {
                    selectColumns.Append(", ");
                }

                if (!buildParam.TableAlias.IsNullOrEmpty())
                {
                    selectColumns.Append(buildParam.TableAlias + ".");
                }

                selectColumns.Append(buildParam.Properties[i]);
            }

            string bodyTableName = type.Name;

            if (!buildParam.TableName.IsNullOrEmpty())
            {
                bodyTableName = buildParam.TableName;
            }

            string fullTableName = null;

            if (buildParam.InlodbType != null)
            {
                fullTableName = $"{buildParam.InlodbType}.dbo.";
            }

            fullTableName += $"[{bodyTableName}] {buildParam.TableAlias} WITH(NOLOCK) ";

            PagedSqlQueryParamsModel model = new PagedSqlQueryParamsModel()
            {
                SelectColumns = selectColumns.ToString(),
                SqlBody = fullTableName
            };

            if (!buildParam.WhereString.IsNullOrEmpty())
            {
                model.SqlBody += buildParam.WhereString;
            }

            if (buildParam.Parameters != null)
            {
                model.Parameters = buildParam.Parameters;
            }

            model.SetPager(buildParam.RequestParam);

            if (buildParam.RequestParam != null)
            {
                model.MaxSearchRowCount = GlobalVariables.MaxSearchRowCount;
            }

            return model;
        }

        /// <summary>
        /// 因為是整張表查詢,避免被濫用,所以改為protected
        /// </summary>
        protected List<T> GetAll(InlodbType inlodbType)
        {
            string sql = GetAllQuerySQL(inlodbType);

            using (IDbConnection connection = GetDbConnection())
            {
                return connection.Query<T>(sql, null).ToList();
            }
        }

        public T GetSingleByKey(InlodbType inlodbType, T model)
        {
            return GetSingleByKey(inlodbType, model, isClearUpdateUserAndTime: true); //讓更新回DB的時候寫入登入用戶資訊
        }

        public T GetSingleByKey(InlodbType inlodbType, T model, bool isClearUpdateUserAndTime)
        {
            using (IDbConnection connection = GetDbConnection())
            {
                T modelByKey = connection.GetByKey(inlodbType, model);

                if (isClearUpdateUserAndTime && modelByKey != null)
                {
                    modelByKey.SetPropertyValue(_defaultUpdateDateColumnName, null);
                    modelByKey.SetPropertyValue(_defaultUpdateUserColumnName, null);
                }

                return modelByKey;
            }
        }

        //public string GetTableSequence()
        //{
        //    string tableName = GetTableName();

        //    return GetTableSequence(tableName);
        //}

        protected string GetTableName() => ModelUtil.GetTableName(typeof(T));

        //protected string GetTableSequence(string tableName)
        //{
        //    var dynamicParameters = new DynamicParameters();
        //    dynamicParameters.Add("TableName", tableName.ToVarchar(50));
        //    string sql = @"
        //                DECLARE @SeqResult varchar(32)

        //                EXEC [dbo].[Pro_GetTableSequence]
        //                  @TableName = @TableName,
        //                  @SeqResult = @SeqResult OUTPUT

        //                SELECT	@SeqResult ";
        //    return DbHelper.ExecuteScalar<string>(sql, dynamicParameters);
        //}

        protected string GetSequenceIdentity(string sequenceName)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("SequenceName", sequenceName.ToVarchar(50));

            string sql = $@"
                DECLARE @SeqResult varchar(32)

                EXEC {InlodbType.Inlodb}.[dbo].[Pro_GetSequenceIdentity]
		            @SequenceName = @SequenceName,
		            @SEQID = @SeqResult OUTPUT

                SELECT @SeqResult ";

            return DbHelper.ExecuteScalar<string>(sql, dynamicParameters);
        }

        protected PagedResultModel<T> GetCrossDbQueryList(JxCrossDbQueryParam param)
        {
            return GetCrossDbQueryList<T>(param, null);
        }

        protected PagedResultModel<ModelType> GetCrossDbQueryList<ModelType>(JxCrossDbQueryParam param) where ModelType : class
        {
            return GetCrossDbQueryList<ModelType>(param, null);
        }

        protected PagedResultModel<ModelType> GetCrossDbQueryList<ModelType>(JxCrossDbQueryParam param, Action<SqlMapper.GridReader> saveStatJob) where ModelType : class
        {
            var jxCrossDbQueryParam = new MultiplePagedSqlQueryParam()
            {
                PageNo = param.PageNo,
                PageSize = param.PageSize,
                Parameters = param.Parameters
            };

            if (!param.SelectColumnInfos.AnyAndNotNull())
            {
                param.SelectColumnInfos = ModelUtil.GetAllColumnInfos<ModelType>();
            }

            string tableName = param.BasicTableName;

            if (tableName.IsNullOrEmpty())
            {
                tableName = ModelUtil.GetTableName(typeof(ModelType));
            }

            foreach (InlodbType inlodbType in InlodbType.GetAll())
            {
                if (inlodbType == InlodbType.Inlodb && param.InlodbFilters.IsNullOrEmpty())
                {
                    continue;
                }
                else if (inlodbType == InlodbType.InlodbBak && param.InlodbBakFilters.IsNullOrEmpty())
                {
                    continue;
                }

                jxCrossDbQueryParam.SingleTableQueryParams.Add(new SingleTableQueryParam()
                {
                    SelectColumnInfos = param.SelectColumnInfos,
                    StatColumns = param.StatColumns,
                    StatGroupByColumns = param.StatGroupByColumns,
                    Filters = param.GetFilter(inlodbType),
                    OrderBy = param.OrderBy,
                    SummaryTempTableOrderBy = param.SummaryTempTableOrderBy,
                    FullTableName = $"{inlodbType}.dbo.{tableName}"
                });
            }

            return DbHelper.MultiplePagedSqlQuery<ModelType>(jxCrossDbQueryParam, saveStatJob);
        }

        /// <summary> 從InlodbBak存留的最早日期到目前 </summary>
        protected TableSearchDateRange GetBiggestTableSearchDateRange()
        {
            return GetTableSearchDateRange(DateTime.Now.AddMonths(-_inloDbBakClearDataMonths), DateTime.Now);
        }

        protected TableSearchDateRange GetTableSearchDateRange(DateTime searchStartDate, DateTime searchEndDate)
        {
            var tableSearchDateRange = new TableSearchDateRange();

            if (searchStartDate > searchEndDate)
            {
                return tableSearchDateRange;
            }

            DateTime divide = DateTime.Now.AddDays(-_inloDbBakMoveDataPreDays);

            if (searchStartDate < divide && searchEndDate < divide) //落在inlodbBak
            {
                tableSearchDateRange.InlodbBakStartDate = searchStartDate;
                tableSearchDateRange.SmallThanInlodbBakEndDate = searchEndDate.ToQuerySmallThanTime(DatePeriods.Second);
            }
            else if (searchStartDate >= divide && searchEndDate >= divide) //落在inlodb
            {
                tableSearchDateRange.InlodbStartDate = searchStartDate;
                tableSearchDateRange.SmallThanInlodbEndDate = searchEndDate.ToQuerySmallThanTime(DatePeriods.Second);
            }
            else if (searchStartDate < divide && searchEndDate >= divide) //跨db
            {
                tableSearchDateRange.InlodbBakStartDate = searchStartDate;
                tableSearchDateRange.SmallThanInlodbBakEndDate = divide;
                tableSearchDateRange.InlodbStartDate = divide;
                tableSearchDateRange.SmallThanInlodbEndDate = searchEndDate.ToQuerySmallThanTime(DatePeriods.Second);
            }

            return tableSearchDateRange;
        }

        protected DailyReportDateRange GetSearchDailyReportDateRange(DateTime queryStartDate, DateTime querySmallEqaulThanEndDate)
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

        protected List<Type> GetBaseUsersByBatchQuery<Type>(string querySql, List<int> userIds)
        {
            List<int> allUserIds = userIds.Distinct().ToList();
            var returnModels = new List<Type>();

            //怕sql param數量太多爆掉
            while (allUserIds.Any())
            {
                List<int> partUserIds = allUserIds.Take(_batchQueryParamCount).ToList();
                returnModels.AddRange(DbHelper.QueryList<Type>(querySql, new { userIds = partUserIds }));
                int removeCount = _batchQueryParamCount;

                if (allUserIds.Count < _batchQueryParamCount)
                {
                    removeCount = allUserIds.Count;
                }

                allUserIds.RemoveRange(0, removeCount);
            }

            return returnModels;
        }

        public DataTable CreateEmptyDataTable(InlodbType inlodbType)
        {
            string sql = GetAllQuerySQL(inlodbType) + "WHERE 1 = 2 ";

            var dataTable = DbHelper.ExecuteReader(
                sql,
                null,
                CommandType.Text,
                (dataReader) =>
                {
                    var table = new DataTable();
                    table.Load(dataReader);

                    return table;
                });

            return dataTable;
        }

        private string GetConnectionString(JxApplication jxApplication, DbConnectionTypes dbConnectionType)
        {
            IAppSettingService appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(jxApplication, SharedAppSettings.PlatformMerchant);
            string connectionString = appSettingService.GetConnectionString(dbConnectionType);
            var fullConnectionString = new StringBuilder(connectionString);

            if (connectionString.IndexOf("TrustServerCertificate", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                if (!connectionString.EndsWith(";"))
                {
                    fullConnectionString.Append(";");
                }

                fullConnectionString.Append("TrustServerCertificate=true");
            }

            return fullConnectionString.ToString();
        }
    }
}