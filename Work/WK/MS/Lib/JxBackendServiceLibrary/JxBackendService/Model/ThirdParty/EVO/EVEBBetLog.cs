using JxBackendService.Model.ThirdParty.Base;
using System;

namespace JxBackendService.Model.ThirdParty.EVO
{
    public class EVEBBetLog : EVEBRowDataBetLog
    {
        public string BetId { get; set; }
        public string MemberAccount { get; set; }
        public DateTime WagersTime { get; set; }
        public decimal BetAmount { get; set; }
        public decimal Payoff { get; set; }
        public decimal Commissionable { get; set; }
        public DateTime UpdateTime { get; set; }

        public override string KeyId => BetId;

        public override string TPGameAccount => MemberAccount;
    }

    public class EVEBRowDataBetLog : BaseRemoteBetLog
    {
        public string GameId { get; set; }
        public DateTime GameStartedAt { get; set; }
        public DateTime GameSettledAt { get; set; }
        public string GameStatus { get; set; }
        public string GameType { get; set; }
        public string TableId { get; set; }
        public string TableName { get; set; }
        public string DealerUid { get; set; }
        public string DealerName { get; set; }
        public string GameCurrency { get; set; }
        public decimal GameWager { get; set; }
        public decimal GamePayout { get; set; }
        public string GameResult { get; set; }
        public string BetCode { get; set; }
        public decimal BetStake { get; set; }
        public decimal BetPayout { get; set; }
        public DateTime BetPlacedOn { get; set; }
        public string BetDescription { get; set; }
        public string BetTransactionId { get; set; }
        //public string ParticipantScreenName { get; set; }
        //public string ParticipantCasinoId { get; set; }
        //public string ParticipantSeats { get; set; }
        //public string ParticipantConfigOverlays { get; set; }
        //public string ParticipantDecisions { get; set; }
        //public string ParticipantSessionId { get; set; }
        //public string ParticipantPlayerId { get; set; }
        //public string ParticipantCurrency { get; set; }
        //public string ParticipantSideBetPlayerPair { get; set; }
        //public string ParticipantSideBetBankerPair { get; set; }
        //public string ParticipantSideBetPerfectPair { get; set; }
        //public string ParticipantSideBetEitherPair { get; set; }
        //public string ParticipantSideBetPlayerBonus { get; set; }
        //public string ParticipantSideBetBankerBonus { get; set; }
        //public string ParticipantSideBetBonusBet { get; set; }
        //public string ParticipantSideBet5P1 { get; set; }
        //public string ParticipantSideBetPairPlus { get; set; }
        //public string ParticipantSideBet6CardBonus { get; set; }
        //public string ParticipantSideBetPairOrBetter { get; set; }
        //public string ParticipantSideBetTrips { get; set; }
        //public string ParticipantSideBetBestFive { get; set; }
        //public string ParticipantSideBetAaBonus { get; set; }
        public string WagersId { get; set; }
        //public string ParticipantAamsParticipationId { get; set; }
        //public string ParticipantAamsSessionId { get; set; }
        //public string ParticipantFreebet { get; set; }
        //public string ParticipantCasinoSessionId { get; set; }
        //public string ParticipantChannel { get; set; }
        //public string ParticipantDevice { get; set; }
        //public string ParticipantOs { get; set; }
        //public string ParticipantBetCoverage { get; set; }
        //public string ParticipantRewardBets { get; set; }
        //public string ParticipantPlayMode { get; set; }
        //public object ParticipantSideBetSuperSix { get; set; }
        //public string ParticipantQualificationSpin { get; set; }
        //public object ParticipantQualifiedAt { get; set; }
        //public string ParticipantBoxes { get; set; }
        //public string ParticipantTopUpSpins { get; set; }
        //public string ParticipantOffers { get; set; }
        //public string ParticipantUseNewBetCodes { get; set; }
        //public string ParticipantBetStakePerCard { get; set; }
        //public string ParticipantCards { get; set; }
        //public string ParticipantTotalMultiplier { get; set; }
        //public string ParticipantBonus { get; set; }
        //public string ParticipantLeftOnRoll { get; set; }
        //public string ParticipantSubType { get; set; }

        public override string KeyId => "";

        public override string TPGameAccount => "";
    }

}
