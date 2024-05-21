using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerUsersDetail
    {
        /// <summary>
        /// 頭像
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

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
        /// 会员身份
        /// </summary>
        public IdentityType UserIdentity { get; set; }

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
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public string EarnestMoneyText => EarnestMoney.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 通讯APP
        /// </summary>
        public string ContactApp { get; set; }

        /// <summary>
        /// 通讯账号
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 身份备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 会员卡
        /// </summary>
        public string VipCards { get; set; }

        /// <summary>
        /// 会员卡到期时间
        /// </summary>
        public DateTime? VipCardEffectiveTime { get; set; }

        /// <summary>
        /// 会员卡到期时间
        /// </summary>
        public string VipCardEffectiveTimeText => VipCardEffectiveTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 本月收益
        /// </summary>
        public decimal MonthIncome { get; set; }

        /// <summary>
        /// 本月收益
        /// </summary>
        public string MonthIncomeText => MonthIncome.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 暂锁收益
        /// </summary>
        public decimal ComingIncome { get; set; }

        /// <summary>
        /// 暂锁收益
        /// </summary>
        public string ComingIncomeText => ComingIncome.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 累积发贴上限
        /// </summary>
        public int PostLimit { get; set; }

        /// <summary>
        /// 剩余发贴次数
        /// </summary>
        public int PostRemain { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal RewardsPoint { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public string RewardsPointText => RewardsPoint.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 钻石钱包余额
        /// </summary>
        public decimal Point { get; set; }

        /// <summary>
        /// 钻石钱包余额
        /// </summary>
        public string PointText => Point.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 主账户余额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 主账户余额
        /// </summary>
        public string AmountText => Amount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 区域數量
        /// </summary>
        public AdminSquareQuantity[] Quantity { get; set; } = new AdminSquareQuantity[0];

        /// <summary>
        /// 会员卡资讯
        /// </summary>
        public AdminCardDetail[] CardDetails { get; set; } = new AdminCardDetail[0];

        //会员卡1日1发
        public int VipDailyOnce { get; set; }
        public int PlatformSharing { get; set; } = 0;
        public string PlatformSharingText => UserIdentity == IdentityType.SuperBoss && PlatformSharing >= 0 ? PlatformSharing.ToString() + "%" : UserIdentity == IdentityType.SuperBoss ? "0%" : "-%";
        public string SuperBossSharing => UserIdentity == IdentityType.SuperBoss && PlatformSharing >= 0 ? (100 - PlatformSharing).ToString() + "%" : UserIdentity == IdentityType.SuperBoss ? "0%" : "-%";
    }

    public class AdminSquareQuantity
    {
        /// <summary>
        /// 已发贴
        /// </summary>
        public int Post { get; set; }

        /// <summary>
        /// 发贴待审
        /// </summary>
        public int PostReview { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 评价待审
        /// </summary>
        public int CommentReview { get; set; }

        /// <summary>
        /// 解锁次数
        /// </summary>
        public int Unlock { get; set; }

        /// <summary>
        /// 被解锁次数
        /// </summary>
        public int Unlocked { get; set; }

        /// <summary>
        /// 累积收益
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 累积收益
        /// </summary>
        public string IncomeText => Income.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 区域
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string PostTypeText => PostType.GetDescription();

        /// <summary>
        /// 预约次数
        /// </summary>
        public int BookCount { get; set; }

        /// <summary>
        /// 被预约次数
        /// </summary>
        public int BookedCount { get; set; }

        /// <summary>
        /// 预约中订单数
        /// </summary>
        public int BookInProgressCount { get; set; }
    }

    public class AdminCardDetail
    {
        public string CardName { get; set; }

        /// <summary>
        /// 今日广场免费解锁次数
        /// </summary>
        public int FreeUnlockCount { get; set; }

        /// <summary>
        /// 解锁折扣
        /// </summary>
		public decimal UnlockDiscount { get; set; }

        /// <summary>
        /// 预约折扣
        /// </summary>
        public decimal BookingDiscount { get; set; }
    }
}