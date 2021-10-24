namespace JxBackendService.Model.ViewModel.Game
{
    public class PlatformTotalProfitloss
    {
        public decimal Charge { get; set; }
        public decimal Withdraw { get; set; }
        public decimal BetAmount { get; set; }
        public decimal Rebate { get; set; }
        public decimal ProfitLoss { get; set; }
    }

    public class PlatformTotalProfitlossStat : PlatformScoreStat
    {
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }
        public decimal AllMoney { get; set; }
        public decimal BetMoney { get; set; }
        public decimal AllPctMoney { get; set; }
        public decimal ProfitLoss { get; set; }
        public decimal AllProfitLoss { get; set; }
        
    }

    public class PlatformScoreStat
    {
        public decimal SumAvailableScores { get; set; }
        public decimal SumFreezeScores { get; set; }
    }
}
