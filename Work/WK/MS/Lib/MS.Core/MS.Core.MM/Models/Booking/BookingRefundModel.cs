using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking
{
    public class BookingRefundModel
    {
        /// <summary>
        /// 更新狀態
        /// </summary>
        public BookingStatus Status { get; set; }
        /// <summary>
        /// 取消時間
        /// </summary>
        public DateTime CancelTime { get; set; }
        /// <summary>
        /// 原始狀態
        /// </summary>
        public BookingStatus OriginalStatus { get; set; }
        /// <summary>
        /// 預約單號
        /// </summary>
        public string BookingId { get; set; }
    }
}
