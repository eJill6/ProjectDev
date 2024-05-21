using System;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGamePlayInfoSearchParam
    {
        /// <summary>產品代碼</summary>
        public string ProductCode { get; set; }

        public string GameType { get; set; }

        /// <summary>開始日期</summary>
        public DateTime QueryStartDate { get; set; }

        /// <summary>結束日期</summary>
        public DateTime QueryEndDate { get; set; }

        /// <summary>畫面最後一筆LastKey</summary>
        public string LastKey { get; set; }

        /// <summary>獲取下一頁的資料筆數</summary>
        public int PageSize { get; set; }
    }
}