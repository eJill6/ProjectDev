using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Entities.Post
{
    public interface IBookingPaymentStatus
    {
        BookingPaymentType PaymentType { get; set; }
        BookingStatus Status { get; set; }
        decimal PaymentMoney { get; set; }
    }
}
