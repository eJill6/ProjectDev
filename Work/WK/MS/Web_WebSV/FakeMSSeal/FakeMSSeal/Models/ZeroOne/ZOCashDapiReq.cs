using MS.Core.Infrastructures.ZeroOne.Models.Requests;

namespace FakeMSSeal.Models.ZeroOne
{
    public class ZOCashDapiReq : MS.Core.Infrastructures.ZeroOne.Models.Requests.ZOCashDapiReq
    {
        public ZOCashDapiReq(ZOIncomeExpenseCategory categoryId, int userId, decimal amount, string memo) : base(categoryId, userId, amount, memo)
        {
        }

        public long Ts { get; set; } = 0;
    }
}
