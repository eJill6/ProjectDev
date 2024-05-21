using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Auth
{
    /// <summary>
    /// 覓老闆身份申請資料
    /// </summary>
    public class BossIdentityApplyDataForClient
    {
        /// <summary>
        /// 店鋪資料
        /// </summary>
        public string ShopName { get; set; } = string.Empty;

        /// <summary>
        /// 店铺介绍
        /// </summary>
        public string ShopIntroduce { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int ShopAge { get; set; }

        /// <summary>
        /// 自评人气
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 联系软件
        /// </summary>
        public string ContactType { get; set; }

        /// <summary>
        /// 妹子數量
        /// </summary>
        public string Girls { get; set; } = string.Empty;

        /// <summary>
        /// 成交订单数
        /// </summary>
        public int OrderQuantity { get; set; }

        /// <summary>
        /// 服務價格 - 最低價格
        /// </summary>
        //public decimal LowPrice { get; set; }

        /// <summary>
        /// 服務價格 - 最高價格
        /// </summary>
        //public decimal HighPrice { get; set; }


        /// <summary>
        /// 聯繫方式
        /// </summary>
        public string Contact { get; set; } = string.Empty;
        /// <summary>
        /// 订单成交量
        /// </summary>
        public int OrderTurnover { get; set; }

        /// <summary>
        /// 人气
        /// </summary>
        public int Popularity { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverPhoto{ get; set; } = string.Empty;
        /// <summary>
        /// 照片 id list
        /// </summary>
        public string[] PhotoIds { get; set; } = new string[0];

        /// <summary>
        /// 商家照片
        /// </summary>
        public string[] ShopPhotoIds { get; set; } = new string[0];
    }
}