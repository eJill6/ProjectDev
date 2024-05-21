using System;

namespace MS.Core.MMModel.Models.HomeAnnouncement
{
    public class HomeAnnouncement
    {
        public int Id { get; set; }
        public string HomeContent { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 类型。1：公告、2：首页盖台
        /// </summary>
        public int Type { get; set; }
    }
}