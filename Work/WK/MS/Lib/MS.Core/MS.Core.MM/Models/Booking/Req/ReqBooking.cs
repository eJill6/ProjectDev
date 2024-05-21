using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Req
{
    public class ReqBooking
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = null!;

        /// <summary>
        /// 預約用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 官方贴價格ID
        /// </summary>
        public int PostPriceId { get; set; }

        /// <summary>
        /// 支付方式。1：預約支付、2：全額支付
        /// </summary>
        public BookingPaymentType PaymentType { get; set; }

        /// <summary>
        /// 发帖人原身份
        /// </summary>
        public int PostUserIdentity { get; set; }
    }
}