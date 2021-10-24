using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using System;
using System.Collections.Specialized;

namespace JxBackendService.Model.ThirdParty.OB.OBSP
{
    public class OBSPApiResponse
    {
        public bool Status { get; set; }

        public string Msg { get; set; }

        public string Code { get; set; }

        public long ServerTime { get; set; }

        public bool IsSuccess => Status && Code == OBSPReturnCode.Success;
    }

    public class OBSPApiResponse<T> : OBSPApiResponse
    {
        public T Data { get; set; }
    }

    public class CheckBalanceData
    {
        public decimal Balance { get; set; }
        public string UserName { get; set; }
    }

    public class GetTransferRecordData
    {
        public string TransferId { get; set; }
        public string MerchantCode { get; set; }
        public long UserId { get; set; }
        public int TransferType { get; set; }
        public decimal Amount { get; set; }
        public decimal BeforeTransfer { get; set; }
        public decimal AfterTransfer { get; set; }
        public int Status { get; set; }
        public string Mag { get; set; }
        public int TransferMode { get; set; }
        public long CreateTime { get; set; }
    }

    public class CreateUserData
    {
        public long UserId { get; set; }
        public long CreateTime { get; set; }
    }

    public class UserLoginData
    {
        public string Domain { get; set; }
        public string Token { get; set; }
    }    
}
