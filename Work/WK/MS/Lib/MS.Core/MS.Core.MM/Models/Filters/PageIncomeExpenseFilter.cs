using MS.Core.MM.Models.IncomeExpense;
using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Filters
{
    public class PageIncomeExpenseFilter : IncomeExpenseFilter
    {
        public PaginationModel Pagination { get; set; }
    }
}
