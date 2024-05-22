using IMeBetDataBase.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMeBetDataBase.DBUtility
{
    public static class SQLiteDBHelper
    {
        private static string dbPassword = "";//"F@lix123";

        static object lockObj = new object();

        public static void CreateDataBase(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
            //ChangePassword(dbPath, dbPassword);
        }

        /// <summary>
        /// 创建IMeBetProfitLossInfo表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateIMeBetProfitLossInfo(string dbPath)
        {
            string sql = @"
                CREATE TABLE IMeBetProfitLossInfo(
                    Id INTEGER PRIMARY KEY,     
                    UserID NVARCHAR(50) NULL,     
                    Provider INTEGER NULL,
                    GameId NVARCHAR(50) NULL,  
                    GameName NVARCHAR(50) NULL, 
                    ChineseGameName NVARCHAR(50) NULL, 
                    BetType NVARCHAR(50) NULL, 
                    BetId NVARCHAR(50) NULL,     
                    ExternalBetId NVARCHAR(50) NULL, 
                    RoundId NVARCHAR(50) NULL, 
                    PlayerId NVARCHAR(50) NULL,
                    ProviderPlayerId NVARCHAR(50) NULL,
                    Currency NVARCHAR(20) NULL, 
                    BetAmount NVARCHAR(50) NULL,
                    ValidBet NVARCHAR(50) NULL,
                    Tips NVARCHAR(50) NULL,
                    WinLoss NVARCHAR(50) NULL,
                    ProviderBonus NVARCHAR(50) NULL,
                    ProviderTourFee NVARCHAR(50) NULL,
                    ProviderTourRefund NVARCHAR(50) NULL,
                    Status NVARCHAR(20) NULL,
                    Platform NVARCHAR(20) NULL,
                    BetDate NVARCHAR(100) NULL,
                    ReportingDate NVARCHAR(100) NULL,
                    DateCreated NVARCHAR(100) NULL,
                    LastUpdatedDate NVARCHAR(100) NULL,              
                    Memo NVARCHAR(500) NULL,
                    localSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                    remoteSaved INTEGER default 0 NOT NULL,
                    remoteSavedTime TIMESTAMP NULL,
                    remoteSaveTryCount INTEGER default 0 NOT NULL,
                    remoteSaveLastTryTime TIMESTAMP NULL
                )";
            
            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE UNIQUE INDEX IMeBetProfitLossInfo_idx_BetId ON IMeBetProfitLossInfo(BetId ASC);";

            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 创建IMeBetProfitLossInfo表
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

        /// <summary>
        /// 查询数据库中的所有数据类型信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSchema(string dbPath)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString(dbPath, dbPassword)))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                //foreach (DataColumn column in data.Columns)
                //{
                //    Console.WriteLine(column.ColumnName);
                //}
                return data;
            }
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
