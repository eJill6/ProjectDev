using Dapper;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base.SqlLite
{
    public class BaseSqlLiteRepository
    {
        private static readonly string _dbPassword = string.Empty;
        
        protected string DbFullPath { get; set; }

        private Lazy<SqliteDbHelperSQL> _sqliteDbHelper;

        protected SqliteDbHelperSQL SqliteDbHelper => _sqliteDbHelper.Value;

        public BaseSqlLiteRepository()
        {
            _sqliteDbHelper = new Lazy<SqliteDbHelperSQL>(() =>
            {
                return new SqliteDbHelperSQL(GetConnectionString());
            });
        }

        protected bool IsTableExists(string tableName)
        {
            bool isExists = false;
            if (!string.IsNullOrEmpty(tableName))
            {
                //这里表名可以是Sqlite_master
                string sql = "select count(*) as cnt from sqlite_master where type ='table' and name = @tableName ";
                int count = SqliteDbHelper.ExecuteScalar<int>(sql, new { tableName = tableName.Trim() });
                isExists = (count > 0);
            }

            return isExists;
        }

        private string GetConnectionString()
        {
            string connStr = "Data Source=" + DbFullPath;

            if (!string.IsNullOrEmpty(_dbPassword))
            {
                connStr += ";Password=" + _dbPassword;
            }

            return connStr;
        }
    }
}
