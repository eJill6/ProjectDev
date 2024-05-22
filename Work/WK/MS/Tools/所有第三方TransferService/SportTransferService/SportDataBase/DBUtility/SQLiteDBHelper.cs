using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using SportDataBase.Common;

namespace Maticsoft.DBUtility
{
    public static class SQLiteDBHelper
    {
        private static string dbPassword = "";//"F@lix123";

        public static void CreateDataBase(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
            //ChangePassword(dbPath, dbPassword);
        }

        /// <summary>
        /// 创建SportProfitLossInfo表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateSportProfitLossInfo(string dbPath)
        {
            string sql = @"CREATE TABLE SportProfitLossInfo(
                                    Id INTEGER PRIMARY KEY,
                                    trans_id NVARCHAR(100) NOT NULL,
                                    sport_type NVARCHAR(100)  NULL,
                                    after_amount decimal(18,4) NOT NULL,
                                    away_id NVARCHAR(100)  NULL,
                                    away_id_text NVARCHAR(100)  NULL,
                                    bet_type NVARCHAR(100)  NULL,
                                    bet_type_text NVARCHAR(100)  NULL,
                                    currency NVARCHAR(20)  NULL,
                                    home_id NVARCHAR(100)  NULL,
                                    home_id_text NVARCHAR(100)  NULL,
                                    league_id NVARCHAR(100) NULL,
                                    league_id_text NVARCHAR(100) NULL,
                                    match_datetime  NVARCHAR(100)  NULL,
                                    winlost_datetime NVARCHAR(100)  NULL,
                                    transaction_time NVARCHAR(100) NOT NULL,
                                    match_id NVARCHAR(100)  NULL,
                                    match_id_text NVARCHAR(100)  NULL,
                                    Odds_Type NVARCHAR(20) NULL,
                                    odds NVARCHAR(100)  NULL,
                                    stake decimal(18,4)  NULL,
                                    ticket_status NVARCHAR(100)  NULL,
                                    version_key NVARCHAR(100)  NULL,
                                    winlost_amount  decimal(18,4)  NULL,
                                    vendor_member_id  version_key NVARCHAR(100) NOT NULL,
                                    Memo NVARCHAR(500)  NULL,
                                    localSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                                    remoteSaved INTEGER default 0 NOT NULL,
                                    remoteSavedTime TIMESTAMP NULL,
                                    remoteSaveTryCount INTEGER default 0 NOT NULL,
                                    remoteSaveLastTryTime TIMESTAMP NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE UNIQUE INDEX SportProfitLossInfo_idx_trans_id ON SportProfitLossInfo(trans_id ASC);";
            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 创建SportProfitLossInfo表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateVersionKeyTD(string dbPath)
        {
            string sql = @"CREATE TABLE VersionKeyTD(
                                    Id INTEGER PRIMARY KEY,
                                    version_key NVARCHAR(100) NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"insert into VersionKeyTD (version_key) values('0')";
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
                        affectedRows = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }

        public static int BatchUpdate(string dbPath, string sql, List<SQLiteParameter[]> parameters)
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
                        command.CommandTimeout = 30;

                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddRange(parameter);
                            affectedRows = command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteReader(string dbPath, string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(GetConnectionString(dbPath, dbPassword));
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.CommandTimeout = 2;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
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
                    adapter.Fill(data);
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
                    adapter.Fill(data);

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
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            //这里表名可以是Sqlite_master
            String sql = "select count(*) as c from sqlite_master where type ='table' and name ='" + tableName.Trim() + "' ";

            int count = Convert.ToInt32(ExecuteScalar(dbPath, sql, null));

            if (count > 0)
            {
                result = true;
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
            if (string.IsNullOrEmpty(password))
                return "Data Source=" + path;
            return "Data Source=" + path + ";Password=" + password;
        }
    }
}
