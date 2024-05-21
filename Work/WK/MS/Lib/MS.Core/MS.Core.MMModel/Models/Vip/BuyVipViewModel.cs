using System;

namespace MS.Core.MMModel.Models.Vip
{
    public class BuyVipViewModel
    {
        /// <summary>
        /// 價格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 會員卡名稱
        /// </summary>
        public string VipName { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>
        public string EffectiveTime { get; set; }
    }
}
