using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABRegisterResponseModel : ABBaseResponseModel
    {
        public string client { get; set; }
    }

    public class ABLunchGameResponseModel : ABBaseResponseModel
    {
        public string gameLoginUrl { get; set; }
    }

    public class ABTransferResponseModel : ABBaseResponseModel { }

    public class ABQueryOrderResponseModel : ABBaseResponseModel
    {
        public decimal agentCreditAfter { get; set; }
        public decimal agentCreditBefore { get; set; }
        public decimal clientCreditAfter { get; set; }
        public decimal clientCreditBefore { get; set; }
        public int transferState { get; set; }
    }

    public class ABBalanceResponseModel : ABBaseResponseModel
    {
        public decimal balance { get; set; }
        public string currency { get; set; }
    }

    public class ABBetLogResponseModel : ABBaseResponseModel
    {
        public string startTime { get; set; }
        public string endTime { get; set; }

        public List<ABEBBetLog> histories { get; set; }
    }

}
