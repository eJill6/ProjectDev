namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOFirstBuyReq : ZOTransactionReqBase
    {
        public ZOFirstBuyReq(ZOIncomeExpenseCategory categoryId, int userId, decimal amount, string memo) : base(categoryId, userId, memo)
        {
            Amount = amount;
        }
        public decimal Amount { get; set; }
    }
}
