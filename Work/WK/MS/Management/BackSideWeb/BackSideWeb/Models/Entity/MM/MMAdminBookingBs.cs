using JxBackendService.Model.Entity.Base;
using MS.Core.Models.Models;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMAdminBookingBs
    {
        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 预约会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string BookingTimeText { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public string AcceptTimeText { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string FinishTimeText { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CancelTimeText { get; set; }

        ///<summary>
        /// 支付类型
        /// </summary>
        public string PaymentTypeText { get; set; }

        /// <summary>
        /// 支付金額
        /// </summary>
        public string PaymentMoneyText { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string DiscountText { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string ActualPaymentMoneyText { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public string BookingStatusText { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatusText { get; set; }
    }
}