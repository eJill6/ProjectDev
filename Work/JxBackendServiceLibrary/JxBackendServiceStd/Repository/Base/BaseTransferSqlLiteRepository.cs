using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base.SqlLite;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTransferSqlLiteRepository<T> : BaseSqlLiteRepository, ITransferSqlLiteRepository
    {
        private readonly string _lastSearchKeyTableName = "LastSearchToken";

        #region abstract methods

        public abstract PlatformProduct Product { get; }

        public abstract string ProfitlossTableName { get; }

        public abstract string[] CreateProfitlossTableSqls { get; }

        #endregion abstract methods

        public BaseTransferSqlLiteRepository() : base()
        {
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            DbFullPath = $"{baseDirectoryPath}{Path.DirectorySeparatorChar}{Product.Value}_data.db";
        }

        public void TryCreateDataBase()
        {
            if (!File.Exists(DbFullPath))
            {
                SQLiteConnection.CreateFile(DbFullPath);
            }
        }

        /// <summary>
        /// 創建第三方ProfitLossInfo表和索引
        /// </summary>
        public void TryCreateTableProfitLoss()
        {
            if (IsTableExists(ProfitlossTableName))
            {
                return;
            }

            string[] sqls = CreateProfitlossTableSqls;

            foreach (string sql in sqls)
            {
                SqliteDbHelper.Execute(sql, null);
            }
        }

        public void TryCreateTableLastSearchToken()
        {
            if (IsTableExists(_lastSearchKeyTableName))
            {
                return;
            }

            string sql = $@"
                CREATE TABLE {_lastSearchKeyTableName}(
                    Token NVARCHAR(100) NOT NULL
                )";
            SqliteDbHelper.Execute(sql, null);

            sql = $"INSERT INTO {_lastSearchKeyTableName} (Token) VALUES('');";
            SqliteDbHelper.Execute(sql, null);
        }

        public void SaveProfitloss<BetLogType>(List<BetLogType> betLogs) where BetLogType : BaseRemoteBetLog
        {
            foreach (BetLogType betLog in betLogs)
            {
                //檢查訂單是否存在
                if (IsOrderExist(betLog.KeyId))
                {
                    continue;
                }

                betLog.LocalSavedTime = DateTime.Now;
                betLog.RemoteSaved = 0;
                betLog.RemoteSavedTime = null;
                betLog.RemoteSaveTryCount = 0;
                betLog.RemoteSaveLastTryTime = null;

                InsertProfitlossToSqlite(betLog);
            }
        }

        protected virtual void InsertProfitlossToSqlite<BetLogType>(BetLogType betLog) where BetLogType : BaseRemoteBetLog
        {
            string insertSQL = ReflectUtil.GenerateInsertSQL<BetLogType>(ProfitlossTableName);
            SqliteDbHelper.Execute(insertSQL, betLog);
        }

        public List<BetLogType> GetBatchProfitlossNotSavedToRemote<BetLogType>() where BetLogType : BaseRemoteBetLog
        {
            string sql = $@"
                SELECT *
                FROM [{ProfitlossTableName}]
                WHERE RemoteSaved = 0 AND (RemoteSaveTryCount >= 0 AND RemoteSaveTryCount < 10)
                ORDER BY KeyId
                LIMIT 0, 100 ";

            return GetFormatBetLogs<BetLogType>(sql);
        }

        protected virtual List<BetLogType> GetFormatBetLogs<BetLogType>(string sql) where BetLogType : BaseRemoteBetLog
        {
            return SqliteDbHelper.QueryList<BetLogType>(sql, null);
        }

        public int SaveProfitlossToPlatformSuccess(string keyId)
        {
            string sql = $@"
                UPDATE [{ProfitlossTableName}]
                SET
                    RemoteSaved = 1,
                    RemoteSavedTime = datetime('now', 'localtime')
                WHERE KeyId = @KeyId ";
            return SqliteDbHelper.Execute(sql, new { keyId = keyId.ToNVarchar(100) });
        }

        public int SaveProfitlossToPlatformFail(string keyId)
        {
            string sql = $@"
                UPDATE [{ProfitlossTableName}]
                SET
                    RemoteSaveTryCount = RemoteSaveTryCount + 1,
                    RemoteSaveLastTryTime = datetime('now', 'localtime')
                WHERE KeyId = @KeyId ";
            return SqliteDbHelper.Execute(sql, new { keyId = keyId.ToNVarchar(100) });
        }

        public int SaveProfitlossToPlatformIgnore(string keyId)
        {
            string sql = $@"
                UPDATE [{ProfitlossTableName}]
                SET
                    RemoteSaveTryCount = 999,
                    RemoteSaveLastTryTime = datetime('now', 'localtime')
                WHERE KeyId = @KeyId ";
            return SqliteDbHelper.Execute(sql, new { keyId = keyId.ToNVarchar(100) });
        }

        /// <summary>
        /// 重置下一期搜尋token
        /// </summary>
        public int UpdateNextSearchToken(string newToken)
        {
            string sql = $@"
                UPDATE [{_lastSearchKeyTableName}]
                SET
                    Token = @newToken ";

            return SqliteDbHelper.Execute(sql, new { newToken = newToken.ToNVarchar(100) });
        }

        public string GetLastSearchToken()
        {
            string sql = $@"
                SELECT TOKEN
                FROM [{_lastSearchKeyTableName}]
                LIMIT 0,1 ";

            return SqliteDbHelper.ExecuteScalar<string>(sql, null);
        }

        /// <summary>
        /// 檢查訂單是否存在
        /// </summary>
        private bool IsOrderExist(string keyId)
        {
            string sql = $@"
                SELECT KeyId FROM [{ProfitlossTableName}]
                WHERE KeyId = @KeyId
                LIMIT 0,1 ";

            return !SqliteDbHelper.ExecuteScalar<string>(sql, new { keyId = keyId.ToNVarchar(100) }).IsNullOrEmpty();
        }

        public void DeleteExpiredProfitLoss()
        {
            while (true)
            {
                string deleteSql = $@"
                    DELETE FROM [{ProfitlossTableName}]
                    WHERE KeyId IN (
                        SELECT KeyId FROM [{ProfitlossTableName}]
                        WHERE
                            localSavedTime < @limitMaxSavedTime
                            AND (RemoteSaved = 1 OR RemoteSaveTryCount >= 10)
                        ORDER BY KeyId LIMIT 300) ";

                DateTime limitMaxSavedTime = DateTime.Now.AddMonths(-1);
                int deleteCount = SqliteDbHelper.Execute(deleteSql, new { limitMaxSavedTime });

                LogUtilService.ForcedDebug($"从 SqlLite 数据库中删除{ProfitlossTableName} {deleteCount} 条过期亏赢数据");

                if (deleteCount == 0)
                {
                    break;
                }

                Thread.Sleep(2000);
            }
        }
    }

    public abstract class BaseTransferJsonFormatSqlLiteRepository<BTISBetLog> : BaseTransferSqlLiteRepository<BTISBetLog>
    {
        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId TEXT PRIMARY KEY,           --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount TEXT,               --抽象欄位, 第三方帳號名稱,
                 Memo TEXT,
                 BetLogJson TEXT,
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)"
        };

        protected override void InsertProfitlossToSqlite<BetLogType>(BetLogType betLog)
        {
            string sql = $@"
                INSERT INTO {ProfitlossTableName} (
                    KeyId,
                    TPGameAccount,
                    Memo,
                    BetLogJson,
                    LocalSavedTime,
                    RemoteSaved,
                    RemoteSavedTime,
                    RemoteSaveTryCount,
                    RemoteSaveLastTryTime)
                VALUES(
                    @KeyId,
                    @TPGameAccount,
                    @Memo,
                    @BetLogJson,
                    @LocalSavedTime,
                    @RemoteSaved,
                    @RemoteSavedTime,
                    @RemoteSaveTryCount,
                    @RemoteSaveLastTryTime);";

            object param = new
            {
                betLog.KeyId,
                betLog.TPGameAccount,
                betLog.Memo,
                BetLogJson = betLog.ToJsonString(ignoreNull: true, ignoreDefault: true),
                betLog.LocalSavedTime,
                betLog.RemoteSaved,
                betLog.RemoteSavedTime,
                betLog.RemoteSaveTryCount,
                betLog.RemoteSaveLastTryTime
            };

            SqliteDbHelper.Execute(sql, param);
        }

        protected override List<BetLogType> GetFormatBetLogs<BetLogType>(string sql)
        {
            List<JsonRemoteBetLog> jsonRemoteBetLogs = SqliteDbHelper.QueryList<JsonRemoteBetLog>(sql, null);

            var returnList = jsonRemoteBetLogs.Select(s =>
            {
                var betLog = s.BetLogJson.Deserialize<BetLogType>();
                betLog.Memo = s.Memo;
                return betLog;
            }).ToList();

            return returnList;
        }
    }
}