using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Req
{
    public class ReqManageBooking
    {
        public MyBookingStatus Status { get; set; }

        /// <summary>
        /// 預約用戶 Id
        /// </summary>
        public int UserId { get; set; }

        public int PageNo { get; set; }
    }
}