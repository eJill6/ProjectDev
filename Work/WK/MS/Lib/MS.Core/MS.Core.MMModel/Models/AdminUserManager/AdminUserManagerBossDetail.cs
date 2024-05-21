using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerBossDetail
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public int ApplyIdentity { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int? ShopYears { get; set; }

        /// <summary>
        /// 妹子数量
        /// </summary>
        public int? Girls { get; set; }

        /// <summary>
        /// 成交订单
        /// </summary>
        public int? DealOrder { get; set; }

        /// <summary>
        /// 自评人气
        /// </summary>
        public int? SelfPopularity { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 营业时段
        /// </summary>
        public string BusinessDate { get; set; }

        public int? BusinessDateStart { get; set; }
        public int? BusinessDateEnd { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHour { get; set; }

        public int? BusinessHourStart { get; set; }
        public int? BusinessHourEnd { get; set; }

        /// <summary>
        /// 店铺头像
        /// </summary>
        public string ShopAvatar { get; set; }

        /// <summary>
        /// 商家照片
        /// </summary>
        public string BusinessPhoto { get; set; }
    }
}