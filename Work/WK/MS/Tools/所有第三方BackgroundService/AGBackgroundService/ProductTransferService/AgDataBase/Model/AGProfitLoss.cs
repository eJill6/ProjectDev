namespace ProductTransferService.AgDataBase.Model
{
    public class AGProfitLoss
    {
        public int ProfitLossID { get; set; }

        public int UserID { get; set; }

        public string UserPaths { get; set; }

        public DateTime ProfitLossTime { get; set; }

        public string ProfitLossType { get; set; }

        public decimal ProfitLossMoney { get; set; }

        public decimal WinMoney { get; set; }

        public decimal PrizeMoney { get; set; }

        public string Memo { get; set; }

        public string PalyID { get; set; }
    }
}