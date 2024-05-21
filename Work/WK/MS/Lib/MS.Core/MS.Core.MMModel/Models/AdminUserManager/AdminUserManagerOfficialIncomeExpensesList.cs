using MS.Core.MMModel.Models.IncomeExpense;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerOfficialIncomeExpensesList: AdminUserManagerIncomeExpensesList
    {
        public int PaymentType { get; set; } = 0;
        public string CategoryText { 
            get {
                
                 
                if(this.Category== IncomeExpenseCategoryEnum.Official && PaymentType>0)
                {

                    switch(this.TransactionType)
                    {
                        case IncomeExpenseTransactionTypeEnum.Income:
                            return "帖子收益";
                        case IncomeExpenseTransactionTypeEnum.Expense:
                          
                            switch(this.PaymentType)
                            {
                                case 1:
                                    return "支付预约金";
        
                                case 2:
                                    return "支付全额";
                                default:
                                    return "-";
                            }
                        case IncomeExpenseTransactionTypeEnum.Refund:
                            switch (this.PaymentType)
                            {
                                case 1:
                                    return "退回预约金";

                                case 2:
                                    return "退回全额";
                                default:
                                    return "-";
                            }
                        default:
                            return "-";
                    }
                }
                else
                {
                    switch (Category)
                    {
                        case IncomeExpenseCategoryEnum.Square:
                        case IncomeExpenseCategoryEnum.Agency:
                        case IncomeExpenseCategoryEnum.Experience:
                            switch (TransactionType)
                            {
                                case IncomeExpenseTransactionTypeEnum.Income:
                                    return "贴子收益";
                                case IncomeExpenseTransactionTypeEnum.Refund:
                                    return "解锁退款";
                                case IncomeExpenseTransactionTypeEnum.Expense:
                                    return "贴子解锁";
                                default:
                                    return "-";
                            }
                        case IncomeExpenseCategoryEnum.Vip:
                            return "购买会员卡";
                        default:
                            return "-";
                    }
                }

            } 
        }
    }
}
