using JxBackendService.Model.Paging;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryAdminBookingRefundParam : BasePagingRequestParam
    {
        /// <summary>
        /// 预约单ID
        /// </summary>
        public string? BookingId { get; set; }

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
        /// 根据身份查询~
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
