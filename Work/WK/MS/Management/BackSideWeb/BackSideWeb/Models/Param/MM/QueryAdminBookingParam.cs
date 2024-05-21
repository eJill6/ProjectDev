using JxBackendService.Model.Paging;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryAdminBookingParam : BasePagingRequestParam
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
        /// 订单状态
        /// </summary>
        public int? OrderStatus { get; set; }

        /// <summary>
        /// 商户預約狀態
        /// </summary>
        public int? BookingStatus { get; set; }

        /// <summary>
        /// 0: 下单时间，1: 接单时间，2: 确认完成时间，3: 取消订单时间
        /// </summary>
        public int DateTimeType { get; set; }
        /// <summary>
        /// 根据用户身份查询
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
