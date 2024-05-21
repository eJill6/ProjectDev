using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyFavoriteOfficialShop: MyFavorite
    {
        /// <summary>
        /// 店铺id(ApplyId)
        /// </summary>
        public string ApplyId { get; set; }
        public string BossId { get; set; }

        /// <summary>
        /// 店铺图片地址
        /// </summary>
        public string ShopAvatarSource { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 妹子数量
        /// </summary>

        public int Girls { get; set; }
        /// <summary>
        /// 浏览数量
        /// </summary>

        public int ViewBaseCount { get; set; }
        /// <summary>
        /// 观看数
        /// </summary>
        public int Views { get; set; }
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
    }
}
