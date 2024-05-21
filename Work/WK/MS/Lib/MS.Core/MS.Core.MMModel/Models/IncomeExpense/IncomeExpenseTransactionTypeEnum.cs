namespace MS.Core.MMModel.Models.IncomeExpense
{
    /// <summary>
    /// 支出收益的方向, Expense 或 Income
    /// </summary>
    public enum IncomeExpenseTransactionTypeEnum : byte
    {
        /// <summary>
        /// 支出
        /// </summary>
        Expense = 1,
        /// <summary>
        /// 收益
        /// </summary>
        Income = 2,
        /// <summary>
        /// 退款
        /// </summary>
        Refund = 3,
    }

}
