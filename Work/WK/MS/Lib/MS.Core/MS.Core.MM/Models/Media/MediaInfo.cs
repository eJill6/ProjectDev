using MS.Core.MM.Model.Entities.Media;

namespace MS.Core.MM.Models.Media
{
    /// <summary>
    /// 媒體資料
    /// </summary>
    public class MediaInfo : MMMedia
    {
        /// <summary>
        /// 完整路徑
        /// </summary>
        public string FullMediaUrl { get; set; } = string.Empty;
    }
}
