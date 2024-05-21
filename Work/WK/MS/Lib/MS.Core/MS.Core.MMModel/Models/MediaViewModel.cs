namespace MS.Core.MMModel.Models
{
    /// <summary>
    /// 媒體檔案
    /// </summary>
    public class MediaViewModel
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 完整路徑
        /// </summary>
        public string FullMediaUrl { get; set; } = string.Empty;
    }
}
