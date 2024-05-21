namespace MMService.Models.Post
{
    /// <summary>
    /// 評論清單
    /// </summary>
    public class CommentList
    {
        /// <summary>
        /// 頭像
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 評論當下暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 消費當下時間
        /// </summary>
        public string SpentTime { get; set; } = string.Empty;

        /// <summary>
        /// 區域代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 發布時間(審核通過) = 審核通過時間
        /// </summary>
        public string PublishTime { get; set; } = string.Empty;

        /// <summary>
        /// 評論上傳的照片
        /// </summary>
        public string[] PhotoUrls { get; set; } = Array.Empty<string>();
    }
}