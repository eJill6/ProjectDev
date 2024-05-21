using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminComment
{
    public class AdminCommentDetail
    {
        /// <summary>
        /// 评价ID
        /// </summary>
        public string CommentId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 首次送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 再次审核时间
        /// </summary>
        public string UpdateTimeText => UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 消费时间
        /// </summary>
        public string SpentTimeText => SpentTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 服务详情
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 所在位置
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 照片 id list
        /// </summary>
        public string[] PhotoIds { get; set; }

        /// <summary>
        /// 消費時間
        /// </summary>
        public DateTime SpentTime { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        public string Memo { get; set; }

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
