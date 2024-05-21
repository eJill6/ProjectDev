namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public enum ZOIncomeExpenseCategory
    {
        /// <summary>
        /// 买会员卡(扣餘額)
        /// </summary>
        BuyMembershipCard = 16,
        /// <summary>
        /// 解锁收益(加餘額)
        /// </summary>
        UnlockEarnings = 17,
        /// <summary>
        /// 解锁贴子(扣鑽)
        /// </summary>
        UnlockPost = 18,
        /// <summary>
        /// 解锁违规退款(加鑽石)
        /// </summary>
        UnlockRefund = 19,
        /// <summary>
        /// 官方預約(扣鑽)
        /// </summary>
        Booking = 21,
        /// <summary>
        /// 預約退費(加鑽) 
        /// </summary>
        UnBooking = 22,
        /// <summary>
        /// 預約收益(加餘額)
        /// </summary>
        BookingEarnings = 21,
    }
}
