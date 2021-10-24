using System;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.VIP
{
    public class VIPAgentParam
    {
        public int UserID { get; set; }
        public string RC_Success => ReturnCode.Success.Value;
        public string RC_OperationFailed => ReturnCode.OperationFailed.Value;
    }
}