using Dapper;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.Paging;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base
{
    public class SqliteDbHelperSQL : BaseDbHelperSQL
    {
        public SqliteDbHelperSQL(string connectionString) : base(connectionString)
        {

        }

        protected override Type SqlConnectionType => typeof(SQLiteConnection);
    }
}
