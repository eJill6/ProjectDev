using System.Collections.Generic;

namespace MS.Core.MMModel.Models
{
    /// <summary>
    /// 官方店铺信息
    /// </summary>
    public class OfficialShopViewModel
    {
        /// <summary>
        /// 店铺id(ApplyId)
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
        /// 浏览数量
        /// </summary>

        public int ViewBaseCount { get; set; }

        public int Views{ get; set; }

        /// <summary>
        /// 店龄
        /// </summary>

        public int ShopYears { get; set; }
        /// <summary>
        /// 成交订单数量
        /// </summary>

        public int DealOrder { get; set; }
        /// <summary>
        /// 评分
        /// </summary>

        public int SelfPopularity { get; set; }

        /// <summary>
        /// 帖子列表
        /// </summary>
        public List<OfficialShopPostList> PostList { get; set; }
    }

    public class OfficialShopPostList
    {
        /// <summary>
        ///帖子id
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 帖子封面图片url
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 期望收入，最低价
        /// </summary>
        public decimal LowPrice { get; set; }
    }
}