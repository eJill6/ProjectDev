using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Req
{
    public class ReqMyBooking
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public BookingStatus Status { get; set; }

        /// <summary>
        /// 預約用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        public int PageNo { get; set; }
    }
}