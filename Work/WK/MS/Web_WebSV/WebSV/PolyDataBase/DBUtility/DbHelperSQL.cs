using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace SLPolyGame.Web.DBUtility
{
    /// <summary>
    /// 数据访问抽象基础类
    /// Copyright (C) 2004-2008 By LiTianPing
    /// </summary>
    public abstract class DbHelperSQL
    {
        /// <summary>
        /// 数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.
        /// </summary>
        private static string connectionString = PubConstant.ConnectionString;

        public static SqlDbHelper Main { get; set; }

        public static SqlDbHelper Bak { get; set; }

        public static SqlDbHelper Login { get; set; }

        static DbHelperSQL()
        {
            Main = new SqlDbHelper(PubConstant.ConnectionString);
            Bak = new SqlDbHelper(PubConstant.ConnectionString_Bak);
            Login = new SqlDbHelper(PubConstant.ConnectionString_Login);
        }

        #region 公用方法

        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        [Obsolete]
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [Obsolete]
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object obj = GetSingle(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion 公用方法

        #region 执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        [Obsolete]
        public static int ExecuteSql(string SQLString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SQLString, connection);

            try
            {
                connection.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        [Obsolete]
        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SQLString, connection);

            try
            {
                connection.Open();
                cmd.CommandTimeout = Times;
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        [Obsolete]
        public static object GetSingle(string SQLString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SQLString, connection);

            try
            {
                connection.Open();
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        [Obsolete]
        public static DataSet Query(string SQLString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                command.Fill(ds, "ds");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        [Obsolete]
        public static DataSet Query(string SQLString, int timeoutSeconds)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                command.SelectCommand.CommandTimeout = timeoutSeconds;
                command.Fill(ds, "ds");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ds;
        }

        #endregion 执行简单SQL语句

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        [Obsolete]
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        [Obsolete]
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        [Obsolete]
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 180;
            PrepareCommand(cmd, connection, null, SQLString, cmdParms);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ds;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        [Obsolete]
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion 执行带参数的SQL语句

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        [Obsolete]
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.SelectCommand.CommandTimeout = 180;
            try
            {
                sqlDA.Fill(dataSet, tableName);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return dataSet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete]
        public static int ExecuteNonQuery(string spName, IDataParameter[] parameters)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var sqlCommand = new SqlCommand(spName, connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddRange(parameters);

            try
            {
                return sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        [Obsolete]
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        #endregion 存储过程操作
    }
}