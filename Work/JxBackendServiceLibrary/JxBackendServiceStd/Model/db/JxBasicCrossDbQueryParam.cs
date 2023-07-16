using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.db
{
    public class JxBasicCrossDbQueryParam<T>
    {
        public string FullTableName { get; set; }
        public string Filters { get; set; }

        public T DataModel { get; set; }
    }

    public class JxBasicCrossDbQueryParam : JxBasicCrossDbQueryParam<object>
    {

    }
}