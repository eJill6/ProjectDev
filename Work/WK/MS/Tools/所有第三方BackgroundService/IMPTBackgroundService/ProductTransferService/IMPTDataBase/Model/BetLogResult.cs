using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;

namespace IMPTDataBase.Model
{
    /// <summary>
    /// 第三方注單明細
    /// </summary>
    public class BetLogResult<T> : ApiResult, IOldBetLogModel
    {
        public T Result { get; set; }

        public PageInfo Pagination { get; set; }

        public string RemoteFileSeq { get; set; }

        public Action WriteRemoteContentToOtherMerchant { get; set; }
    }

    public class PageInfo
    {
        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }

        public int ItemPerPage { get; set; }

        public int TotalCount { get; set; }
    }

    public class BetResult
    {
        public string PlayerName { get; set; }

        public string ProviderPlayerId { get; set; }

        public string WindowCode { get; set; }

        public string GameId { get; set; }

        public string GameCode { get; set; }

        public string GameType { get; set; }

        public string GameName { get; set; }

        public string SessionId { get; set; }

        public decimal Bet { get; set; }

        public decimal Win { get; set; }

        public decimal ProgressiveBet { get; set; }

        public decimal ProgressiveWin { get; set; }

        public decimal Balance { get; set; }

        public decimal CurrentBet { get; set; }

        public string GameDate { get; set; }

        public string LiveNetwork { get; set; }

        public string ExitGame { get; set; }

        public string BonusType { get; set; }

        public string RNum { get; set; }
    }
}