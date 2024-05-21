namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 官方評論清單
    /// </summary>
    public class OfficialCommentList
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

        /// <summary>
        /// 發布時間(審核通過) = 審核通過時間
        /// </summary>
        public string PublishTime { get; set; } = string.Empty;
    }
}