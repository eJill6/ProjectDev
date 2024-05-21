using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 我的解鎖贴子
    /// </summary>
    public class MyUnlockPostListForClient : PostListForClient
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 評論編號
        /// </summary>
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// 評論結果
        /// </summary>
        public string CommentMemo { get; set; } = string.Empty;
    }
}