using System;

namespace JxBackendService.Model.Entity
{
    /// <summary>
    /// 用戶行為記錄資料迷型
    /// </summary>
    public class ActivityLog
    {
        public int? ID { get; set; }

        public decimal? PrizeMoney { get; set; }

        public DateTime? ActyDate { get; set; }

        public string IP { get; set; }

        public int? UserID { get; set; }

        public string Msg { get; set; }

        public int NotIssuingType { get; set; }
    }
}