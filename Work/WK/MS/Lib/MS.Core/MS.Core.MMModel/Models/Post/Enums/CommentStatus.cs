using System.ComponentModel;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum CommentStatus
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        UnderReview = 0,

        /// <summary>
        /// 核准
        /// </summary>
        [Description("核准")]
        Approval = 1,

        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        NotApproved = 2,

        /// <summary>
        /// 贴子尚未解鎖
        /// </summary>
        [Description("贴子尚未解鎖")]
        PostLock = 3,

        /// <summary>
        /// 尚未評論
        /// </summary>
        [Description("尚未評論")]
        NotYetComment = 4
    }
}