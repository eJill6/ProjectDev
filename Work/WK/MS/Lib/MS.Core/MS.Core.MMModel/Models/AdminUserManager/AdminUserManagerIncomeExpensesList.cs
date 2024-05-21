using MS.Core.MM.Extensions;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using System;
using System.Data;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerIncomeExpensesList
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账变时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Rebate { get; set; }

        /// <summary>
        /// 账变时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 收付行為
        /// </summary>
        public IncomeExpenseCategoryEnum Category { get; set; }

       // /// <summary>
       // /// 收付行為
       // /// </summary>
       // public string CategoryText
       // {
       //     get
       //     {
       //         switch (Category)
       //         {
       //             case IncomeExpenseCategoryEnum.Square:
       //             case IncomeExpenseCategoryEnum.Agency:
       //             case IncomeExpenseCategoryEnum.Experience:
       //                 switch(TransactionType)
       //                 {
       //                     case IncomeExpenseTransactionTypeEnum.Income:
       //                         return "贴子收益";
       //                     case IncomeExpenseTransactionTypeEnum.Refund:
       //                         return "解锁退款";
       //                     case IncomeExpenseTransactionTypeEnum.Expense:
       //                         return "贴子解锁";
       //                     default:
       //                         return "-";
       //                 }
       //             case IncomeExpenseCategoryEnum.Official:
       //                 switch (TransactionType)
       //                 {
							//case IncomeExpenseTransactionTypeEnum.Income:
							//	return "贴子收益";
							//case IncomeExpenseTransactionTypeEnum.Refund:
       //                         return "退回预约金";
       //                     case IncomeExpenseTransactionTypeEnum.Expense:
       //                         return "支付预约金";
       //                     default:
       //                         return "-";
       //                 }
       //             //case IncomeExpenseCategoryEnum.SuperBoss:
       //             //    switch (TransactionType)
       //             //    {
       //             //        case IncomeExpenseTransactionTypeEnum.Refund:
       //             //            return "退回全额";
       //             //        case IncomeExpenseTransactionTypeEnum.Expense:
       //             //            return "支付全额";
       //             //        default:
       //             //            return "-";
       //             //    }
       //             case IncomeExpenseCategoryEnum.Vip:
       //                 return "购买会员卡";
       //             default:
       //                 return "-";
       //         }
       //     }
       // }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string PostType => Category.ConvertToPostType()?.GetDescription() ?? "-";

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 余额账变
        /// </summary>
        public string AmountText
        {
            get
            {
                if (PayType == IncomeExpensePayType.Amount)
                {
                    switch (TransactionType)
                    {
                        case IncomeExpenseTransactionTypeEnum.Income:
                        case IncomeExpenseTransactionTypeEnum.Refund:
                            {
                                return Math.Floor(Amount * Rebate).ToString();
                            }
                        case IncomeExpenseTransactionTypeEnum.Expense:
                            {
                                return Amount == 0 ? "0" : $"-{Math.Floor(Amount * Rebate)}";
                            }
                        default:
                            return "-";
                    }
                }
                return "-";
            }
        }

        /// <summary>
        /// 钻石账变
        /// </summary>
        public string PointText
        {
            get
            {
                if (PayType == IncomeExpensePayType.Point)
                {
                    switch (TransactionType)
                    {
                        case IncomeExpenseTransactionTypeEnum.Income:
                        case IncomeExpenseTransactionTypeEnum.Refund:
                            {
                                return Rebate == 0 ? Amount.ToString("0") : (Amount * Rebate).ToString("0");
                            }
                        case IncomeExpenseTransactionTypeEnum.Expense:
                            {
                                return Amount == 0 ? "0" : "-" + (Amount * Rebate).ToString("0");
                            }
                        default:
                            return "-";
                    }
                }
                return "-";
            }
        }

        /// <summary>
        /// 帳變
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public IncomeExpensePayType PayType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypeText => PayType.GetDescription();

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 交易類型(收入/支出)
        /// </summary>
        public IncomeExpenseTransactionTypeEnum TransactionType { get; set; }
    }
}