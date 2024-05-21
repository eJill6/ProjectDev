using JxBackendService.Common.Extensions;
using System.Linq;

namespace JxBackendService.Model.Paging
{
    public class PagedSqlQueryParamsModel : BasePagedParamsModel
    {
        public string PreSearchSql { get; set; }
        /// <summary>
        /// fill sql after "FROM"
        /// </summary>
        public string SqlBody { get; set; }
        public string SelectColumns { get; set; }
        public string OrderBy { get; set; }

        public int MaxSearchRowCount { get; set; }

        public object Parameters { get; set; }

        public void SetPager(BasePagingRequestParam basePagingRequestParam)
        {
            if (basePagingRequestParam.PageNo <= 0 ||
                basePagingRequestParam.PageSize <= 0)
            {
                return;
            }

            PageNo = basePagingRequestParam.PageNo;
            PageSize = basePagingRequestParam.PageSize;

            if (basePagingRequestParam.Offset > 0)
            {
                Offset = basePagingRequestParam.Offset;
            }

            if (basePagingRequestParam.SortModels.AnyAndNotNull())
            {
                //分頁語法必須要有排序
                OrderBy = "ORDER BY " +
                    string.Join(",", basePagingRequestParam.SortModels.Select(s => $"{s.ColumnName} {s.SortOrderText}"));
            }
        }
    }
}