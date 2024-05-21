using System;
using System.Data.SQLite;

namespace JxBackendService.Repository.Base
{
    public class SqliteDbHelperSQL : BaseDbHelperSQL
    {
        public static int QueryInMaxParameterCount => 2000;

        public SqliteDbHelperSQL(string connectionString) : base(connectionString)
        {

        }

        protected override Type SqlConnectionType => typeof(SQLiteConnection);
    }
}
