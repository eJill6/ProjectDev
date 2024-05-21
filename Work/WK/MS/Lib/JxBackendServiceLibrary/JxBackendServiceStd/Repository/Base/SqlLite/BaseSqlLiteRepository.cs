using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using System;

namespace JxBackendService.Repository.Base.SqlLite
{
    public abstract class BaseSqlLiteRepository
    {
        private readonly Lazy<SqliteDbHelperSQL> _sqliteDbHelper;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private static readonly string _defaultDbPassword = string.Empty;

        protected abstract string DbFullPath { get; }

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected virtual SqliteDbHelperSQL SqliteDbHelper => _sqliteDbHelper.Value;

        private static readonly object s_locker = new object();

        protected static object Locker => s_locker;

        protected static string DefaultDbPassword => _defaultDbPassword;

        public BaseSqlLiteRepository()
        {
            _sqliteDbHelper = new Lazy<SqliteDbHelperSQL>(() =>
            {
                return new SqliteDbHelperSQL(GetConnectionString(DbFullPath, _defaultDbPassword));
            });

            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        protected bool IsTableExists(string tableName)
        {
            bool isExists = false;

            if (!string.IsNullOrEmpty(tableName))
            {
                //这里表名可以是Sqlite_master
                string sql = "select count(*) as cnt from sqlite_master where type ='table' and name = @tableName ";

                lock (s_locker)
                {
                    int count = SqliteDbHelper.ExecuteScalar<int>(sql, new { tableName = tableName.Trim() });
                    isExists = (count > 0);
                }
            }

            return isExists;
        }

        protected string GetConnectionString(string dbFullPath, string dbPassword)
        {
            string connStr = "Data Source=" + dbFullPath;

            if (!string.IsNullOrEmpty(dbPassword))
            {
                connStr += ";Password=" + dbPassword;
            }

            return connStr;
        }
    }
}