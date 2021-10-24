using JxBackendService.Model.ThirdParty.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.IM.Lottery
{
    public class IMRegisterResponseModel : IMBaseResponseModel
    {
        public string Currency { get; set; }

    }

    public class IMGetBalanceResponseModel : IMRegisterResponseModel
    {
        public string Balance { get; set; }
    }

    public class IMLaunchGameResponseModel : IMBaseResponseModel
    {
        public string GameUrl { get; set; }
    }

    public class IMTransferResponseModel : IMBaseResponseModel
    {
        public string Status { get; set; }        
    }

    public class IMLotteryBetLogResponseModel : IMBaseResponseModel
    {
        public List<IMLotteryBetLog> Result { get; set; }
    }
}
