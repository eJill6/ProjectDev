using JxBackendService.Model.Entity.Base;
using MS.Core.Models.Models;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMAdminBookingRefundBs
    {
        /// <summary>
        /// 预约退款ID
        /// </summary>
        public string RefundId { get; set; }

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
        /// 下单时间
        /// </summary>
        public string BookingTimeText { get; set; }

        ///<summary>
        /// 支付类型
        /// </summary>
        public string PaymentTypeText { get; set; }

        /// <summary>
        /// 支付金額
        /// </summary>
        public string PaymentMoneyText { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string ActualPaymentMoneyText { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string ApplyReasonText { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyTimeText { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText { get; set; }

        /// <summary>
        /// 審核狀態。0：審核中、1：通過、2：不通過
        /// </summary>
        public byte Status { get; set; }

        public string StatusText { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 发帖者身份
        /// </summary>
        public string UserIdentityText { get; set; }
    }
}