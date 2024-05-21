using MS.Core.MM.Models.Entities.PostTransaction;

namespace MS.Core.MM.Models.Booking
{
    public class BookingDistributeModel
    {
        public BookingDistribute Booking { get; set; }
        public MMIncomeExpenseModel Income { get; set; } = null!;
        public MMIncomeExpenseModel Expense { get; set; } = null!;
    }
}
