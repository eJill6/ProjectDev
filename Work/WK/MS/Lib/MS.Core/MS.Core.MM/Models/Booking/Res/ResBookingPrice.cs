namespace MS.Core.MM.Models.Booking.Res
{
    public class ResBookingPrice
    {
        public int PriceId { get; set; }

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string ComboName { get; set; } = string.Empty;

        /// <summary>
        /// 套餐價格(餘額)
        /// </summary>
        public string ComboPrice { get; set; } = null!;

        /// <summary>
        /// 全額價格(鑽石)
        /// </summary>
        public string FullPrice { get; set; } = null!;
        /// <summary>
        /// 預約價格(鑽石)
        /// </summary>

        public string BookingPrice { get; set; } = null!;

        /// <summary>
        /// 服務時間、次數或包含項目
        /// </summary>
        public string Service { get; set; } = string.Empty;
    }
}
