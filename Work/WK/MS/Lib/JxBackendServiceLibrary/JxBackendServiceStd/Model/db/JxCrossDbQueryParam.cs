using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.db
{
    public class JxCrossDbQueryParam : BasePagedParamsModel
    {
        private string _summaryTempTableOrderBy;

        public string BasicTableName { get; set; }

        public List<SqlSelectColumnInfo> SelectColumnInfos { get; set; }

        public string StatColumns { get; set; }

        public string StatGroupByColumns { get; set; }

        public string InlodbFilters { get; set; }

        public string InlodbBakFilters { get; set; }

        public object Parameters { get; set; }

        public string OrderBy { get; set; }

        /// <summary>針對cross db放入的temp表做的排序, 若沒有資料則以OrderBy為主</summary>
        public string SummaryTempTableOrderBy
        {
            get
            {
                if (_summaryTempTableOrderBy.IsNullOrEmpty())
                {
                    return OrderBy;
                }

                return _summaryTempTableOrderBy;
            }

            set => _summaryTempTableOrderBy = value;
        }

        public string GetFilter(InlodbType inlodbType)
        {
            if (inlodbType == InlodbType.Inlodb)
            {
                return InlodbFilters;
            }
            else if (inlodbType == InlodbType.InlodbBak)
            {
                return InlodbBakFilters;
            }

            throw new NotImplementedException();
        }
    }
}