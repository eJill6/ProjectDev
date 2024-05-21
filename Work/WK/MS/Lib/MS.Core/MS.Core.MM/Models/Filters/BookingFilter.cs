using MS.Core.MM.Models.Booking.Enums;

namespace MS.Core.MM.Models.Filters
{
    public class BookingFilter
    {
        public int? UserId { get; set; }

        public int? PostUserId { get; set; }

        public IEnumerable<BookingStatus>? Statuses { get; set; }

        /// <summary>
        /// 贴子id
        /// </summary>
        public string[]? PostIds { get; set; } = new string[0];
        public string? PostId { get; set; }

        /// <summary>
        /// 预约支付方式
        /// </summary>
        public BookingPaymentType? PaymentType { get; set; }

        /// <summary>
        /// 是否派發
        /// </summary>
        public bool? IsDistribute { get; set; }

        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool? IsDelete { get; set; }
        /// <summary>
        /// 是否超出48小时
        /// </summary>
        public bool? IsRunOutOfTime { get; set; }
    }
}