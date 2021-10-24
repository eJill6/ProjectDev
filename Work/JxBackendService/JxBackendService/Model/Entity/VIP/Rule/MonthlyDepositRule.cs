
namespace JxBackendService.Model.Entity.VIP.Rule
{
    public class MonthlyDepositRule
    {
        public decimal BonusRate { get; set; }
        public int JoinTimes { get; set; }
        public int JoinFrequencyType { get; set; }
        public decimal MaxGiftMoney { get; set; }
    }
}