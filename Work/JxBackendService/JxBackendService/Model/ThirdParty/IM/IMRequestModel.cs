using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.IM.Lottery
{
    public class IMGetBalanceRequestModel : IMBaseRequestModel
    {
        public string PlayerId { get; set; }
    }

    public class IMRegisterRequestModel : IMGetBalanceRequestModel
    {
        public string Currency { get; set; }
        public string Password { get; set; }
    }

    public class IMCheckExistRequestModel : IMGetBalanceRequestModel
    {
    }

    public class IMCheckTransferRequestModel : IMGetBalanceRequestModel
    {
        public string TransactionId { get; set; }
    }

    public class IMTransferRequestModel : IMCheckTransferRequestModel
    {
        public decimal Amount { get; set; }
    }

    public class IMGetBetLogRequestModel : IMBaseRequestModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 50000;
        public string Currency { get; set; }
    }

    public class IMLaunchGameRequestModel : IMGetBalanceRequestModel
    {
        public string GameCode { get; set; }
        public string Language { get; set; }
        public string IpAddress { get; set; }
        public int Http { get; set; } = 1;
        public int IsDownload { get; set; } = 0;
    }

}
