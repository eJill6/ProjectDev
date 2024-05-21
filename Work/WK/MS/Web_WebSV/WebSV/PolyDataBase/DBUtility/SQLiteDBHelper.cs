using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.Threading;

namespace PolyDataBase.DBUtility
{
    public static class SQLiteDBHelper
    {
        private static string dbPassword = "F@lix123";

        public static void CreateDataBase(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
            ChangePassword(dbPath, dbPassword);
        }

        /// <summary>
        /// 创建IpData表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateIpDataTable(string dbPath)
        {
            string sql = @"CREATE TABLE IpData(
	                                StartIP VARCHAR(16) NOT NULL,
	                                SartIPNum INT64 NOT NULL,
	                                EndIP VARCHAR(16) NOT NULL,
	                                EndIPNum INT64 NOT NULL,
	                                Area NVARCHAR(128) NOT NULL,
	                                Remark NVARCHAR(512) NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE INDEX IpData_idx ON IpData(SartIPNum ASC,EndIPNum ASC);";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE WhiteIpList(
	                        Ip varchar(16) NOT NULL,
	                        Remark nvarchar(200) NOT NULL
                        )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE BlackIpList(
	                        Ip varchar(16) NOT NULL,
	                        Remark nvarchar(200) NOT NULL
                        )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE BlackIpRangeList(
	                        StartIp varchar(16) NOT NULL,
	                        StartIpNum INT64 NOT NULL,
	                        EndIp varchar(16) NOT NULL,
	                        EndIpNum INT64 NOT NULL,
	                        Remark nvarchar(200) NOT NULL
                        )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE BlackAreaList(
	                        Area nvarchar(128) NOT NULL,
	                        Remark nvarchar(200) NOT NULL
                        )";
            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 创建IpData表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateAccountDataTable(string dbPath)
        {
            string sql = @"CREATE TABLE FailureLoginHistory(
	                                UserName NVARCHAR(50) NOT NULL,
	                                LoginTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
	                                LoginIp VARCHAR(16) NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE INDEX FailureLoginHistory_idx_UserName_LoginTime ON FailureLoginHistory(UserName ASC,LoginTime ASC);";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE FreezeLoginHistory(
	                                UserName NVARCHAR(50) NOT NULL,
	                                FreezeTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            //------------------------------------------------------------------------------------

            sql = @"CREATE TABLE FailureWithdrawHistory(
	                                UserName NVARCHAR(50) NOT NULL,
	                                WithdrawTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
	                                WithdrawIp VARCHAR(16) NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);

            sql = "CREATE INDEX FailureWithdrawHistory_idx_UserName_WithdrawTime ON FailureWithdrawHistory(UserName ASC,WithdrawTime ASC);";
            ExecuteNonQuery(dbPath, sql, null);

            sql = @"CREATE TABLE FreezeWithdrawHistory(
	                                UserName NVARCHAR(50) NOT NULL,
	                                FreezeTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL
                                )";
            ExecuteNonQuery(dbPath, sql, null);
        }

        /// <summary>
        /// 创建Email表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public static void CreateEmailDataTable(string dbPath)
        {
            string sql = @"CREATE TABLE Message(
                                    id INTEGER PRIMARY KEY,
                                    address NVARCHAR(200) NOT NULL,
	                                title NVARCHAR(200) NOT NULL,
                                    content TEXT NOT NULL,
	                                savetime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                                    sendtime TIMESTAMP NULL 
                                )";
            ExecuteNonQuery(dbPath, sql, null);
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


        private static bool ChangePassword(string dbPath, string newPassword, string oldPassword = null)
        {
            try
            {
                var con = new SQLiteConnection(GetConnectionString(dbPath, oldPassword));
                con.Open();
                con.ChangePassword(newPassword);
                con.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static string GetConnectionString(string path, string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Data Source=" + path+";Journal Mode=WAL;";
            return "Data Source=" + path + ";Password=" + password + ";Journal Mode=WAL;";
        }
    }
}
