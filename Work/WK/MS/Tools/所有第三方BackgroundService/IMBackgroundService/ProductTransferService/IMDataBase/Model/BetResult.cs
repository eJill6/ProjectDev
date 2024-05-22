using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMDataBase.Model
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
        public string Provider { get; set; }

        public string GameId { get; set; }

        public string BetId { get; set; }

        public string WagerCreationDateTime { get; set; }

        public string LastUpdatedDate { get; set; }

        public string PlayerId { get; set; }

        public string ProviderPlayerId { get; set; }

        public string Currency { get; set; }

        public string StakeAmount { get; set; }

        public string WinLoss { get; set; }

        public string OddsType { get; set; }

        public string WagerType { get; set; }

        public string Platform { get; set; }

        public int IsSettled { get; set; }

        public int IsCancelled { get; set; }

        public string SettlementDateTime { get; set; }

        public List<DetailItem> DetailItems { get; set; }
    }

    public class DetailItem
    {
        public string EventName { get; set; }

        public string EventDateTime { get; set; }

        public string BetDescription { get; set; }

        public string CompetitionName { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public string FavTeam { get; set; }

        public string BetType { get; set; }

        public string Selection { get; set; }

        public string Odds { get; set; }

        public string HomeTeamHTScore { get; set; }

        public string AwayTeamHTScore { get; set; }

        public string HomeTeamFTScore { get; set; }

        public string AwayTeamFTScore { get; set; }

        public string WagerHomeTeamScore { get; set; }

        public string WagerAwayTeamScore { get; set; }

        public string Handicap { get; set; }

        public string SportsName { get; set; }

        public string EventId { get; set; }

        public string EventType { get; set; }

        public string GameOrder { get; set; }

        public string ParlaySign { get; set; }

        public string IsWagerItemCancelled { get; set; }

        public string ParlayWagerCreationDateTime { get; set; }
    }
}