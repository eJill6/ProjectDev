namespace JxBackendService.Model.ThirdParty.Base
{
    public class IMLotteryBetLog : BaseRemoteBetLog
    {
        public string Provider { get; set; }

        public string GameId { get; set; }

        public string GameName { get; set; }

        public string ChineseGameName { get; set; }

        public string GameNo { get; set; }

        public string GameNoId { get; set; }

        public string PlayerId { get; set; }

        public string ProviderPlayerId { get; set; }

        public string Currency { get; set; }

        public string Tray { get; set; }

        public string BetId { get; set; }

        public string BetOn { get; set; }

        public string BetType { get; set; }

        public string BetDetails { get; set; }

        public string Odds { get; set; }

        public decimal BetAmount { get; set; }

        public decimal ValidBet { get; set; }

        public decimal WinLoss { get; set; }

        public decimal PlayerWinLoss { get; set; }

        public decimal LossPrize { get; set; }

        public decimal Tips { get; set; }

        public decimal CommissionRate { get; set; }

        public decimal Commission { get; set; }

        public string Status { get; set; }

        public string Platform { get; set; }

        public string BetDate { get; set; }

        public string ResultDate { get; set; }

        public string SettlementDate { get; set; }

        public string ReportingDate { get; set; }

        public string DateCreated { get; set; }

        public string LastUpdatedDate { get; set; }

        public override string KeyId => $"{BetId}_{GameNoId}";

        public override string TPGameAccount => PlayerId;
    }
}