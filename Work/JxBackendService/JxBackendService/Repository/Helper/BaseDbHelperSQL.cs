using Dapper;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.Paging;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseDbHelperSQL
    {
        private readonly string _connectionString;

        public BaseDbHelperSQL(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected abstract Type SqlConnectionType { get; }

        public IDbConnection GetSqlConnection()
        {
            IDbConnection dbConnection = (IDbConnection)Activator.CreateInstance(SqlConnectionType, _connectionString);
            return dbConnection;
        }

        public int Execute(string sql, object param)
        {
            return Execute(sql, param, CommandType.Text);
        }

        public int Execute(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.Execute(sql, param, commandType: cmdType);
            }
        }

        public List<T> QueryList<T>(string sql, object param)
        {
            return QueryList<T>(sql, param, CommandType.Text);
        }

        public List<T> QueryList<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.Query<T>(sql, param, commandType: cmdType).ToList();
            }
        }

        public T QueryFirst<T>(string sql, object param)
        {
            return QueryFirst<T>(sql, param, CommandType.Text);
        }

        public T QueryFirst<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.QueryFirst<T>(sql, param, commandType: cmdType);
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            return QueryFirstOrDefault<T>(sql, param, CommandType.Text);
        }

        public T QueryFirstOrDefault<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, param, commandType: cmdType);
            }
        }

        public T QuerySingle<T>(string sql, object param)
        {
            return QuerySingle<T>(sql, param, CommandType.Text);
        }

        public T QuerySingle<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.QuerySingle<T>(sql, param, commandType: cmdType);
            }
        }

        public T QuerySingleOrDefault<T>(string sql, object param)
        {
            return QuerySingleOrDefault<T>(sql, param, CommandType.Text);
        }

        public T QuerySingleOrDefault<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.QuerySingleOrDefault<T>(sql, param, commandType: cmdType);
            }
        }

        public T ExecuteScalar<T>(string sql, object param)
        {
            return ExecuteScalar<T>(sql, param, CommandType.Text);
        }

        public T ExecuteScalar<T>(string sql, object param, CommandType cmdType)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.ExecuteScalar<T>(sql, param, commandType: cmdType);
            }
        }

    }
}
