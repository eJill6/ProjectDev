using MS.Core.MMModel.Models.Booking.Enums;

namespace MS.Core.MMModel.Models.Booking
{
    public class ApplyRefundDataForClient
    {
        /// <summary>
        /// 申请退费的预约单
        /// </summary>
        public string BookingId { get; set; } = string.Empty;

        /// <summary>
        /// 退费类型
        /// </summary>
        public RefundReasonType ReasonType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; } = string.Empty;

        /// <summary>
        /// 證據圖片
        /// </summary>
        public string[] PhotoIds { get; set; } = new string[0];
    }
}