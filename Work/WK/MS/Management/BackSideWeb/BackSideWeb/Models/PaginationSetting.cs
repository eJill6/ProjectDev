using JxBackendService.Interface.Model.Paging;

namespace BackSideWeb.Models
{
    public class PaginationSetting
    {
        public PaginationSetting()
        { }

        public PaginationSetting(IPagerInfo pagerInfo)
        {
            PageNo = pagerInfo.PageNo;
            PageSize = pagerInfo.PageSize;
            PageCount = pagerInfo.TotalPageCount;
            DataCount = pagerInfo.TotalCount;
        }

        public int DataCount { get; set; }

        public int PageCount { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 執行傳入參數pageNumber的js函式名稱
        /// </summary>
        public string Callback { get; set; }

        /// <summary>
        /// 用於在同個頁面裡有複數分頁區塊時以此ID做區分
        /// </summary>
        public int SettingId { get; set; } = 0;
    }
}