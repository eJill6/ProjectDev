using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminComment
{
    /// <summary>
    /// 官方贴子评价列表
    /// </summary>
    public class AdminOfficialCommentList
    {
        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string CommentId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusText => Status.GetDescription();

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 首次送审时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 首次送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送审时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 再次送审时间
        /// </summary>
        public string UpdateTimeText => UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 状态
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; }
    }
}