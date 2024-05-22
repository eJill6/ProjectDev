using System.Data.SQLite;
using System.Data.Common;
using System.Data;

namespace ProductTransferService.AgDataBase.DBUtility
{
    public static class SQLiteDBHelper
    {
        private static readonly string dbPassword = "";

        private static readonly object s_writeLockObj = new object();

        public static object WriteLockobj => s_writeLockObj;

        public static void CreateDataBase(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
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

                lock (WriteLockobj)
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

        private static bool ChangePassword(string dbPath, string newPassword, string oldPassword = null)
        {
            try
            {
                using (var connection = new SQLiteConnection(GetConnectionString(dbPath, oldPassword)))
                {
                    connection.Open();
                    //connection.ChangePassword(newPassword);

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT quote($newPassword);";
                    command.Parameters.AddWithValue("$newPassword", newPassword);
                    var quotedNewPassword = (string)command.ExecuteScalar();
                    command.CommandText = "PRAGMA rekey =" + quotedNewPassword;
                    command.Parameters.Clear();
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static SQLiteConnection GetSQLiteConnection(string dbPath)=> new SQLiteConnection(GetConnectionString(dbPath, dbPassword));
        
        private static string GetConnectionString(string path, string password)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder("Data Source=" + path)
            {
                JournalMode = SQLiteJournalModeEnum.Wal
            };

            if (!string.IsNullOrEmpty(password))
            {
                builder.Password = password;
            }

            return builder.ToString();

            //if (string.IsNullOrEmpty(password))
            //{
            //    return "Data Source=" + path + ";Journal Mode=WAL;";
            //}

            //return "Data Source=" + path + ";Password=" + password+";Journal Mode=WAL;";
        }
    }
}