using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.EVO
{
    public class EVTokenModel
    {
        public string MerchantCode => EVEBSharedAppSettings.Instance.MerchantCode;
        public string MerchantKey => EVEBSharedAppSettings.Instance.SecretKey;
    }

    public class EVBaseAccountRequestModel
    {
        public string MemberAccount { get; set; }
    }

    public class EVBaseAccountWithCodeRequestModel : EVBaseAccountRequestModel
    {
        public int ProductCode { get; set; }
    }

    public class EVRegisterRequestModel : EVBaseAccountRequestModel
    {
    }

    public class EVTransferRequestModel : EVCheckTransferRequestModel
    {
        public decimal Amount { get; set; }
        public int Type { get; set; }
    }

    public class EVCheckTransferRequestModel : EVBaseAccountWithCodeRequestModel
    {
        public string TransNo { get; set; }
    }

    public class EVLunchGameRequestModel : EVBaseAccountWithCodeRequestModel
    {
        public bool IsMobile { get; set; }
        public string GameId { get; set; }
        public string Ip { get; set; }
    }

    public class EVBetLogRequestModel
    {
        public int ProductCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TimeType { get; set; }
        public int Page { get; set; }
    }

}
