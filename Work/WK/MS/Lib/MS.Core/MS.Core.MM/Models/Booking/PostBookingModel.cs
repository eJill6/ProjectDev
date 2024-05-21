using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;

namespace MS.Core.MM.Models.Booking
{
    public class PostBookingModel
    {
        public MMBooking MMBooking { get; set; } = null!;
        public MMIncomeExpenseModel MMIncomeExpenseModel { get; set; } = null!;
    }
}
