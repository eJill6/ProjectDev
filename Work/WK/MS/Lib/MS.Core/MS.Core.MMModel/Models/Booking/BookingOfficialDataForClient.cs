using MS.Core.MMModel.Models.Booking.Enums;
using System.ComponentModel.DataAnnotations;

namespace MS.Core.MMModel.Models.Booking
{
    public class BookingOfficialDataForClient
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        [Required(ErrorMessage = "请填写 {0} 这个栏位")]
        public string PostId { get; set; }

        /// <summary>
        /// 发帖人原身份
        /// </summary>
        public int PostUserIdentity { get; set; }

        /// <summary>
        /// 官方贴價格ID
        /// </summary>
        public int PostPriceId { get; set; }

        /// <summary>
        /// 支付方式。1：預約支付、2：全額支付
        /// </summary>
        public BookingPaymentTypeForClient PaymentType { get; set; }
    }
}