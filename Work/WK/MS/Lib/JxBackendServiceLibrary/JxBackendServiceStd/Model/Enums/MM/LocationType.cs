using System.ComponentModel;

namespace JxBackendService.Model.Enums.MM
{
    /// <summary>
    /// 前台位置
    /// </summary>
    public enum LocationType : byte
    {
        /// <summary>
        /// 首页主选单
        /// </summary>
        [Description("首页主选单")]
        HomePageMenu = 1,

        /// <summary>
        /// 首页轮播Banner
        /// </summary>
        [Description("首页轮播Banner")]
        HomePageBanner = 2,

        /// <summary>
        /// 店铺轮播Banner
        /// </summary>
        [Description("店铺轮播Banner")]
        StoreBanner = 3
    }
}