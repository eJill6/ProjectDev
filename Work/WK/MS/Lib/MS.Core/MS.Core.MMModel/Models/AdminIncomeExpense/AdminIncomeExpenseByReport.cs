using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    /// <summary>
    /// 透過舉報來取的對應的支出單
    /// </summary>
    public class AdminIncomeExpenseByReport
    {
        /// <inheritdoc cref="IncomeExpenseCategoryEnum"/>
        public IncomeExpenseCategoryEnum Category { get; set; }

        /// <summary>
        /// 解鎖單單號
        /// </summary>
        public string SoruceId { get; set; }

        /// <inheritdoc cref="IncomeExpenseTransactionTypeEnum"/>
        public IncomeExpenseTransactionTypeEnum TransactionType { get; set; } = IncomeExpenseTransactionTypeEnum.Expense;
    }
}