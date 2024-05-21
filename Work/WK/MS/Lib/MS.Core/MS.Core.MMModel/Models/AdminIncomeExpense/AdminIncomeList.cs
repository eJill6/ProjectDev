using MS.Core.MM.Extensions;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    public class AdminIncomeList
    {
        /// <summary>
        /// 收益单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string CategoryText => Category.ConvertToPostType()?.GetDescription() ?? string.Empty;

        /// <summary>
        /// 贴子区域
        /// </summary>
        public IncomeExpenseCategoryEnum Category { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 解锁单ID
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 预约单ID
        /// </summary>
        public string BookingId { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 应入账时间
        /// </summary>
        public string ApplyTimeText => CreateTime.AddHours(MMGlobalSettings.BaseDispatchHours).ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 入账时间
        /// </summary>
        public DateTime? DistributeTime { get; set; }

        public string DistributeTimeText => DistributeTime.HasValue ? DistributeTime.Value.ToString(GlobalSettings.DateTimeFormat) : "-";

        /// <summary>
        /// 是否到期
        /// </summary>
        public bool IsOntime => CreateTime.AddHours(MMGlobalSettings.BaseDispatchHours) <= DateTime.Now;

        /// <summary>
        /// 锁定状态
        /// </summary>
        public string LockTypeText => IsOntime ? "已到期" : "未到期";

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

                    case IncomeExpenseStatusEnum.ReportUnDispatched:
                        return "不入账";

                    case IncomeExpenseStatusEnum.Reject:
                        return "审核不入账";

                    case IncomeExpenseStatusEnum.Unusual:
                        return "异常";

                    default:
                        return "-";
                }
            }
        }

        /// <summary>
        /// 實際解鎖鑽石
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public decimal Rebate { get; set; }

        /// <summary>
        /// 注解
        /// </summary>
        public string UnusualMemo { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string PointText => Amount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 暂锁收益
        /// </summary>
        public string AmountText
        {
            get
            {
                if ((Category == IncomeExpenseCategoryEnum.Square || Category == IncomeExpenseCategoryEnum.Agency) &&
                    (ReportStatus == 1 || Status == IncomeExpenseStatusEnum.Refund))
                    return "0.00";
                if (Status == IncomeExpenseStatusEnum.UnDispatched)
                {
                    return Math.Floor(Amount * Rebate).ToString();
                }
                return "-";
            }
        }

        /// <summary>
        /// 入账收益
        /// </summary>
        public string IncomeAmountText
        {
            get
            {
                if (Status == IncomeExpenseStatusEnum.Approved ||
                    Status == IncomeExpenseStatusEnum.Dispatched)
                {
                    return Math.Floor(Amount * Rebate).ToString();
                }
                return "-";
            }
        }

        /// <summary>
        /// 來源編號(卡片解鎖編號)
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 投诉单状态
        /// </summary>
        public int ReportStatus { get; set; }
        /// <summary>
        /// 用户身份
        /// </summary>
        public IdentityType CurrentIdentity { get; set; }

        /// <summary>
        /// 用户身份描述
        /// </summary>
        public string UserIdentityText => CurrentIdentity.GetDescription();

    }
}