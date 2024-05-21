using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminBooking
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminBookingRefundListParam : PageParam
    {
        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 预约会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 支付类型。1：預約支付、2：全額支付
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public int? ReasonType { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        public int? UserIdentity { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
