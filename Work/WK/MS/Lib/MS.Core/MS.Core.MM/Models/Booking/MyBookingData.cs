using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking
{
    public class MyBookingData
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public BookingStatus Status { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        public int PageNo { get; set; }
    }
}