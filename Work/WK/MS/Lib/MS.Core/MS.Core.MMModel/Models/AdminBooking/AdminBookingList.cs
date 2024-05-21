using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminBooking
{
    public class AdminBookingList
    {
        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 预约会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string BookingTimeText => BookingTime == DateTime.MinValue ? "-" : BookingTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime AcceptTime { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public string AcceptTimeText => AcceptTime == DateTime.MinValue ? "-" : AcceptTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 确认完成时间
        /// </summary>
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string FinishTimeText => FinishTime == DateTime.MinValue ? "-" : FinishTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CancelTimeText => CancelTime == DateTime.MinValue ? "-" : CancelTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 支付类型
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string PaymentTypeText
        {
            get
            {
                switch (PaymentType)
                {
                    case 1:
                        if (Status == 4)
                            return "退还预约金";
                        return "预约金";

                    case 2:
                        if (Status == 4)
                            return "退还全额";
                        return "全额";

                    default:
                        return "-";
                }
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
        /// 折扣
        /// </summary>
        public string DiscountText => Discount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string ActualPaymentMoneyText => PaymentMoney.ToString("0");

        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public string BookingStatusText
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "待接单";

                    case 1:
                        return "服务中";

                    case 2:
                        return "服务完成";

                    case 3:
                        return "服务完成";

                    case 4:
                        return "服务完成";

                    case 5:
                        return "服务完成";

                    case 6:
                        return "订单已取消";

                    case 7:
                        return "超时未接单";

                    case 8:
                        return "申请退款中";

                    case 9:
                        return "退款成功";

                    case 10:
                        return "服务完成";

                    default:
                        return "-";
                }
            }
        }
        /// <summary>
        /// 用户身份
        /// </summary>
        public IdentityType CurrentIdentity { get; set; }

        public string UserIdentityText => ((IdentityType)CurrentIdentity).GetDescription();
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatusText
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "服务中";

                    case 2:
                        return "已完成";

                    case 3:
                        return "退款中";

                    case 4:
                        return "已退款";

                    default:
                        return "-";
                }
            }
        }
    }
}