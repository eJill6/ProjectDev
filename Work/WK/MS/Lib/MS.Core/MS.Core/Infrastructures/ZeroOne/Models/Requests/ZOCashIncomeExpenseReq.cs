namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOCashIncomeExpenseReq
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <param name="memo">收益支出單號</param>
        public ZOCashIncomeExpenseReq(ZOIncomeExpenseCategory categoryId, int userId, decimal amount, string memo)
        {
            CategoryId = categoryId;
            UserId = userId;
            Amount = amount;
            Memo = memo;
        }
        public ZOIncomeExpenseCategory CategoryId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }

        public string Memo { get; set; }
    }
}
