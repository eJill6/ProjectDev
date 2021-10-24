using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Paging
{
    public class SortModel
    {
        public SortModel() { }

        public SortModel(string orderByColumnAndSort)
        {
            if (orderByColumnAndSort.IsNullOrEmpty())
            {
                return;
            }

            string[] orderByDatas=  orderByColumnAndSort.Split(' ');

            if(orderByDatas.Length > 0)
            {
                ColumnName = orderByDatas[0];

                if (orderByDatas.Length > 1)
                {
                    SortOrderText = orderByDatas[1];
                }
            }
        }

        public string ColumnName { get; set; }
        public SortOrder Sort { get; set; }

        public string SortOrderText
        {
            get
            {
                switch (Sort)
                {
                    case SortOrder.Ascending:
                        return "asc";
                    case SortOrder.Descending:
                        return "desc";
                }

                return string.Empty;
            }
            set
            {
                if (value.ToTrimString().Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    Sort = SortOrder.Descending;
                }
                else
                {
                    Sort = SortOrder.Ascending;
                }
            }
        }        
    }
}
