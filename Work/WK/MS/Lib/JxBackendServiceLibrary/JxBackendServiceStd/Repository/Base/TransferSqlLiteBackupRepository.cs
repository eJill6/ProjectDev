using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base.SqlLite;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace JxBackendService.Repository.Base
{
    public class TransferSqlLiteBackupRepository : BaseSqlLiteRepository, ITransferSqlLiteBackupRepository
    {
        private static readonly string s_backupFolder = "BetlogBackup";

        private static readonly string s_backupTableName = "ProfitlossBackup";

        private static readonly int _queryInMaxParameterCount = SqliteDbHelperSQL.QueryInMaxParameterCount;

        private static readonly object s_lock = new object();

        protected override SqliteDbHelperSQL SqliteDbHelper => new SqliteDbHelperSQL(GetConnectionString(DbFullPath, DefaultDbPassword));

        public TransferSqlLiteBackupRepository() : base()
        {
        }

        protected override string DbFullPath
        {
            get
            {
                DateTime today = DateTime.Today;
                string dbFullPath = GetDbFullPath(today);

                return dbFullPath;
            }
        }

        public void DeleteExpiredDbFile()
        {
            for (int i = 6; i <= 30; i++)
            {
                DateTime deleteDay = DateTime.Today.AddDays(-i);
                DeleteDbFile(deleteDay);
            }
        }

        public List<T> GetBetLogs<T>(DateTime fileDate, int rowCount)
        {
            string dbFullPath = GetDbFullPath(fileDate);
            var sqliteDbHelperSQL = new SqliteDbHelperSQL(GetConnectionString(dbFullPath, DefaultDbPassword));

            string sql = $@"
                SELECT BetLogJson
                FROM {s_backupTableName}
                LIMIT 0, {rowCount}";

            List<string> betLogJsons = sqliteDbHelperSQL.QueryList<string>(sql, param: null);

            return betLogJsons.Select(s => s.Deserialize<T>()).ToList();
        }

        public void BackupNewBetLogs<T>(List<T> betLogs) where T : BaseRemoteBetLog
        {
            List<string> keyIds = betLogs.Select(s => s.KeyId).ToList();
            HashSet<string> keyIdSet = GetExistKeyIdSet(keyIds);
            Dictionary<string, T> backupBetLogMap = betLogs.Where(w => !keyIdSet.Contains(w.KeyId)).ToDictionary(d => d.KeyId);

            if (backupBetLogMap.Any())
            {
                InsertBetLogs(backupBetLogMap);
            }
        }

        private HashSet<string> GetExistKeyIdSet(List<string> keyIds)
        {
            lock (s_lock)
            {
                InitSettings(DbFullPath);

                string sql = $@"
                SELECT KeyId
                FROM [{s_backupTableName}]
                WHERE KeyId IN @keyIds ";

                var allKeyIds = keyIds.CloneByJson();
                var set = new HashSet<string>();

                lock (s_lock)
                {
                    using (IDbConnection dbConnection = SqliteDbHelper.GetSqlConnection())
                    {
                        dbConnection.Open();

                        while (allKeyIds.Any())
                        {
                            List<string> batchKeyIds = allKeyIds.Take(_queryInMaxParameterCount).ToList();

                            object param = new
                            {
                                keyIds = batchKeyIds.Select(s => s.ToNVarchar(100))
                            };

                            List<string> queryResult = dbConnection.Query<string>(sql, param).ToList();
                            queryResult.ForEach(f => set.Add(f));

                            allKeyIds.RemoveRangeByFit(0, _queryInMaxParameterCount);
                        }

                        dbConnection.Close();
                    }
                }

                return set;
            }
        }

        public void InsertBetLogs<T>(Dictionary<string, T> betLogMap)
        {
            lock (s_lock)
            {
                InitSettings(DbFullPath);

                string insertSQL = $@"
                INSERT INTO {s_backupTableName}(
                    KeyId,
                    BetLogJson,
                    LocalSavedTime)
                VALUES(
                    @KeyId,
                    @BetLogJson,
                    @LocalSavedTime); ";

                using (IDbConnection dbConnection = SqliteDbHelper.GetSqlConnection())
                {
                    dbConnection.Open();

                    using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                    {
                        foreach (string keyId in betLogMap.Keys)
                        {
                            object param = new
                            {
                                KeyId = keyId,
                                BetLogJson = betLogMap[keyId].ToJsonString(),
                                LocalSavedTime = DateTime.Now.ToFormatDateTimeMillisecondsString()
                            };

                            dbConnection.Execute(insertSQL, param, commandType: CommandType.Text);
                        }

                        dbTransaction.Commit();
                    }

                    dbConnection.Close();
                }
            }
        }

        private void InitSettings(string dbFullPath)
        {
            if (File.Exists(dbFullPath))
            {
                return;
            }

            var fileInfo = new FileInfo(dbFullPath);

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            SQLiteConnection.CreateFile(dbFullPath); //建立db的時候不可相依在DbFullPath屬性上,否則有機會無限遞迴
            TryCreateTableProfitLoss();
        }

        /// <summary>
        /// 創建第三方ProfitLossInfo backup表
        /// </summary>
        private void TryCreateTableProfitLoss()
        {
            if (IsTableExists(s_backupTableName))
            {
                return;
            }

            string sql = $@"
                CREATE TABLE {s_backupTableName}(
                    KeyId TEXT,
                    BetLogJson TEXT,
                    LocalSavedTime TEXT
                );

                CREATE INDEX IDX_{s_backupTableName}_KeyId ON {s_backupTableName} (KeyId);";

            SqliteDbHelper.Execute(sql, null);
        }

        private string GetDbFullPath(DateTime dateTime)
        {
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string dbFileName = $"{dateTime.ToFormatYearMonthDateValue()}.db";
            string dbFullPath = Path.Combine(baseDirectoryPath, s_backupFolder, dbFileName);

            return dbFullPath;
        }

        private void DeleteDbFile(DateTime dateTime)
        {
            string dbFullPath = GetDbFullPath(dateTime);

            if (!File.Exists(dbFullPath))
            {
                return;
            }

            File.Delete(dbFullPath);
        }
    }
}