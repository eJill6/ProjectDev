using MMService.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;

namespace MMService.Models.My
{
    /// <summary>
    /// 我的解鎖贴子
    /// </summary>
    public class MyUnlockPostList : PostList
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }
        /// <summary>
        /// 观看基础值
        /// </summary>
        public int ViewBaseCount { get; set; }
        /// <summary>
        /// 解锁基础值
        /// </summary>
        public int UnlockBaseCount { get; set; }

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