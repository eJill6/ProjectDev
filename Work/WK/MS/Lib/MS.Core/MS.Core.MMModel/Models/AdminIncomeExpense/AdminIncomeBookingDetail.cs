using MS.Core.MM.Extensions;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    public class AdminIncomeBookingDetail
    {
        /// <summary>
        /// 收益单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public string CategoryText => Category.ConvertToPostType()?.GetDescription() ?? string.Empty;

        /// <summary>
        /// 帖子区域
        /// </summary>
        public IncomeExpenseCategoryEnum Category { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 预约会员ID
        /// </summary>
        public int BookingUserId { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public string BookingTimeText => BookingTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 支付方式。1：预约支付、2：全额支付
        /// </summary>
        public int PaymentType { get; set; }

        public string PaymentTypeText
        {
            get
            {
                switch (PaymentType)
                {
                    case 1:
                        return "预约支付";

                    case 2:
                        return "全额支付";
                }
                return "-";
            }
        }

        /// <summary>
        /// 预约狀態。0：確認中、1：服務中、2：服務完成、3：取消接單、4：超時未接單
        /// </summary>
        public int BookingStatus { get; set; }

        /// <summary>
        /// 预约单状态
        /// </summary>
        public string BookingStatusText
        {
            get
            {
                switch (BookingStatus)
                {
                    case 0:
                        return "待接单";

                    case 1:
                        return "服务中";

                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 10:
                        return "服务完成";

                    case 6:
                        return "订单已取消";

                    case 7:
                    case 11:
                        return "超时未接单";

                    case 8:
                        return "申请退款中";

                    case 9:
                        return "退款成功";
                }
                return "-";
            }
        }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        public IncomeExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case IncomeExpenseStatusEnum.Approved:
                        return "审核入账";

                    case IncomeExpenseStatusEnum.UnDispatched:
                        return "暂锁";

                    case IncomeExpenseStatusEnum.Dispatched:
                        return "入账";

                    case IncomeExpenseStatusEnum.Reject:
                        return "审核不入账";

                    default:
                        return "-";
                }
            }
        }

        /// <summary>
        /// 套餐费用
        /// </summary>
        public decimal ComboPrice { get; set; }

        public string ComboPriceText => ComboPrice.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 支付金額(鑽石)
        /// </summary>
        public decimal PaymentMoney { get; set; }

        public string PaymentMoneyText => PaymentMoney.ToString(GlobalSettings.AmountToIntString);

        /// <summary>
        /// 平台拆账
        /// </summary>
        public int ChargeMoney
        {
            get
            {
                if (CurrentIdentity == IdentityType.SuperBoss && PlatformSharing.HasValue)
                {
                    var paymentMoney = (PaymentMoney / 10) - Math.Floor(PaymentMoney * (1 - PlatformSharing.Value) / 10);

                    return Convert.ToInt32(paymentMoney);
                }
                if (ComboPrice > 0 && ComboPrice <= 1500)
                {
                    return 1000;
                }
                else if (ComboPrice > 1500 && ComboPrice <= 3000)
                {
                    return 2000;
                }
                else if (ComboPrice > 3000 && ComboPrice <= 5000)
                {
                    return 4000;
                }
                else if (ComboPrice > 5000 && ComboPrice <= 8000)
                {
                    return 6000;
                }
                else if (ComboPrice >= 8000)
                {
                    return 8000;
                }
                return 0;
            }
        }

        /// <summary>
        /// 暂锁/未入账收益
        /// </summary>
        public string LockedRebatText
        {
            get
            {
                if (Status == IncomeExpenseStatusEnum.UnDispatched)
                {
                    return Math.Floor((PaymentMoney - ChargeMoney) / 10).ToString();
                }
                return "0.00";
            }
        }

        /// <summary>
        /// 商户入账收益
        /// </summary>
        public string RebateText
        {
            get
            {
                if (Status == IncomeExpenseStatusEnum.Dispatched)
                {
                    if (CurrentIdentity == IdentityType.SuperBoss && PlatformSharing.HasValue)
                    {
                        return Math.Floor(PaymentMoney * (1 - PlatformSharing.Value) / 10).ToString();
                    }
                    decimal result = Math.Floor((PaymentMoney - ChargeMoney - Discount) / 10);
                    return result < 0 ? "0.00" : result.ToString();
                }
                return "-";
            }
        }

        /// <summary>
        /// 入账時間
        /// </summary>
        public DateTime? DistributeTime { get; set; }

        public string DistributeTimeText => DistributeTime == DateTime.MinValue ? "-" : DistributeTime.Value.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 注解
        /// </summary>
        public string UnusualMemo { get; set; }

        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 预定时发帖人身份
        /// </summary>
        public IdentityType? CurrentIdentity { get; set; }

        /// <summary>
        /// 超觅老板拆占比
        /// </summary>
        public decimal? PlatformSharing { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
    }
}