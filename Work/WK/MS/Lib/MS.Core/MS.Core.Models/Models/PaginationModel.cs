namespace MS.Core.Models.Models
{
    public class PaginationModel
    {
        /// <summary>
        /// 存放頁面資料
        /// </summary>
        private int _page;

        /// <summary>
        /// 查詢頁
        /// </summary>
        public int Page
        {
            get
            {
                return _page;
            }

            set
            {
                _page = value;
            }
        }

        /// <summary>
        /// 查詢頁
        /// </summary>
        public int PageNo
        {
            get
            {
                return _page;
            }

            set
            {
                _page = value;
            }
        }

        /// <summary>
        /// 分頁大小
        /// </summary>
        public int PageSize { get; set; } = GlobalSettings.PageSize;
    }
}