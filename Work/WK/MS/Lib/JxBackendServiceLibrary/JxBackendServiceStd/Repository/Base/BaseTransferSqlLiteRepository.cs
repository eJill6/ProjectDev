using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
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
    public abstract class BaseTransferSqlLiteRepository : BaseSqlLiteRepository, ITransferSqlLiteRepository
    {
        private readonly string _lastSearchKeyTableName = "LastSearchToken";

        private readonly string dbFullPath;

        #region abstract methods

        public abstract PlatformProduct Product { get; }

        #endregion abstract methods

        public BaseTransferSqlLiteRepository() : base()
        {
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            dbFullPath = Path.Combine(baseDirectoryPath, $"{Product.Value}_data.db");
        }

        protected override string DbFullPath => dbFullPath;

        public void InitSettings()
        {
            TryCreateDataBase();
            TryCreateTableLastSearchToken();
        }

        private void TryCreateDataBase()
        {
            if (!File.Exists(DbFullPath))
            {
                SQLiteConnection.CreateFile(DbFullPath);
            }
        }

        private void TryCreateTableLastSearchToken()
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

        protected virtual List<BetLogType> GetFormatBetLogs<BetLogType>(string sql) where BetLogType : BaseRemoteBetLog
        {
            lock (Locker)
            {
                return SqliteDbHelper.QueryList<BetLogType>(sql, null);
            }
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

            lock (Locker)
            {
                return SqliteDbHelper.Execute(sql, new { newToken = newToken.ToNVarchar(100) });
            }
        }

        public string GetLastSearchToken()
        {
            string sql = $@"
                SELECT TOKEN
                FROM [{_lastSearchKeyTableName}]
                LIMIT 0,1 ";

            lock (Locker)
            {
                return SqliteDbHelper.ExecuteScalar<string>(sql, null);
            }
        }
    }

    public abstract class BaseTransferJsonFormatSqlLiteRepository : BaseTransferSqlLiteRepository
    {
        protected override List<BetLogType> GetFormatBetLogs<BetLogType>(string sql)
        {
            List<JsonRemoteBetLog> jsonRemoteBetLogs;

            lock (Locker)
            {
                jsonRemoteBetLogs = SqliteDbHelper.QueryList<JsonRemoteBetLog>(sql, null);
            }

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