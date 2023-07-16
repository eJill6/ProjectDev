namespace JxBackendService.Model.Paging
{
    public class BasePagedParamsModel
    {
        private static readonly int _defaultPageSize = 30;

        private static readonly int _maxPageSize = 1000;

        private int _pageNo = 1;

        private int _pageSize;

        public bool HasMaxPageSize { get; set; } = true;

        public int PageNo
        {
            get
            {
                if (_pageNo <= 0)
                {
                    _pageNo = 1;
                }

                return _pageNo;
            }
            set
            {
                _pageNo = value;
            }
        }

        public int PageSize
        {
            get
            {
                if (_pageSize <= 0)
                {
                    _pageSize = _defaultPageSize;
                }

                if (HasMaxPageSize && _pageSize > _maxPageSize)
                {
                    _pageSize = _maxPageSize;
                }

                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public int PageIndex => PageNo - 1;

        /// <summary>開放給瀑布式載入型的功能使用</summary>
        public int? Offset { get; set; }

        /// <summary>取得sql要用到的offset, 若offset沒有外部指定, 則使用(PageNo - 1)* PageSize</summary>
        public int GetComputedOffset()
        {
            if (Offset.HasValue)
            {
                if (Offset.Value >= 0)
                {
                    return Offset.Value;
                }

                return 0;
            }

            return (PageNo - 1) * PageSize;
        }
    }
}