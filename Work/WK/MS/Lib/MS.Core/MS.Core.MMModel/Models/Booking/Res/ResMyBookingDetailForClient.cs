namespace MS.Core.MM.Models.Booking.Res
{
    public class ResMyBookingDetailForClient : ResMyBookingForClient
    {
        public string BookingTime { get; set; }
        public string AcceptTime { get; set; }
        public string FinishTime { get; set; }
        public string Contact { get; set; }
        public string Memo { get; set; }
    }
}