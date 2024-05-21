using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 贴子列表
    /// </summary>
    public class AdminPostList
    {
        /// <summary>
        /// 贴子編號
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string PostTypeText => PostType.GetDescription();

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 使用者類型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 會員卡內容
        /// </summary>
        public string VipCard { get; set; }

        /// <summary>
        /// 首次送審時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 再次審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 首次送審時間
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public string UpdateTimeText => UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 再次審核時間
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 精選
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// 精選
        /// </summary>
        public string IsFeaturedText => IsFeatured == null ? "-" : IsFeatured.Value ? "精选" : "-";

        /// <summary>
        /// 熱度
        /// </summary>
        public int? Heat { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusText => Status != ReviewStatus.Approval ? Status.GetDescription() : "展示中";

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus Status { get; set; }
    }
}