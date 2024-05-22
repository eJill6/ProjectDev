using MS.Core.Infrastructures.ZeroOne.Models.Requests;

namespace FakeMSSeal.Models.ZeroOne
{
    public class ZOPointDapiReq : MS.Core.Infrastructures.ZeroOne.Models.Requests.ZOPointDapiReq
    {
        public ZOPointDapiReq(ZOIncomeExpenseCategory categoryId, int userId, decimal point, string memo) : base(categoryId, userId, point, memo)
        {
        }

        public new string Source { get; set; } = string.Empty;

        public long Ts { get; set; } = 0;
    }
}
