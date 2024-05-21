using MS.Core.MMModel.Models.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserBossParam
    {
        /// <summary>
        /// Id MMBossShop店铺编辑
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// BossId
        /// </summary>
        public string BossId { get; set; }

        /// <summary>
        /// ApplyId
        /// </summary>
        public string ApplyId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public int ApplyIdentity { get; set; }
		public string ApplyIdentityText { get; set; }

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

        public string BusinessDateStart { get; set; }
        public string BusinessDateEnd { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHour { get; set; }

        public string BusinessHourStart { get; set; }
        public string BusinessHourEnd { get; set; }

        /// <summary>
        /// 店铺头像
        /// </summary>
        public string ShopAvatar { get; set; }

        /// <summary>
        /// 商家照片
        /// </summary>
        public string BusinessPhotoUrls { get; set; }

        /// <summary>
        /// 店铺（头像）來源
        /// </summary>
        public Dictionary<string, string> ShopAvatarSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 店铺（展示）來源
        /// </summary>
        public Dictionary<string, string> BusinessPhotoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 审核状态 0审核中 1 通过2 未通过
        /// </summary>
        public int Status { get; set; }

        public string StatusText
        {
            get
            {
                return EnumExtension.GetDescription((ReviewStatus)Status);
            }
        }

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 联系软件
        /// </summary>
        public string ContactApp { get; set; } = string.Empty;

        /// <summary>
        /// 联系号码
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;

        /// <summary>
        /// 是否营业
        /// </summary>
        public bool? IsOpen { get; set; }

        public string IsOpenText => IsOpen.HasValue ? IsOpen.Value ? "开启" : "关闭" : "-";

        /// <summary>
        /// TG群ID
        /// </summary>
        public string TelegramGroupId { get; set; }
    }
}