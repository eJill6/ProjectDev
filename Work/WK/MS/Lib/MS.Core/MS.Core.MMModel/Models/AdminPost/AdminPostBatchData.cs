using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 批量更新資料
    /// </summary>
    public class AdminPostBatchData
    {
        /// <summary>
        /// 贴子編號
        /// </summary>
        public string PostIds { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public int PostStatus { get; set; }
    }
}