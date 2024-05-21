using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 贴子清單
    /// </summary>
    public class MyPostList
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 封面Url
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 解鎖次數
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateTime { get; set; } = string.Empty;

        /// <summary>
        /// 審核評論
        /// </summary>
        public string Memo { get; set; } = string.Empty;
    }
}