using JxBackendService.Common.Util;
using System.Collections.Generic;

namespace JxBackendService.Model.Paging
{
    public class BasePagingRequestParam : BasePagedParamsModel
    {
        public List<SortModel> SortModels { get; set; }

        /// <summary>
        /// wcf呼叫端直接設定SortModels會異常,故開設此欄位做設定(生命週期的關係,在setter設定SortModels沒有作用)
        /// </summary>
        public string SortField { get; set; }

        public void InitSortModelsBySortField()
        {
            if (SortField.IsNullOrEmpty())
            {
                return;
            }

            var sortModels = new List<SortModel>();
            string[] sortInfos = SortField.Split(' ');

            if (sortInfos.Length != 2)
            {
                return;
            }

            if (sortInfos[0].IsNullOrEmpty())
            {
                return;
            }

            var sortModel = new SortModel()
            {
                ColumnName = sortInfos[0]
            };

            sortModel.SortOrderText = sortInfos[1];
            sortModels.Add(sortModel);
            SortModels = sortModels;
        }

        public string ToOrderByText()
        {
            return SortModels.ToOrderByText();
        }
    }
}