namespace MS.Core.MM.Models.Booking.Res
{
    public class ResBooking
    {
        public string BookingId { get; set; } = null!;

        public string PhoneNo { get; set; }

        /// <summary>
        /// 國碼
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 內容參數
        /// </summary>
        public string ContentParamInfo { get; set; }
    }
}
