using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMHomeAnnouncementBs : BaseEntityModel
    {
        public int Id { get; set; }
        public string? HomeContent { get; set; } = string.Empty;
        public string? RedirectUrl { get; set; } = string.Empty;
        public DateTime? ModifyDate { get; set; }
        public string Operator { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ModifyDateText => ModifyDate.ToFormatDateTimeString();

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        public string StartTimeText => StartTime.ToFormatDateTimeString();

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public string EndTimeText => EndTime.ToFormatDateTimeString();

        /// <summary>
        /// 类型。1：公告、2：首页盖台
        /// </summary>
        public int Type { get; set; }
    }
}