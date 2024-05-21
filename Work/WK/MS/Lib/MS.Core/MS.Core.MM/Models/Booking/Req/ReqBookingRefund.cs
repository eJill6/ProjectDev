namespace MS.Core.MM.Models.Booking.Req
{
    public class ReqBookingRefund : ApplyRefundData
    {
        /// <summary>
        /// 用戶id
        /// </summary>
        public int UserId { get; set; }
    }
}