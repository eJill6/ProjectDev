using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.db
{
    public class JxCrossDbQueryParam : BasePagedParamsModel
    {
        public string BasicTableName { get; set; }

        public List<SqlSelectColumnInfo> SelectColumnInfos { get; set; }

        public string StatColumns { get; set; }

        public string StatGroupByColumns { get; set; }

        public string InlodbFilters { get; set; }

        public string InlodbBakFilters { get; set; }

        public object Parameters { get; set; }

        public string OrderBy { get; set; }        

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
