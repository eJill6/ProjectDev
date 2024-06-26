﻿using IMSportsbookDataBase.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;

namespace IMSportsbookDataBase.DBUtility
{
    public static class SQLiteDBHelper
    {
        private static string dbPassword = "";//"F@lix123";

        private static object lockObj = new object();

        public static void CreateDataBase(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
            //ChangePassword(dbPath, dbPassword);
        }

        /// <summary>
        /// 创建IMSportsbookProfitLossInfo表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateIMSportsbookProfitLossInfo(string dbPath)
        {
            string sql = @"
                CREATE TABLE IMSportsbookProfitLossInfo(
                    Id INTEGER PRIMARY KEY,
                    UserID NVARCHAR(50) NULL,
                    Provider INTEGER NULL,
                    GameId NVARCHAR(50) NULL,
                    BetId NVARCHAR(50) NULL,
                    WagerCreationDateTime NVARCHAR(100) NULL,
                    PlayerId NVARCHAR(50) NULL,
                    ProviderPlayerId NVARCHAR(50) NULL,
                    Currency NVARCHAR(50) NULL,
                    StakeAmount NVARCHAR(50) NULL,
                    MemberExposure NVARCHAR(50) NULL,
                    PayoutAmount NVARCHAR(50) NULL,
                    WinLoss NVARCHAR(50) NULL,
                    OddsType NVARCHAR(50) NULL,
                    WagerType NVARCHAR(50) NULL,
                    Platform NVARCHAR(50) NULL,
                    IsSettled INTEGER NULL,
                    IsConfirmed INTEGER NULL,
                    IsCancelled INTEGER NULL,
                    BetTradeStatus NVARCHAR(50) NULL,
                    BetTradeCommission NVARCHAR(50) NULL,
                    BetTradeBuybackAmount  NVARCHAR(50) NULL,
                    ComboType NVARCHAR(50) NULL,
                    LastUpdatedDate NVARCHAR(100) NULL,
                    DetailItems NVARCHAR(50) NULL,
                    Memo NVARCHAR(500) NULL,
                    localSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                    remoteSaved INTEGER default 0 NOT NULL,
                    remoteSavedTime TIMESTAMP NULL,
                    remoteSaveTryCount INTEGER default 0 NOT NULL,
                    remoteSaveLastTryTime TIMESTAMP NULL
                )";

            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE UNIQUE INDEX IMSportsbookProfitLossInfo_idx_BetId ON IMSportsbookProfitLossInfo(BetId ASC);";

            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 创建IMSportsbookProfitLossInfo表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateSearchLastTimeTable(string dbPath)
        {
            string sql = @"
                CREATE TABLE LastTimeTable(
                    Id INTEGER PRIMARY KEY,
                    LastSearchTime NVARCHAR(100) NOT NULL
                )";

            ExecuteNonQuery(dbPath, sql, null);

            sql = @"INSERT INTO LastTimeTable (LastSearchTime)
                    VALUES ('" + Utility.ConvertToUnixOfTime(DateTime.Now) + "')";

            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。
        /// </summary>
        /// <param name="sql">要执行的增删改的SQL语句</param>
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string dbPath, string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(dbPath, dbPassword)))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        command.CommandTimeout = 2;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        lock (lockObj)
                        {
                            affectedRows = command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string dbPath, string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(dbPath, dbPassword)))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.CommandTimeout = 2;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    lock (lockObj)
                    {
                        adapter.Fill(data);
                    }
                    return data;
                }
            }
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的第一行第一列
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>
        /// <returns></returns>
        public static Object ExecuteScalar(string dbPath, string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(dbPath, dbPassword)))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.CommandTimeout = 2;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();

                    lock (lockObj)
                    {
                        adapter.Fill(data);
                    }

                    if (data.Rows.Count > 0)
                    {
                        return data.Rows[0][0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static bool TableIsExist(string dbPath, String tableName)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(tableName))
            {
                //这里表名可以是Sqlite_master
                string sql = "select count(*) as c from sqlite_master where type ='table' and name ='" + tableName.Trim() + "' ";

                int count = Convert.ToInt32(ExecuteScalar(dbPath, sql, null));

                result = (count > 0);
            }

            return result;
        }

        public static void AddColumnNX(string dbPath, string tableName, string columnName, string columnDataType, bool isNullable = true)
        {
            DataTable columns = GetTableInfo(dbPath, tableName);

            bool hasColumn = columns
                .AsEnumerable()
                .Any(row => row.Field<string>("Name")
                .Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (!hasColumn)
            {
                AddColumn(dbPath, tableName, columnName, columnDataType, isNullable);
            }
        }

        /// <summary>
        /// 取得 Table 資訊
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetTableInfo(string dbPath, string tableName)
        {
            return ExecuteDataTable(
                dbPath,
                sql: string.Format("PRAGMA table_info({0})", tableName),
                parameters: null
            );
        }

        /// <summary>
        /// 新增表格欄位
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnDataType"></param>
        /// <param name="isNullable"></param>
        public static void AddColumn(string dbPath, string tableName, string columnName, string columnDataType, bool isNullable)
        {
            var sql = string.Format(
                "ALTER TABLE {0} ADD COLUMN {1} {2} {3}",
                tableName,
                columnName,
                columnDataType,
                isNullable ? "NULL" : "NOT NULL");

            ExecuteNonQuery(dbPath, sql, null);
        }

        private static string GetConnectionString(string path, string password)
        {
            string connStr = "Data Source=" + path;
            if (!string.IsNullOrEmpty(password))
            {
                connStr = "Data Source=" + path + ";Password=" + password;
            }
            return connStr;
        }
    }
}