using Castle.Core.Internal;
using JxBackendService.Model.ThirdParty.Base;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.CQ9SL
{
    public class CQ9BetLogViewModel
    {
        /// <summary>總筆數</summary>
        public int TotalSize { get; set; }

        public List<CQ9BetLog> Data { get; set; }
    }

    public class CQ9BetLog : BaseRemoteBetLog
    {
        public string GameHall { get; set; }

        /// <summary>遊戲種類</summary>
        public string GameType { get; set; }

        public string GamePlatform { get; set; }

        public string GameCode { get; set; }

        /// <summary>玩家帳號</summary>
        public string Account { get; set; }

        /// <summary>注單號</summary>
        public string Round { get; set; }

        public decimal Balance { get; set; }

        /// <summary>遊戲贏分</summary>
        public decimal Win { get; set; }

        /// <summary>下注金額</summary>
        public decimal Bet { get; set; }

        /// <summary>有效下注額 ※此欄位值用於牌桌/真人/體彩類遊戲</summary>
        public decimal ValidBet { get; set; }

        public decimal Jackpot { get; set; }

        public List<decimal> JackpotContribution { get; set; }

        public string JackpotType { get; set; }

        public string Status { get; set; }

        public DateTimeOffset EndRoundTime { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public DateTimeOffset BetTime { get; set; }

        public List<CQ9BetDetail> Detail { get; set; }

        public bool SingleRowBet { get; set; }

        public string GameRole { get; set; }

        public string BankerType { get; set; }

        public decimal Rake { get; set; }

        public decimal RoomFee { get; set; }

        public bool IsFreeGame => !TicketId.IsNullOrEmpty();

        /// <summary>免費券id</summary>
        public string TicketId { get; set; }

        /// <summary>免費券類型 1 = 免費遊戲(獲得一局 free game ) 2 = 免費 spin(獲得一次 free spin )</summary>
        public string TicketType { get; set; }

        /// <summary>免費券取得類型 1 = 活動贈送 101 = 代理贈送 111 = 寶箱贈送 112 = 商城購買</summary>
        public string GivenType { get; set; }

        /// <summary>免費券下注額</summary>
        public decimal TicketBets { get; set; }

        public string Currency { get; set; }

        /// <summary>派彩加成金額</summary>
        public decimal CardWin { get; set; }

        public override string KeyId => Round;

        public override string TPGameAccount => Account;
    }

    public class CQ9BetDetail
    {
        public int FreeGame { get; set; }

        public int Luckydraw { get; set; }

        public int Bonus { get; set; }
    }
}