using MS.Core.MM.Model.Entities.Banner;
using MS.Core.MM.Models.Media;

namespace MS.Core.MM.Model.Banner
{
    /// <summary>
    /// Banner資料庫資料
    /// </summary>
    public class BannerInfo : MMBanner
    {
        /// <summary>
        /// 媒體資訊
        /// </summary>
        public MediaInfo? Media { get; set; }
    }
}