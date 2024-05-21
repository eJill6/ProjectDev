using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;

namespace MS.Core.MM.Models.Booking
{
    public class RefundModel
    {
        public RefundModel(BookingRefundModel booking, MMIncomeExpenseModel mMIncomeExpenseModel)
        {
            Booking = booking;
            MMIncomeExpenseModel = mMIncomeExpenseModel;
        }

        public RefundModel(BookingRefundModel booking, MMIncomeExpenseModel mMIncomeExpenseModel, MMApplyRefund applyRefundModel)
        {
            Booking = booking;
            MMIncomeExpenseModel = mMIncomeExpenseModel;
            ApplyRefundModel = applyRefundModel;
        }

        public BookingRefundModel Booking { get; set; } = null!;
        public MMIncomeExpenseModel MMIncomeExpenseModel { get; set; } = null!;
        public MMApplyRefund ApplyRefundModel { get; set; } = null!;
    }
}
