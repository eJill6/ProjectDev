using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.BTI
{
    public class BTISBettingHistoryResponse : BTISBaseDataResponse
    {
        public List<BTISBetLog> Bets { get; set; }
    }

    public class BTISBetLog : BaseRemoteBetLog
    {
        /// <summary>游戏盈亏金额</summary>
        public decimal PL { get; set; }

        public decimal NonCashOutAmount { get; set; }

        public decimal ComboBonusAmount { get; set; }

        public DateTime? BetSettledDate { get; set; }

        public string PurchaseID { get; set; }

        public DateTime UpdateDate { get; set; }

        public decimal Odds { get; set; }

        /// <summary>赔率</summary>
        public string OddsInUserStyle { get; set; }

        /// <summary>盘口</summary>
        public string OddsStyleOfUser { get; set; }

        /// <summary>总投注金额</summary>
        public decimal TotalStake { get; set; }

        public decimal OddsDec { get; set; }

        public decimal ValidStake { get; set; }

        public string Platform { get; set; }

        public decimal Return { get; set; }

        public decimal DomainID { get; set; }

        public string BetStatus { get; set; }

        public string Brand { get; set; }

        public string UserName { get; set; }

        public string BetTypeName { get; set; }

        public int BetTypeId { get; set; }

        public DateTime CreationDate { get; set; }

        public string Status { get; set; }

        public decimal CustomerID { get; set; }

        public string MerchantCustomerID { get; set; }

        public string Currency { get; set; }

        public decimal PlayerLevelID { get; set; }

        public List<Selection> Selections { get; set; }

        public override string KeyId => PurchaseID;

        public override string TPGameAccount => MerchantCustomerID;
    }

    public class Selection
    {
        public decimal? IsResettled { get; set; }

        public decimal? RelatedBetID { get; set; }

        public string ActionType { get; set; }

        public decimal? BonusID { get; set; }

        public string CouponCode { get; set; }

        public decimal? ReferenceID { get; set; }

        public string BetID { get; set; }

        public decimal? LineID { get; set; }

        public decimal? Odds { get; set; }

        public string OddsInUserStyle { get; set; }

        public string LeagueName { get; set; }

        public decimal? LeagueID { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string BranchName { get; set; }

        public decimal? BranchID { get; set; }

        public string LineTypeName { get; set; }

        public decimal? Podecimals { get; set; }

        public string Score { get; set; }

        public string YourBet { get; set; }

        public DateTime? EventDate { get; set; }

        public string EventTypeName { get; set; }

        public string BetType { get; set; }

        public decimal? IsFreeBet { get; set; }

        public decimal? OddsDec { get; set; }

        public decimal? LiveScore1 { get; set; }

        public decimal? LiveScore2 { get; set; }

        public string Status { get; set; }

        public decimal? GameID { get; set; }

        public decimal? EventTypeID { get; set; }

        public decimal? LineTypeID { get; set; }

        public decimal? IsLive { get; set; }

        public DateTime? UpdateDate { get; set; }

        public decimal? IsNewLine { get; set; }
    }

    public class BTISBettingHistoryStatus : BaseStringValueModel<BTISBettingHistoryStatus>
    {
        public BetResultType BetResultType { get; private set; }

        private BTISBettingHistoryStatus()
        { }

        /// <summary>
        /// 成立订单
        /// </summary>
        public static readonly BTISBettingHistoryStatus Opened = new BTISBettingHistoryStatus()
        {
            Value = "Opened"
        };

        /// <summary>
        /// 已结算且为赢
        /// </summary>
        public static readonly BTISBettingHistoryStatus Won = new BTISBettingHistoryStatus()
        {
            Value = "Won",
            BetResultType = BetResultType.Win
        };

        /// <summary>
        /// 已结算且为输
        /// </summary>
        public static readonly BTISBettingHistoryStatus Lost = new BTISBettingHistoryStatus()
        {
            Value = "Lost",
            BetResultType = BetResultType.Lose
        };

        /// <summary>
        /// 半赢
        /// </summary>
        public static readonly BTISBettingHistoryStatus HalfWon = new BTISBettingHistoryStatus()
        {
            Value = "Half Won",
            BetResultType = BetResultType.HalfWin
        };

        /// <summary>
        /// 半输
        /// </summary>
        public static readonly BTISBettingHistoryStatus HalfLost = new BTISBettingHistoryStatus()
        {
            Value = "Half Lost",
            BetResultType = BetResultType.HalfLose
        };

        /// <summary>
        /// 已取消
        /// </summary>
        public static readonly BTISBettingHistoryStatus Canceled = new BTISBettingHistoryStatus()
        {
            Value = "Canceled"
        };

        /// <summary>
        /// 兑现
        /// </summary>
        public static readonly BTISBettingHistoryStatus Cashout = new BTISBettingHistoryStatus()
        {
            Value = "Cashout",
            BetResultType = BetResultType.Cashout
        };

        /// <summary>
        /// 和局
        /// </summary>
        public static readonly BTISBettingHistoryStatus Draw = new BTISBettingHistoryStatus()
        {
            Value = "Draw",
            BetResultType = BetResultType.Draw
        };
    }

    public class BTISWagerType : BaseIntValueModel<BTISWagerType>
    {
        public WagerType WagerType { get; private set; }

        public static readonly BTISWagerType SingleBets = new BTISWagerType()
        {
            Value = 1,
            WagerType = WagerType.Single
        };

        public static readonly BTISWagerType ComboBets = new BTISWagerType()
        {
            Value = 2,
            WagerType = WagerType.Combo
        };

        public static readonly BTISWagerType SystemBet = new BTISWagerType()
        {
            Value = 3,
            WagerType = WagerType.Combo
        };

        public static readonly BTISWagerType QABet = new BTISWagerType()
        {
            Value = 5,
            WagerType = WagerType.Single
        };

        public static readonly BTISWagerType QABet2 = new BTISWagerType()
        {
            Value = 7,
            WagerType = WagerType.Single
        };

        public static readonly BTISWagerType SystemBet2 = new BTISWagerType()
        {
            Value = 13,
            WagerType = WagerType.Combo
        };
    }
}