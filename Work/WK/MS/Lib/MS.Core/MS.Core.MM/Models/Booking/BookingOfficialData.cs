using MS.Core.MM.Models.Booking.Enums;
using System.ComponentModel.DataAnnotations;

namespace MS.Core.MM.Models.Booking
{
    /// <summary>
    /// 預約官方
    /// </summary>
    public class BookingOfficialData
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        [Required(ErrorMessage = "请填写 {0} 这个栏位")]
        public string? PostId { get; set; }

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