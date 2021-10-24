
namespace JxBackendService.Model.Entity.VIP.Rule
{
    public class ChangeLevelRule
    {
        public bool HasDeposit { get; set; }
        public decimal? LevelUpPoints { get; set; }
        public decimal? StayDownPoints { get; set; }
    }
}