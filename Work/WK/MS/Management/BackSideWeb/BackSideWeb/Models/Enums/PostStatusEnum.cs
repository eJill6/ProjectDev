using System.ComponentModel;

namespace BackSideWeb.Models.Enums
{
    public enum PostStatusEnum
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        UnderReview = 0,

        /// <summary>
        /// 展示中
        /// </summary>
        [Description("展示中")]
        Approval = 1,

        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        NotApproved = 2
    }
}
