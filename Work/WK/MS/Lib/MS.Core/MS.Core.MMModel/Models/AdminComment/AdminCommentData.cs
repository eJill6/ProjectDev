using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.AdminComment
{
    /// <summary>
    /// 更新資料
    /// </summary>
    public class AdminCommentData
    {
        /// <summary>
        /// 評價編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 審核人員暱稱
        /// </summary>
        public string ExamineMan { get; set; }
    }
}