using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.IM
{
    public class IMKYBetLog : BaseRemoteBetLog
    {
        public string Provider { get; set; }
        public string GameId { get; set; }
        public string GameName { get; set; }
        public string ChineseGameName { get; set; }
        public string BetId { get; set; }
        public string RoundId { get; set; }
        public string PlayerId { get; set; }
        public string ProviderPlayerId { get; set; }
        public string Currency { get; set; }
        public decimal BetAmount { get; set; }
        public decimal ValidBet { get; set; }
        public decimal WinLoss { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }
        public decimal ProviderBonus { get; set; }
        public string Status { get; set; }
        public string Platform { get; set; }
        public string Remarks { get; set; }
        public string DateCreated { get; set; }
        public string GameDate { get; set; }
        public string GameEndDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string BetDate { get; set; }
        public string SettlementDate { get; set; }
        public string ReportingDate { get; set; }

        public override string KeyId => $"{BetId}_{RoundId}";

        public override string TPGameAccount => PlayerId;
    }
}