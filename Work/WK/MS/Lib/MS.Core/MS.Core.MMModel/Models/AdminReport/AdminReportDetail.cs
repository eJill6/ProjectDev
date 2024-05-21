using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminComment
{
    public class AdminReportDetail
    {
        /// <summary>
        /// 舉報編號
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int ComplainantUserId { get; set; }

        /// <summary>
        /// 送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送审时间
        /// </summary>
        public string UpdateTimeText => UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";


        /// <summary>
        /// 投诉原因
        /// </summary>
        public string ReportTypeText => ReportType.GetDescription();

        /// <summary>
        /// 详情描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 截图证据
        /// </summary>
        public string[] PhotoIds { get; set; }

        /// <summary>
        /// 投诉原因
        /// </summary>
        public ReportType ReportType { get; set; }

        /// <inheritdoc cref="PostType"/>
        public PostType PostType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PostTranId { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 送審時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public string StatusText => Status.GetDescription();
    }
}
