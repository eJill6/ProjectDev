namespace MS.Core.MMModel.Models
{
    public class BannerViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 廣告類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 連結方式
        /// </summary>
        public byte LinkType { get; set; }

        /// <summary>
        /// 轉導網址
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;

        /// <summary>
        /// 完整路徑
        /// </summary>
        public string FullMediaUrl { get; set; } = string.Empty;

        /// <summary>
        /// 前台位置
        /// </summary>

        public int LocationType { get; set; }
    }
}