using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminReport
{
    /// <summary>
    /// 贴子列表
    /// </summary>
    public class AdminReportList
    {
        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string PostTypeText => PostType.GetDescription();

        /// <summary>
        /// 投诉原因
        /// </summary>
        public string ReportTypeText => ReportType.GetDescription();


        /// <summary>
        /// 状态
        /// </summary>
        public string StatusText => Status.GetDescription();

        /// <summary>
        /// 贴子類型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 投诉会员ID
        /// </summary>
        public int ComplainantUserId { get; set; }

        /// <summary>
        /// 送审时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 投诉原因
        /// </summary>
        public ReportType ReportType { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 详情描述
        /// </summary>
        public string Describe { get; set; }
    }
}