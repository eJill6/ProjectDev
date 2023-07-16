using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Model.Paging
{
    public class SortModel
    {
        public SortModel()
        { }

        public SortModel(string orderByColumnAndSort)
        {
            if (orderByColumnAndSort.IsNullOrEmpty())
            {
                return;
            }

            string[] orderByDatas = orderByColumnAndSort.Split(' ');

            if (orderByDatas.Length > 0)
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

    public static class SortModelExtensions
    {
        public static string ToOrderByText(this List<SortModel> sortModels)
        {
            if (!sortModels.AnyAndNotNull())
            {
                return string.Empty;
            }

            return $" ORDER BY {string.Join(",", sortModels.Select(s => $"{s.ColumnName} {s.SortOrderText}"))} ";
        }
    }
}