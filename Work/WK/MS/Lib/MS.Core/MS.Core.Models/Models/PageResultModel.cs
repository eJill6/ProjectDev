using System;

namespace MS.Core.Models.Models
{
    /// <summary>
    /// 分頁模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResultModel<T> : PaginationModel
    {
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }


        /// <summary>
        /// 資料總筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 分頁資料
        /// </summary>
        public T[] Data { get; set; } = Array.Empty<T>();
    }
}