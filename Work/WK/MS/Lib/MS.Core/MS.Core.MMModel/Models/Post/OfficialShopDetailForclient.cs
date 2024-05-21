using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Core.MMModel.Models.Post
{
    public class OfficialShopDetailForclient
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public string ApplyId { get; set; }

        /// <summary>
        /// bossId
        /// </summary>
        public string BossId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 当前身份
        /// </summary>
        public IdentityType OriginalIdentity { get; set; }

        /// <summary>
        /// 申请身份
        /// </summary>
        public IdentityType ApplyIdentity { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 身份认证状态
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 店鋪名稱
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 妹子數量
        /// </summary>
        public int? Girls { get; set; }

        /// <summary>
        /// 服務價格 - 最低價格
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// 服務價格 - 最高價格
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// 图片(店铺头像)
        /// </summary>
        public string ShopAvatarSource { get; set; }

        public string[] BusinessPhotoUrls { get; set; }

        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public string[] BusinessPhotoSource { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int? ShopYears { get; set; }

        /// <summary>
        /// 成交订单
        /// </summary>
        public int? DealOrder { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

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

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHour { get; set; }

        /// <summary>
        /// 当前用户是否收藏该店铺
        /// </summary>
        public bool? IsFollow { get; set; }

        /// 联系软件
        public string ContactApp { get; set; }

        /// 联系地址
        public string ContactInfo { get; set;}
        /// <summary>
        /// 帖子地区列表
        /// </summary>
        public string[] AreaCodes { get; set; }

        public MediaViewModel[] BusinessPhotoSourceViewModel { get; set; }
        /// <summary>
        /// 该店铺是否处于修改审核当中
        /// </summary>
        public bool IsEditAudit { get; set; }
    }
}