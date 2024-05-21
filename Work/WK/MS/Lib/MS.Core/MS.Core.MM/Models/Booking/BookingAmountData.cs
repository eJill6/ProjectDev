namespace MS.Core.MM.Models.Booking
{
    /// <summary>
    /// 預約金相關資料
    /// </summary>
    public class BookingAmountData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalBookingAmount">原始預約金</param>
        /// <param name="bookingAmount">實際預約金</param>
        /// <param name="paymentMoney">支付金額</param>
        /// <param name="discount">折扣</param>
        public BookingAmountData(decimal originalBookingAmount, decimal bookingAmount, decimal fullAmount, decimal discount)
        {
            OriginalBookingAmount = originalBookingAmount;
            BookingAmount = bookingAmount;
            FullAmount = fullAmount;
            Discount = discount;
        }
        /// <summary>
        /// 原始預約金(鑽石)
        /// </summary>
        public decimal OriginalBookingAmount { get; set; }
        /// <summary>
        /// 實際預約金(鑽石)
        /// </summary>
        public decimal BookingAmount { get; set; }

        /// <summary>
        /// 全額金額(鑽石)
        /// </summary>
        public decimal FullAmount { get; set; }
        /// <summary>
        /// 折扣(鑽石)
        /// </summary>
        public decimal Discount { get; set; }
    }
}
