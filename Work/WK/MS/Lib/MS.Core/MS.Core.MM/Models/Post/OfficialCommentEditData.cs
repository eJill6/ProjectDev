namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 官方評論編輯資訊
    /// </summary>
    public class OfficialCommentEditData
    {
        /// <summary>
        /// 評論 Id
        /// </summary>
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// 顏值評分
        /// </summary>
        public int FacialScore { get; set; }

        /// <summary>
        /// 服務質量
        /// </summary>
        public int ServiceQuality { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;
    }
}