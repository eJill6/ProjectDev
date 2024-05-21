namespace MS.Core.MM.Models.Booking.Req
{
    public class ReqBookingCancel
    {
        public string BookingId { get; set; } = null!;
        public int UserId { get; set; }
    }
}
