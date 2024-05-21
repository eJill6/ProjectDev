using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerIdentityApplyList
    {
        /// <summary>
        /// BossId
        /// </summary>
        public string BossId { get; set; }

        /// <summary>
        /// 申请ID
        /// </summary>
        public string ApplyId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 当前身份
        /// </summary>
        public IdentityType OriginalIdentity { get; set; }

        public string OriginalIdentityText
        {
            get
            {
                return EnumExtension.GetDescription(OriginalIdentity);
            }
        }

        /// <summary>
        /// 申请身份
        /// </summary>
        public IdentityType ApplyIdentity { get; set; }

        public string ApplyIdentityText
        {
            get
            {
                return EnumExtension.GetDescription(ApplyIdentity);
            }
        }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; } = 0;

        public string EarnestMoneyText => EarnestMoney.ToString(GlobalSettings.AmountToIntString);

        /// <summary>
        /// 身份认证状态
        /// </summary>
        public ReviewStatus Status { get; set; }

        public string StatusText
        {
            get
            {
                return EnumExtension.GetDescription(Status);
            }
        }

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

        public string[] BusinessPhotoUrls { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 店铺（头像）來源
        /// </summary>
        public Dictionary<string, string> ShopAvatarSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public Dictionary<string, string> BusinessPhotoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 聯繫软件
        /// </summary>
        public string ContactApp { get; set; }

        /// <summary>
        /// 店铺聯繫方式
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 增加发贴次数
        /// </summary>
        public int? ExtraPostCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        public string ExamineTimeText => ExamineTime == null ? "" : ExamineTime.Value.ToString(GlobalSettings.DateTimeFormat);

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
        /// 当前用户是否收藏该店铺
        /// </summary>
        public bool? IsFollow { get; set; }

        /// <summary>
        /// 是否营业
        /// </summary>
        public bool? IsOpen { get; set; }

        public string IsOpenText => IsOpen.HasValue ? IsOpen.Value ? "开启" : "关闭" : "-";

        /// <summary>
        /// TG群ID
        /// </summary>
        public string TelegramGroupId { get; set; }
        /// <summary>
        /// 平台分成
        /// </summary>
        public int? PlatformSharing { get; set; }
        public string PlatformSharingText => ApplyIdentity == IdentityType.SuperBoss && PlatformSharing >= 0 ? PlatformSharing.ToString() + "%" : ApplyIdentity == IdentityType.SuperBoss ? "0%" : "-%";
        public string SuperBossSharing => ApplyIdentity == IdentityType.SuperBoss && PlatformSharing >= 0 ? (100 - PlatformSharing).ToString() + "%" : ApplyIdentity == IdentityType.SuperBoss ? "0%" : "-%";
    }
}