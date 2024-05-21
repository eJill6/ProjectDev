using System.ComponentModel;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 審核狀態定義
    /// </summary>
    public enum ReviewStatus
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
        NotApproved = 2
    }
}
