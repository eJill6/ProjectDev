using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.OB.OBSP
{
    public class SaveBetLogToPlatformParam
    {
        public List<InsertTPGameProfitlossParam> InsertTPGameProfitlossParams { get; set; }

        public List<OBSPBetLog> OBSPBetLogs { get; set; }
    }

    public class OBSPQueryBetListData
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<OBSPBetLog> List { get; set; }

        public int PageNum { get; set; }
    }

    public class OBSPBetLog : BaseRemoteBetLog
    {
        public override string KeyId => OrderNo;

        public override string TPGameAccount => UserName;

        public string UserName { get; set; }

        public int BetCount { get; set; }

        public int SeriesType { get; set; }

        public string SeriesValue { get; set; }

        public string OrderNo { get; set; }

        public int OrderStatus { get; set; }

        public long CreateTime { get; set; }

        public decimal OrderAmount { get; set; }

        public List<OBSPBetLogDetail> DetailList { get; set; }

        public int? Outcome { get; set; }

        public decimal? SettleAmount { get; set; }

        public decimal? ProfitAmount { get; set; }

        public decimal? PreBetAmount { get; set; }

        public long? SettleTime { get; set; }
    }

    public class OBSPBetLogDetail
    {
        public string BetNo { get; set; }

        public string PlayOptionsId { get; set; }

        public long MatchId { get; set; }

        public decimal BetAmount { get; set; }

        public string MatchName { get; set; }

        public string MatchInfo { get; set; }

        public int MatchType { get; set; }

        public string MarketType { get; set; }

        public int SportId { get; set; }

        public int PlayId { get; set; }

        public string SportName { get; set; }

        public string PlayOptionName { get; set; }

        public string PlayName { get; set; }

        public string MarketValue { get; set; }

        public string OddsValue { get; set; }

        public string BetResult { get; set; }
    }
}