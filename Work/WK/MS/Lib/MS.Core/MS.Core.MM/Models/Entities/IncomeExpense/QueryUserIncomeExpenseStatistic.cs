using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MM.Models.Entities.IncomeExpense
{
    /// <summary>
    /// User收支統計
    /// </summary>
    public class QueryUserIncomeExpenseStatistic
    {
        public IncomeExpenseCategoryEnum Category { get; set; }

        public decimal SumAmount { get; set; }
    }
}
