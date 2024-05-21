namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOPointIncomeExpenseReq
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <param name="point"></param>
        /// <param name="memo">收益支出單號</param>
        public ZOPointIncomeExpenseReq(ZOIncomeExpenseCategory categoryId, int userId, decimal point, string memo)
        {
            CategoryId = categoryId;
            UserId = userId;
            Point = point;
            Memo = memo;
        }
        public ZOIncomeExpenseCategory CategoryId { get; set; }
        public int UserId { get; set; }
        public decimal Point { get; set; }

        public string Memo { get; set; }
    }
}
