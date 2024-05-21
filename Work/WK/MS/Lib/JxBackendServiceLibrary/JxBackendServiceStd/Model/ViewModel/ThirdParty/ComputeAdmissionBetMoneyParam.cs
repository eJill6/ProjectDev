using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class ComputeAdmissionBetMoneyParam
    {
        /// <summary>總投注額</summary>
        public decimal AllBetMoney { get; set; }

        /// <summary>有效投注額(有的第三方會提供)</summary>
        public decimal? BetMoney { get; set; }

        /// <summary>盈虧金額</summary>
        public decimal ProfitLossMoney { get; set; }

        public BetResultType BetResultType { get; set; }

        public WagerType WagerType { get; set; }

        public List<HandicapInfo> HandicapInfos { get; set; }

        public decimal GetDefaultAdmissionBetMoney()
        {
            if (BetMoney.HasValue)
            {
                return BetMoney.Value;
            }

            return AllBetMoney;
        }
    }

    public class HandicapInfo
    {
        /// <summary>盤口代碼</summary>
        public string Handicap { get; set; }


        /// <summary>賠率</summary>
        public decimal? Odds { get; set; }
    }
}
