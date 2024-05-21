using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.db
{
    public class BuildSqlParam : PagerInfo
    {
        public string TableName { get; set; }
    }
}
