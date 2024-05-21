using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Model.Paging
{
    public class BuildPagedSqlQueryParam
    {
        public InlodbType InlodbType { get; set; }

        public string TableName { get; set; }

        public string TableAlias { get; set; }

        public List<string> Properties { get; set; }

        public string WhereString { get; set; }

        public object Parameters { get; set; }

        public BasePagingRequestParam RequestParam { get; set; }
    }
}
