using JxBackendService.Common.Extensions;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Paging
{
    public class MultiplePagedSqlQueryParam : BasePagedParamsModel
    {
        public List<SingleTableQueryParam> SingleTableQueryParams = new List<SingleTableQueryParam>();

        public object Parameters { get; set; }
    }

    public class SingleTableQueryParam
    {
        public List<SqlSelectColumnInfo> SelectColumnInfos { get; set; }

        public string FullTableName { get; set; }

        public string Filters { get; set; }

        public string OrderBy { get; set; }

        /// <summary>
        /// 統計欄位的SQL
        /// </summary>
        public string StatColumns { get; set; }
        
        public string StatGroupByColumns { get; internal set; }
    }
}