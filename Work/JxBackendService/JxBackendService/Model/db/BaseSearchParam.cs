using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.db
{
    public class BaseSearchParam
    {
        public string Sql { get; set; }
        public object Parameters { get; set; }
    }
}
