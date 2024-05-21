using Dapper;
using JxBackendService.Model.Enums;
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
            return Execute(sql, param, cmdType, null);
        }

        public int Execute(string sql, object param, CommandType cmdType, int? commandTimeout)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.Execute(sql, param, commandType: cmdType, commandTimeout: commandTimeout);
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

        public T ExecuteReader<T>(string sql, object param, CommandType cmdType, Func<IDataReader, T> doWork)
        {
            param = param.ConvertValidDynamicParameters();

            using (IDbConnection connection = GetSqlConnection())
            {
                IDataReader dataReader = connection.ExecuteReader(sql, param, commandType: cmdType);

                return doWork.Invoke(dataReader);
            }
        }

        public void BulkCopy(DataTable dataTable, SqlBulkCopyOptions sqlBulkCopyOption)
        {
            using (IDbConnection connection = GetSqlConnection())
            {
                if(connection is SqlConnection == false)
                {
                    throw new NotSupportedException();
                }

                connection.Open();

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection as SqlConnection, sqlBulkCopyOption, null))
                {
                    sqlBulkCopy.BulkCopyTimeout = 600;
                    sqlBulkCopy.BatchSize = 500;
                    sqlBulkCopy.DestinationTableName = dataTable.TableName;
                    
                    //對應資料行
                    for(int i = 0; i< dataTable.Columns.Count; i++)
                    {
                        DataColumn column = dataTable.Columns[i];
                        sqlBulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    
                    sqlBulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
