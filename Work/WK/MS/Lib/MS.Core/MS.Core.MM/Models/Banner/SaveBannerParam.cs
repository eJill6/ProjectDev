using MS.Core.MM.Model.Entities.Banner;
using MS.Core.MM.Model.Media;

namespace MS.Core.MM.Model.Banner
{
    /// <inheritdoc cref="ISaveBannerParam"/>
    public class SaveBannerParam : MMBanner
    {
        /// <inheritdoc/>
        public SaveMediaToOssParam? Media { get; set; }
    }
}