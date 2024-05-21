using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminBooking
{
    public class AdminBookingRefundDetail
    {
        /// <summary>
        /// 退款申请ID
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
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

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
                        return "預約";
                    case 2:
                        return "全額";
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
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 实际支付金額
        /// </summary>
        public string ActualPaymentMoneyText => PaymentMoney.ToString("0");

        /// <summary>
        /// 申请理由
        /// </summary>
        public byte ReasonType { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string ReasonTypeText
        {
            get
            {
                switch (ReasonType)
                {
                    case 1:
                        return "欺騙";
                    case 2:
                        return "貨不對版";
                }
                return "-";
            }
        }

        /// <summary>
        /// 申请原因描述
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyTimeText => ApplyTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 截图证据照片
        /// </summary>
        public string[] ProofPics { get; set; }

        /// <summary>
        /// 審核狀態。0：審核中、1：通過、2：不通過
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
                        return "審核中";
                    case 1:
                        return "通過";
                    case 2:
                        return "不通過";
                }
                return "-";
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 发帖者身份
        /// </summary>
        public string UserIdentityText => UserIdentity.GetDescription();
        public IdentityType UserIdentity { get; set; }
    }
}
