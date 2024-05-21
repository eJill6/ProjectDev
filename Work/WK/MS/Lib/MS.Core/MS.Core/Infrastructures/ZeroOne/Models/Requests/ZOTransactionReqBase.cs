namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOTransactionReqBase
    {
        public ZOTransactionReqBase(ZOIncomeExpenseCategory categoryId, int userId, string memo)
        {
            UserId = userId;
            CategoryId = categoryId;
            Memo = memo;
        }

        public string Source { get; } = "mm";
        public ZOIncomeExpenseCategory CategoryId { get; set; }
        public int UserId { get; set; }

        public string Memo { get; set; }
    }

}