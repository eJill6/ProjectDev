using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.User
{
    public class IdentityApplyForClient
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public string ApplyId { get; set; }
        public string BossId { get; set; }

        /// <summary>
        /// 店鋪名稱
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 妹子數量
        /// </summary>
        public int? Girls { get; set; }

        /// <summary>
        /// 店铺（头像）來源
        /// </summary>
        public string ShopAvatarSource { get; set; }

        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public string[] BusinessPhotoSource { get; set; }
        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public MediaViewModel[] BusinessPhotoSourceViewModel { get; set; }

        /// <summary>
        /// 聯繫软件
        /// </summary>
        public string ContactApp { get; set; }

        /// <summary>
        /// 店铺聯繫方式
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int? ShopYears { get; set; }

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
        public string Introduction { get; set; } = string.Empty;

        /// <summary>
        /// 营业时段
        /// </summary>
        public string BusinessDate { get; set; } = string.Empty;

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHour { get; set; } = string.Empty;

        /// <summary>
        /// 店铺观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 当前用户是否收藏该店铺
        /// </summary>
        public bool? IsFollow { get; set; }

        public string[] AreaCodes { get; set; } = Array.Empty<string>();
        /// <summary>
        /// 该店铺是否处于修改审核当中
        /// </summary>
        public bool IsEditAudit { get; set; }
    }
}