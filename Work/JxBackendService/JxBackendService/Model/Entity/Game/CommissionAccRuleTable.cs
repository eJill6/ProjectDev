namespace JxBackendService.Model.Entity.Game
{
    public class CommissionAccRuleTable
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public decimal MinProfitLossRange { get; set; }

        public decimal MaxProfitLossRange { get; set; }

        public byte Visible { get; set; }

        public double CommissionPercent { get; set; }
    }
}
