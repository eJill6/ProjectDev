using Dapper;
using Microsoft.Extensions.Logging;
using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Extensions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Models;
using MS.Core.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MS.Core.Infrastructures.DBTools
{
    /// <summary>
    /// DapperComponent
    /// </summary>
    public class DapperComponent
    {
        private ILogger? Logger;
        private IRequestIdentifierProvider? Provider;

        List<SQLBox> SQLBox { get; }

        /// <summary>
        /// MsSql 連線字串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionString"></param>
        public DapperComponent(string connectionString)
        {
            ConnectionString = connectionString;
            SQLBox = new List<SQLBox>();
        }

        public DapperComponent(ILogger? logger, IRequestIdentifierProvider? provider, string connectionString)
        {
            this.Logger = logger;
            this.Provider = provider;
            this.ConnectionString = connectionString;
            SQLBox = new List<SQLBox>();
        }

        public DapperQueryComponent<Table> QueryTable<Table>() where Table : class
        {
            return new DapperQueryComponent<Table>(this);
        }
        /// <summary>
        /// 單筆刪除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DapperComponent Delete<T>(T entity) where T : BaseDBModel
        {
            string sql = SQLHelper.GetDeleteCommand<T>();

            AddSQLBox(sql, entity);

            return this;
        }

        /// <summary>
        /// 多筆刪除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DapperComponent Delete<T>(IEnumerable<T> entities) where T : BaseDBModel
        {
            string sql = SQLHelper.GetDeletesCommand<T>();

            AddSQLBox(sql, new { Ids = GetIds(entities) });

            return this;
        }

        private List<object> GetIds<T>(IEnumerable<T> entities) where T : BaseDBModel
        {
            List<object> ids = new List<object>();

            foreach (T entity in entities)
            {
                PropertyInfo? idProperty = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)));

                if (idProperty != null)
                {
                    object? idValue = idProperty.GetValue(entity);
                    if (idValue != null)
                    {
                        ids.Add(idValue);
                    }
                }
            }

            return ids;
        }

        /// <summary>
        /// 單筆新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DapperComponent Insert<T>(T entity) where T : BaseDBModel
        {
            string sql = SQLHelper.GetInsertSql<T>();

            AddSQLBox(sql, entity);

            return this;
        }

        /// <summary>
        /// 多筆新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enties"></param>
        /// <returns></returns>
        public DapperComponent Insert<T>(IEnumerable<T> enties) where T : BaseDBModel
        {
            string sql = SQLHelper.GetInsertSql<T>();

            AddSQLBox(sql, enties);

            return this;
        }

        /// <summary>
        /// 多筆更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enties"></param>
        /// <returns></returns>
        public DapperComponent Update<T>(IEnumerable<T> enties) where T : BaseDBModel
        {
            string sql = SQLHelper.GetUpdateCommand<T>();

            AddSQLBox(sql, enties);

            return this;
        }

        /// <summary>
        /// 單筆更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DapperComponent Update<T>(T entity) where T : BaseDBModel
        {
            string sql = SQLHelper.GetUpdateCommand<T>();

            AddSQLBox(sql, entity);

            return this;
        }

        /// <summary>
        /// 多筆查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? paras = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            IEnumerable<T> entities = Enumerable.Empty<T>();

            await DBConnectionAsync(async conn =>
            {
                Func<string, object, Task<IEnumerable<T>>> func = async (sql, paras) =>
                {
                    return await conn.QueryAsync<T>(sql, paras, commandTimeout: commandTimeout, commandType: commandType);
                };
                entities = await QueryFuncAsync(func, sql, paras);
            });

            return entities;
        }


        private async Task<T> QueryFuncAsync<T>(Func<string, object, Task<T>> func, string sql, object paras = null)
        {
            T? entities = default;

            await DBConnectionAsync(async conn =>
            {
                try
                {
                    entities = await func(sql, paras);
                }
                catch (Exception ex)
                {
                    if (Logger != null && Provider != null)
                    {
                        var param = string.Empty;
                        if (paras != null)
                        {
                            param = " param:";
                            switch (paras)
                            {
                                case DynamicParameters p1:
                                    param += string.Join(",", p1.ParameterNames.Select(x => $"{x}={JsonUtil.ToJsonString(p1.Get<object>(x))}"));
                                    break;
                                default:
                                    param += $"{JsonUtil.ToJsonString(paras)}";
                                    break;
                            }
                        }
                        Logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} sql:{sql}{param}");
                    }
                    throw;
                }
            });

            return entities;
        }

        /// <summary>
        /// 單筆查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<T?> QueryFirstAsync<T>(string sql, object paras = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            return (await QueryAsync<T>(sql, paras, commandTimeout: commandTimeout, commandType: commandType)).FirstOrDefault();
        }

        /// <summary>
        /// 依條件查詢第一筆第一個欄位資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<T?> QueryScalarAsync<T>(string sql, object paras = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            T? result = default;

            await DBConnectionAsync(async conn =>
            {
                Func<string, object, Task<T>> func = async (sql, paras) =>
                {
                    return await conn.ExecuteScalarAsync<T>(sql, paras, commandTimeout: commandTimeout, commandType: commandType);
                };
                result = await QueryFuncAsync(func, sql, paras);
            });

            return result;
        }

        /// <summary>
        /// 以非同步方式將在此內容中所做的所有變更儲存至基礎資料庫
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            if (SQLBox.Count == 0)
            {
                return;
            }

            try
            {
                await ExecuteAsync(SQLBox);
            }
            catch (SqlException sex)
            {
                throw sex;
            }
            finally
            {
                ClearChanges();
            }
        }

        /// <summary>
        /// 清空 _SQLBox
        /// </summary>
        /// <returns></returns>
        public DapperComponent ClearChanges()
        {
            SQLBox.Clear();

            return this;
        }

        public async Task<S> InsertAndResultIdentity<S, T>(T model) where T : BaseDBModel
        {
            string sql = SQLHelper.GetInsertSql<T>();

            sql = SQLHelper.ConcatReturnIdentityCommand(sql);

            S? entities = default;

            await DBConnectionAsync(async conn =>
            {
                entities = await conn.ExecuteScalarAsync<S>(sql, model);
            });

            return entities;
        }

        /// <summary>
        /// DbConnection
        /// </summary>
        protected Func<Func<IDbConnection, Task>, Task> DBConnectionAsync
        {
            get
            {
                return async (action) =>
                {
                    using (IDbConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        await action.Invoke(conn);
                    };
                };
            }
        }

        /// <summary>
        /// 加入 sql 待執行語法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DapperComponent AddExecuteSQL(string sql, object param, bool isParamNullExecute = true)
        {
            if(isParamNullExecute || param != null)
            {
                AddSQLBox(sql, param);
            }
            return this;
        }

        public DapperComponent AddRepoFunction (Func<DapperComponent, DapperComponent> func)
        {
            return func(this);
        }

        /// <summary>
        /// 執行SQL語法
        /// </summary>
        /// <param name="sqlboxes"></param>
        /// <returns></returns>
        private async Task ExecuteAsync(IEnumerable<SQLBox> sqlboxes)
        {
            await DBConnectionAsync(async conn =>
            {
                using IDbTransaction trx = conn.BeginTransaction();

                try
                {
                    foreach (SQLBox box in sqlboxes)
                    {
                        await conn.ExecuteAsync(box.Sql, box.Parameter, trx, box.CommandTimeout, box.CommandType);
                    }
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    if (Logger != null && Provider != null)
                    {
                        Logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} sql:{JsonUtil.ToJsonString(sqlboxes)}");
                    }
                    trx.Rollback();
                    throw;
                }
            });
        }

        private DapperComponent AddSQLBox(string sql, object param)
        {
            SQLBox box = new()
            {
                Parameter = param,
                Sql = sql
            };

            SQLBox.Add(box);

            return this;
        }

        public async Task<string> GetSequenceIdentity(string sequenceName)
        {
            string sql = $@"
                DECLARE @SeqResult varchar(32)

                EXEC Inlodb.[dbo].[Pro_GetSequenceIdentity]
		            @SequenceName = @SequenceName,
		            @SEQID = @SeqResult OUTPUT

                SELECT @SeqResult ";
            return await QueryScalarAsync<string>(sql, new { SequenceName = sequenceName.ToVarchar(50) }) ?? string.Empty;
        }
    }
}
