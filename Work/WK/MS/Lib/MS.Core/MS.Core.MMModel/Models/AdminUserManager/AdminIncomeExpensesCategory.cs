using System.ComponentModel;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public enum AdminIncomeExpensesCategory
    {
        /// <summary>
        /// 贴子解鎖
        /// </summary>
        [Description("贴子解锁")]
        PostUnLock = 0,
        /// <summary>
        /// 支付预约金
        /// </summary>
        [Description("支付预约金")]
        Booking = 1,
        /// <summary>
        /// 取消预约金
        /// </summary>
        [Description("退回预约金")]
        UnBooking = 2,
        /// <summary>
        /// 购买会员卡
        /// </summary>
        [Description("购买会员卡")]
        MembershipCard = 3,
        /// <summary>
        /// 贴子收益
        /// </summary>
        [Description("贴子收益")]
        PostIncome = 4,
        /// <summary>
        /// 解鎖退款
        /// </summary>
        [Description("解锁退款")]
        UnLockRefund = 5,
		/// <summary>
		/// 支付全额
		/// </summary>
		[Description("支付全额")]
		PayInFull=6,
		/// <summary>
		/// 退回全额
		/// </summary>
		[Description("退回全额")]
		FullRefund=7
	}
}