using System.Drawing;

namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOPointDapiReq : ZOTransactionReqBase
    {
        public ZOPointDapiReq(ZOIncomeExpenseCategory categoryId, int userId, decimal point, string memo) : base(categoryId, userId, memo)
        {
            Point = point;
        }

        public decimal Point { get; set; }
    }
}
