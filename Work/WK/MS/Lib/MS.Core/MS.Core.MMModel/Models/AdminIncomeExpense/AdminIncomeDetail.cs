using MS.Core.MM.Extensions;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    public class AdminIncomeDetail
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
        /// 发贴会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 解锁会员ID
        /// </summary>
        public int UnlockUserId { get; set; }

        /// <summary>
        /// 解锁单ID
        /// </summary>
        public string TargetId { get; set; }

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

        // <summary>
        /// 派發時間
        /// </summary>
        public DateTime DistributeTime { get; set; }

        public string DistributeTimeText => DistributeTime == DateTime.MinValue ? "-" : DistributeTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 是否到期
        /// </summary>
        public bool IsOntime => CreateTime.AddHours(MMGlobalSettings.BaseDispatchHours) <= DateTime.Now;

        /// <summary>
        /// 锁定状态
        /// </summary>
        public string LockStatusText => IsOntime ? "已到期" : "未到期";

        /// <summary>
        /// 解锁方式
        /// </summary>
        public string UnlockTypeText {
            get
            {
                if (Rebate == 1.0M || Rebate == 0.06M)
                {
                    return "一般";
                }
                else if (Rebate == 0.0M)
                {
                    return "免费";
                }
                return "打折";
            }
        }


        /// <summary>
        /// 鑽石狀態状态
        /// </summary>
        public string DiamondStatusText
        {
            get
            {
                if (IsOntime)
                {
                    if (Status == IncomeExpenseStatusEnum.Dispatched)
                    {
                        return "入账";
                    }
                    else if (Status == IncomeExpenseStatusEnum.Approved)
                    {
                        return "审核入账";
                    }
                    else if (Status == IncomeExpenseStatusEnum.Reject)
                    {
                        return "审核不入账";
                    }

                    return "异常";
                }
                return "暂锁";
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
					case IncomeExpenseStatusEnum.ReportUnDispatched:
						return "不入账";
					case IncomeExpenseStatusEnum.Reject:
                        return "审核不入账";
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
        /// 審核原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        public int ReportStatus { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string PointText => Amount.ToString("0");

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
                    return (Amount * Rebate).ToString(GlobalSettings.AmountFormat);
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
                    return (Amount * Rebate).ToString(GlobalSettings.AmountFormat);
                }
                return "-";
            }
        }

        /// <summary>
        /// 來源編號(卡片解鎖編號)
        /// </summary>
        public string SourceId { get; set; }
    }
}
