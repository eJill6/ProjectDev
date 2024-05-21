namespace MS.Core.MMModel.Models
{
    /// <summary>
    /// 官方店铺推荐
    /// </summary>
    public class GoldStoreViewModel
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        public string ApplyId { get; set; } = string.Empty;

        /// <summary>
        /// 店铺图片地址
        /// </summary>
        public string ShopAvatarSource { get; set; } = string.Empty;

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; } = string.Empty;

        /// <summary>
        /// 妹子数量
        /// </summary>

        public int Girls { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Top { get; set; }
    }
}