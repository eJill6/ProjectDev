using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MMModel.Models.Refund
{
    public class MMApplyRefundAndBookingModel
    {
        public string RefundId { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///預約單號
        /// </summary>
        public string BookingId { get; set; } = string.Empty;

        /// <summary>
        /// 退費理由類型。1：欺騙、2：貨不對版
        /// </summary>
        public byte ReasonType { get; set; }

        /// <summary>
        /// 退費理由
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 審核狀態。0：審核中、1：通過、2：不通過
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        public IdentityType CurrentIdentity { get; set; }
        /// <summary>
        /// 身份描述
        /// </summary>
        public string UserIdentityText => CurrentIdentity.GetDescription();

        /// <summary>
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
        public string PostId { get; set; }
        public decimal Discount { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string BookingTimeText => BookingTime.ToString(GlobalSettings.DateTimeFormat);
        /// <summary>
        /// 支付金額
        /// </summary>
        public decimal PaymentMoney { get; set; }

        /// <summary>
        /// 支付金額
        /// </summary>
        public string PaymentMoneyText => (PaymentMoney + Discount).ToString("0");









        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string ActualPaymentMoneyText => PaymentMoney.ToString("0");

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
        public string ApplyTimeText => ApplyTime.ToString(GlobalSettings.DateTimeFormat);


        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";


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

    }
}
