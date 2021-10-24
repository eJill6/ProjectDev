using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class TPGameProfitLossDetail
    {
        public decimal ProfitLoss { get; set; }
        public decimal Bet { get; set; }
        public decimal Prize { get; set; }
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }
        public decimal RebateMoney { get; set; }
        public decimal? Commission { get; set; } = 0;
        public decimal? RedPocket { get; set; } = 0;
    }

    public class TPGameChildrenProfitLossDetail : TPGameProfitLossDetail
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class TPGameSelfProfitLossSearchResult : TPGameProfitLossDetail
    {
        public int RegCount { get; set; }
        public int TZCount { get; set; }
    }

    public class TPGameTeamProfitLossSearchResult
    {
        public TPGameProfitLossDetail Total { get; set; }
        public List<TPGameChildrenProfitLossDetail> List { get; set; }
        public TPGameChildrenProfitLossDetail Target { get; set; }
    }
}
