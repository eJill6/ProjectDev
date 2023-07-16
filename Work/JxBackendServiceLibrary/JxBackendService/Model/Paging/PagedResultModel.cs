using JxBackendService.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Paging
{
    public class PagerInfo
    {
        public PagerInfo() { }

        public PagerInfo(BasePagedParamsModel param)
        {
            PageNo = param.PageNo;
            PageSize = param.PageSize;
        }

        public PagerInfo(bool isEmpty)
        {
            if (isEmpty)
            {
                TotalCount = 0;
                PageSize = 10;
                PageNo = 1;
            }
        }

        public int TotalCount { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage => PageNo >= TotalPageCount;
        public int TotalPageCount
        {
            get
            {
                if (PageSize > 0)
                {
                    return Convert.ToInt32(Math.Ceiling((decimal)TotalCount / (decimal)PageSize));
                }

                throw new ArgumentException("PageSize is 0");
            }
            set { }
        }
    }

    public class PagedResultModel<T> : PagerInfo
    {
        public PagedResultModel() { }
        public PagedResultModel(BasePagedParamsModel param) : base(param) { }

        public PagedResultModel(bool isEmpty) : base(isEmpty) { }

        //從IList改為明確型別List, 讓linq foreach能套用
        public List<T> ResultList { get; set; } = new List<T>();
    }

    public class PagedResultWithAdditionalData<T, AdditionalDataType> : PagedResultModel<T>
    {
        public PagedResultWithAdditionalData() { }
        public PagedResultWithAdditionalData(BasePagedParamsModel param) : base(param) { }

        public AdditionalDataType AdditionalData { get; set; }
    }

    public class PagedResultWithAdditionalData<T> : PagedResultWithAdditionalData<T, object>
    {
        public PagedResultWithAdditionalData(BasePagedParamsModel param) : base(param) { }
    }

    public static class PagedResultModelExtensions
    {
        public static PagedResultModel<TargetType> CloneWithoutResult<T, TargetType>(
            this PagedResultModel<T> source)
        {
            var cloneModel = new PagedResultModel<TargetType>()
            {
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
            };

            return cloneModel;
        }

        public static PagedResultModel<TargetType> CloneWithoutResult<T, TargetType, AddtionalDataType>(
            this PagedResultWithAdditionalData<T, AddtionalDataType> source)
        {
            var cloneModel = new PagedResultModel<TargetType>()
            {
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
            };

            return cloneModel;
        }

        public static PagedResultWithAdditionalData<TargetType, TargetAddtionalDataType> CloneWithoutResult<T, TargetType, TargetAddtionalDataType>(
            this PagedResultModel<T> source)
        {
            var cloneModel = new PagedResultWithAdditionalData<TargetType, TargetAddtionalDataType>()
            {
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
            };

            return cloneModel;
        }

        public static PagedResultWithAdditionalData<TargetType, TargetAddtionalDataType> CloneWithoutResult<T, AddtionalDataType, TargetType, TargetAddtionalDataType>(
            this PagedResultWithAdditionalData<T, AddtionalDataType> source)
        {
            var cloneModel = new PagedResultWithAdditionalData<TargetType, TargetAddtionalDataType>()
            {
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
            };

            return cloneModel;
        }

        public static PagedResultWithAdditionalData<TargetType, object> ToPagedResultWithAdditionalData<TargetType>(
            this PagedResultModel<TargetType> source)
        {
            var cloneModel = new PagedResultWithAdditionalData<TargetType, object>()
            {
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                ResultList = source.ResultList
            };

            return cloneModel;
        }
    }
}
