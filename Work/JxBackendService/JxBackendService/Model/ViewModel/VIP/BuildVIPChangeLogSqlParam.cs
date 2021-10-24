using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class BuildVIPChangeLogSqlParam
    {
        public InlodbType InlodbType { get; set; }
     
        public string DataSourceCode { get; set; }
        
        public string DataSourceName { get; set; }
        
        public string TableName { get; set; }
        
        public int PageNo { get; set; }
        
        public int PageSize { get; set; }
        
        public bool IsQueryCount { get; set; }

        public string AddtionalColumns { get; set; }
    }
}
