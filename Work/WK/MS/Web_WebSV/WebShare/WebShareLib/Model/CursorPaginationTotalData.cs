namespace SLPolyGame.Web.Model
{
    public class CursorPaginationTotalData<T> : CursorPagination<T>
    {
        public int TotalBetCount { get; set; }
        
        public decimal TotalPrizeMoney { get; set; }
        
        public decimal TotalWinMoney { get; set; }
    }
}