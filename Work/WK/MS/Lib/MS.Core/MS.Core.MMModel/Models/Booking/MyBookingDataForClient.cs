using MS.Core.MMModel.Models.Booking.Enums;

namespace MS.Core.MMModel.Models.Booking
{
    public class MyBookingDataForClient
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public MyBookingStatusForClient Status { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        public int PageNo { get; set; }
    }
}