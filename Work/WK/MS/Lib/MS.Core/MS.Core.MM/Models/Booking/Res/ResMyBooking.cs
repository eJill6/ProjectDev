using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Res
{
    public class ResMyBooking
    {
        public ResMyBookingPost Post { get; set; } = null!;
        public string BookingId { get; set; } = null!;
        public BookingStatus Status { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMoney { get; set; } = null!;
        public string CommentId { get; set; } = null!;
        /// <summary>
        /// 该预约单是否拒绝退款
        /// </summary>
        public bool RefusalOfRefund { get; set;} = false;
    }
}
