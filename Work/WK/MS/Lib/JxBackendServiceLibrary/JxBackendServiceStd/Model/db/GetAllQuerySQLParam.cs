using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Model.db
{
    public class GetAllQuerySQLParam
    {
        public InlodbType DbType { get; set; }
        public int? TopRow { get; set; }
        public string TableName { get; set; }
        public bool IsAppendTableNameToColumn { get; set; }
        public List<string> ColumnNameFilters { get; set; }
    }
}
