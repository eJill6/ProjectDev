using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Util.Export
{
    public class ExportFullResultParam
    {
        public string PageTitle { get; set; }

        public List<string> PageAdditionalData { get; set; } = new List<string>();

        public ExportQueryResultParam PageGrid { get; set; } = new ExportQueryResultParam();
    }

    public class ExportQueryResultParam
    {
        public Type QueryResultModelType { get; set; }

        public List<object> QueryResult { get; set; } = new List<object>();

        public int QueryResultLimitCount => 100000;

        public bool HasResultExceedLimit { get; set; }
    }
}