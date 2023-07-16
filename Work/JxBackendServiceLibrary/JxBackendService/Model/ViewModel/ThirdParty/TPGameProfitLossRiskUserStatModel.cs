using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class BaseTPGameProfitLossRiskStatModel
    {
        public decimal TotalAllBetMoney { get; set; }

        public decimal TotalWinMoney { get; set; }

        public decimal TotalPrizeMoney { get; set; }

        public int TotalWinBetCount { get; set; }

        public int TotalBetCount { get; set; }

        public decimal WinRate
        {
            get
            {
                if (TotalAllBetMoney == 0)
                {
                    return 0;
                }

                return Math.Round((TotalWinMoney / (decimal)TotalAllBetMoney), 2) * 100m;
            }
        }

        public decimal PrizeRate
        {
            get
            {
                if (TotalBetCount == 0)
                {
                    return 0;
                }

                return Math.Round((TotalWinBetCount / (decimal)TotalBetCount), 2) * 100m;
            }
        }
    }

    public class TPGameProfitLossRiskUserStatModel : BaseTPGameProfitLossRiskStatModel
    {
        public string UserName { get; set; }
    }

    public class TPGameProfitLossRiskProductStatModel : BaseTPGameProfitLossRiskStatModel
    {
        public string ProductCode { get; set; }

        public string ProductName => PlatformProduct.GetName(ProductCode);
    }
}
