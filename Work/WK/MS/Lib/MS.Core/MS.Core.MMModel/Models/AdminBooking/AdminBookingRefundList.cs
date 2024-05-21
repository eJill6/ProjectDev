using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.AdminBooking
{
    public class AdminBookingRefundList
    {
        /// <summary>
        /// 退款ID
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 预约会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string BookingTimeText => BookingTime.ToString(GlobalSettings.DateTimeFormat);

        ///<summary>
        /// 支付类型
        /// </summary>
        public int PaymentType { get; set; }

        ///<summary>
        /// 支付类型
        /// </summary>
        public string PaymentTypeText
        {
            get
            {
                switch (PaymentType)
                {
                    case 1:
                        return "预约";
                    case 2:
                        return "全额";
                }
                return "-";
            }
        }


        /// <summary>
        /// 支付金額
        /// </summary>
        public decimal PaymentMoney { get; set; }

        /// <summary>
        /// 支付金額
        /// </summary>
        public string PaymentMoneyText => (PaymentMoney + Discount).ToString("0");

        /// <summary>
        /// 折扣金額
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string ActualPaymentMoneyText => PaymentMoney.ToString("0");

        /// <summary>
        /// 申请理由
        /// </summary>
        public byte ReasonType { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string ApplyReasonText
        {
            get
            {
                switch (ReasonType)
                {
                    case 1:
                        return "欺骗";
                    case 2:
                        return "货不对版";
                }
                return "-";
            }
        }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyTimeText => ApplyTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 審核狀態。0：审核中、1：通过、2：不通过
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "审核中";
                    case 1:
                        return "通过";
                    case 2:
                        return "不通过";
                }
                return "-";
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
