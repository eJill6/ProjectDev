using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class PaginationSetting
    {
        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        /// <summary>
        /// 執行傳入參數pageNumber的js函式名稱
        /// </summary>
        public string Callback { get; set; }

        /// <summary>
        /// 用於在同個頁面裡有複數分頁區塊時以此ID做區分
        /// </summary>
        public int SettingId { get; set; }
    }
}