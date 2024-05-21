using MS.Core.MMModel.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Res
{
    public class ResMyBookingForClient
    {
        public ResMyBookingPostForClient Post { get; set; }
        public string BookingId { get; set; }
        public BookingStatusForClient Status { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMoney { get; set; }
        public string CommentId { get; set; }
        public bool RefusalOfRefund { get; set; }
    }
}