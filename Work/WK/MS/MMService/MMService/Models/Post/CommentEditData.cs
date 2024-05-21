namespace MMService.Models.Post
{
    /// <summary>
    /// 評論編輯資訊
    /// </summary>
    public class CommentEditData
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 消費時間
        /// </summary>
        public string SpentTime { get; set; } = string.Empty;

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 照片 id list
        /// </summary>
        public Dictionary<string, string>? PhotoSource { get; set; } = new Dictionary<string, string>();
    }
}