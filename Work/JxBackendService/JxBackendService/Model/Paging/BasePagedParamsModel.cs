using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Paging
{
    public class BasePagedParamsModel
    {
        private static readonly int _defaultPageSize = 30;
        private static readonly int _maxPageSize = 1000;

        private int _pageIndex = 0;
        private int _pageNo = 1;
        private int _pageSize;
        
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
            set { 
                _pageNo = value;
                _pageIndex = value - 1;
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

                return _pageSize;
            }
            set
            {
                if (value <= _maxPageSize)
                {
                    _pageSize = value;
                }
                else
                {
                    _pageSize = _maxPageSize;
                }
            }
        }

        public int PageIndex => PageNo - 1;

        public int Offset { get { return (PageNo - 1) * PageSize; } }
    }
}
