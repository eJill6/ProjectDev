using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Xml;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public abstract class OldProfitLossInfo : IOldProfitLossInfo
    {
        private readonly int _queryInMaxParameterCount = SqliteDbHelperSQL.QueryInMaxParameterCount;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private readonly Lazy<ITransferSqlLiteBackupRepository> _transferSqlLiteBackupRepository;

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected OldProfitLossInfo()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _transferSqlLiteBackupRepository = DependencyUtil.ResolveService<ITransferSqlLiteBackupRepository>();
        }

        protected int Execute(string sql, object param)
        {
            using (IDbConnection dbConnection = GetSqliteConnection())
            {
                return dbConnection.Execute(sql, param);
            }
        }

        protected DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection dbConnection = GetSqliteConnection() as SQLiteConnection)
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, dbConnection))
                {
                    command.CommandTimeout = 2;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    return data;
                }
            }
        }

        protected abstract object GetSqliteLock();

        protected abstract IDbConnection GetSqliteConnection();

        public DataTable GetBatchDataFromLocalDB(string tableName)
        {
            string selectSql = $@"
                SELECT *
                FROM [{tableName}]
                WHERE remoteSaved = 0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
                ORDER BY Id
                LIMIT 0, 3000 ";

            return ExecuteDataTable(selectSql, null);
        }

        public void BackupBetLogs(string tableName, List<string> successKeyIds)
        {
            Dictionary<string, object> betLogMap = GetDataFromLocalDB(tableName, successKeyIds);

            if (betLogMap != null)
            {
                _transferSqlLiteBackupRepository.Value.InsertBetLogs(betLogMap);
            }
        }

        protected void BatchSaveDataToLocal<T>(string sql, List<T> parameters)
        {
            if (!parameters.Any())
            {
                return;
            }

            object sqliteLock = GetSqliteLock();

            lock (sqliteLock)
            {
                using (IDbConnection dbConnection = GetSqliteConnection())
                {
                    dbConnection.Open();

                    using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                    {
                        foreach (T param in parameters)
                        {
                            dbConnection.Execute(sql, param);
                        }

                        dbTransaction.Commit();
                    }

                    dbConnection.Close();
                }
            }
        }

        protected HashSet<T> GetExistColumnSet<T>(string tableName, string columnName, List<T> keyIds)
        {
            string sql = $@"
                SELECT {columnName}
                FROM [{tableName}]
                WHERE {columnName} IN @keyIds ";

            var allKeyIds = keyIds.CloneByJson();
            var dbLock = GetSqliteLock();
            var set = new HashSet<T>();

            lock (dbLock)
            {
                using (IDbConnection dbConnection = GetSqliteConnection())
                {
                    dbConnection.Open();

                    while (allKeyIds.Any())
                    {
                        List<T> batchKeyIds = allKeyIds.Take(_queryInMaxParameterCount).ToList();
                        object param = null;

                        if (typeof(T) == typeof(string))
                        {
                            param = new
                            {
                                keyIds = batchKeyIds.Select(s => (s as string).ToNVarchar(100))
                            };
                        }
                        else
                        {
                            param = new { keyIds = batchKeyIds };
                        }

                        List<T> queryResult = dbConnection.Query<T>(sql, param).ToList();
                        queryResult.ForEach(f => set.Add(f));

                        allKeyIds.RemoveRangeByFit(0, _queryInMaxParameterCount);
                    }

                    dbConnection.Close();
                }
            }

            return set;
        }

        private Dictionary<string, object> GetDataFromLocalDB(string tableName, List<string> keyIds)
        {
            string sql = $@"
                SELECT *
                FROM [{tableName}]
                WHERE Id IN @keyIds ";

            var allKeyIds = keyIds.CloneByJson();
            var betLogs = new List<object>();
            var dbLock = GetSqliteLock();

            lock (dbLock)
            {
                using (IDbConnection dbConnection = GetSqliteConnection())
                {
                    dbConnection.Open();

                    while (allKeyIds.Any())
                    {
                        List<string> batchKeyIds = allKeyIds.Take(_queryInMaxParameterCount).ToList();

                        List<object> batchBetLogs = dbConnection.Query<object>(
                            sql,
                            new
                            {
                                keyIds = batchKeyIds.Select(s => s.ToNVarchar(100))
                            })
                            .ToList();

                        if (batchBetLogs.Any())
                        {
                            betLogs.AddRange(batchBetLogs);
                        }

                        allKeyIds.RemoveRangeByFit(0, _queryInMaxParameterCount);
                    }

                    dbConnection.Close();
                }
            }

            return betLogs.ToDictionary(d => { return (d as IDictionary<string, object>)["Id"].ToString(); });
        }
    }
}