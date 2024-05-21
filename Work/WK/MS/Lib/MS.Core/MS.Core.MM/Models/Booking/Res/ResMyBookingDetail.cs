namespace MS.Core.MM.Models.Booking.Res
{
    public class ResMyBookingDetail : ResMyBooking
    {
        public string BookingTime { get; set; } = null!;
        public string AcceptTime { get; set; } = null!;
        public string FinishTime { get; set; } = null!;
        public string? Contact { get; set; }
        public string Memo { get; set; }
    }
}
