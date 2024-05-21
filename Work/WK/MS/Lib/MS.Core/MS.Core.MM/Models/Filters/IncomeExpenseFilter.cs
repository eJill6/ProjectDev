using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MM.Models.Filters
{
    public class IncomeExpenseFilter
    {
        public IEnumerable<string> Ids { get; set; }
        public IEnumerable<string> SourceIds { get; set; }
        public IEnumerable<string> TargetIds { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<IncomeExpenseCategoryEnum>? Categories { get; set; }

        public IEnumerable<IncomeExpenseTransactionTypeEnum>? TransactionTypes { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public IncomeExpensePayType? PayType { get; set; }
        public IncomeExpenseStatusEnum? Status { get; set; }
        /// <summary>
        /// 是否為0元(資料庫沒有小於0的值)
        /// </summary>
        public bool? IsZeroAmount { get; set; }
    }
}
