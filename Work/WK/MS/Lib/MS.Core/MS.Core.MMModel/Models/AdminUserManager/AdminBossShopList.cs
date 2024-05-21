using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminBossShopList
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 申请记录ID
        /// </summary>
        public string ApplyId { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 原bossId
        /// </summary>
        public string BossId { get; set; } = string.Empty;

        /// <summary>
        /// 联系软件
        /// </summary>
        public string ContactApp { get; set; } = string.Empty;

        /// <summary>
        /// 联系号码
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; } = string.Empty;

        /// <summary>
        /// 妹子数量
        /// </summary>
        public int Girls { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int ShopYears { get; set; }

        /// <summary>
        /// 成交订单数
        /// </summary>
        public int DealOrder { get; set; }

        /// <summary>
        /// 自评人气
        /// </summary>
        public int SelfPopularity { get; set; }

        /// <summary>
        /// 店铺介绍
        /// </summary>
        public string Introduction { get; set; } = string.Empty;

        /// <summary>
        /// 店铺封面照片
        /// </summary>
        public string ShopAvatar { get; set; } = string.Empty;

        /// <summary>
        /// 店铺照片
        /// </summary>
        public string BusinessPhotoUrls { get; set; } = string.Empty;

        /// <summary>
        /// 狀態。0：審核中、1：核準 2：未通過
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
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        public string ExamineTimeText => ExamineTime == null ? "-" : ExamineTime.Value.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        public string UpdateTimeText => UpdateTime == null ? "-" : UpdateTime.Value.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 当前身份
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
        /// 店铺（头像）來源
        /// </summary>
        public Dictionary<string, string> ShopAvatarSources { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public Dictionary<string, string> BusinessPhotoSources { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}