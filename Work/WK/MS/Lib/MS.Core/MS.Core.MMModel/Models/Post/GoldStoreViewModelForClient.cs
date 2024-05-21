namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 私信
    /// </summary>
    public class GoldStoreViewModelForClient
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