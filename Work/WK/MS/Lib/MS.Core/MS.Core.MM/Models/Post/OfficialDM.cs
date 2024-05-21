namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 私信
    /// </summary>
    public class OfficialDM
    {
        /// <summary>
        /// 下載提示
        /// </summary>
        public string DownloadTip { get; set; } = string.Empty;

        /// <summary>
        /// 下載連結
        /// </summary>
        public string DownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// App用下載連結
        /// </summary>
        public string AppDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// 聯繫帳號
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// IM 連結
        /// </summary>
        public string ImUrl { get; set; } = string.Empty;
    }
}