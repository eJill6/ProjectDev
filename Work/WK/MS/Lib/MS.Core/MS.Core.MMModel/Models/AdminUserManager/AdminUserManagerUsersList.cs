using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerUsersList
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegisterTimeText => RegisterTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 用戶身份
        /// </summary>
        public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 会员身份
        /// </summary>
        public string UserIdentityText
        {
            get
            {
                return EnumExtension.GetDescription(UserIdentity);
            }
        }

        /// <summary>
        /// 已发贴
        /// </summary>
        public string PostQuantity { get; set; }

        /// <summary>
        /// 被解锁次数
        /// </summary>
        public string UnlockedQuantity { get; set; }

        /// <summary>
        /// 累积收益
        /// </summary>
        public string Income { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public string EarnestMoneyText => EarnestMoney.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 是否營業中
        /// </summary>
        public bool IsOpen { get; set; }

        public string IsOpenText => IsOpen ? "ON" : "OFF";

        /// <summary>
        /// 通讯APP
        /// </summary>
        public string ContactApp { get; set; }

        /// <summary>
        /// 通讯账号
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 成交订单
        /// </summary>
        public int? DealOrder { get; set; }
        /// <summary>
        /// 平台分成比
        /// </summary>
        public int? PlatformSharing { get; set; }
    }
}