using JxBackendService.Common.Util;
using System.Collections.Generic;

namespace JxBackendService.Model.Paging
{
    public class ScrollLoadResultModel<T>
    {
        public ScrollLoadResultModel()
        { }

        public int TotalCount { get; set; }

        public string TotalCountText
        {
            get => TotalCount.ToIntWithThousandComma();
            set { }
        }

        public string LastKey { get; set; }

        public List<T> ResultList { get; set; } = new List<T>();
    }

    public class ScrollLoadResultModel<T, AdditionalDataType> : ScrollLoadResultModel<T>
    {
        public ScrollLoadResultModel()
        { }

        public AdditionalDataType AdditionalData { get; set; }
    }
}