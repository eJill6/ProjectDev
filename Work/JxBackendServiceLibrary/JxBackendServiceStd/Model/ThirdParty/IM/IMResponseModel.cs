using JxBackendService.Model.ThirdParty.Base;
using System.Collections.Generic;

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

    public class IMKYBetLogResponseModel : IMBaseResponseModel
    {
        public List<IMKYBetLog> Result { get; set; }
    }

    public class IMPTJackpotResponseModel : IMBaseResponseModel
    {
        public IMPTJackpotResult Result { get; set; }
    }

    public class IMPTJackpotResult
    {
        public string Name { get; set; }

        public string WinCount { get; set; }

        public string Timestamp { get; set; }

        public string Currency { get; set; }

        public string Wins { get; set; }

        public string Step { get; set; }

        public string Sign { get; set; }

        public string Position { get; set; }

        public string Amount { get; set; }
    }

    public class IMPPJackpotResponseModel : IMBaseResponseModel
    {
        public IMPPJackpotResult Result { get; set; }
    }

    public class IMPPJackpotResult
    {
        public string Timestamp { get; set; }

        public string Currency { get; set; }

        public List<IMSlotGame> Games { get; set; }
    }

    public class IMSlotGame
    {
        public string Provider { get; set; }

        public string IMGameCode { get; set; }

        public string GameName { get; set; }

        public string JackpotName { get; set; }

        public string Amount { get; set; }
    }
}