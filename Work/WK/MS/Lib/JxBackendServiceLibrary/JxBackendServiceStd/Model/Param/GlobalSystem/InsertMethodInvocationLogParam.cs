using System;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.GlobalSystem
{
    public class InsertMethodInvocationLogParam : IInsertMethodInvocationLogParam
    {
        public object Arguments { get; set; }

        public string CorrelationId { get; set; }

        public decimal ElapsedMilliseconds { get; set; }

        public string ErrorMsg { get; set; }

        public string MethodName { get; set; }

        public string TypeName { get; set; }

        public object ReturnValue { get; set; }

        public int UserID { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public class ProAddMultipleMethodInvocationLogParam
    {
        public string BulkAddMethodInvocationLogJson { get; set; }

        [VarcharColumnInfo(6)]
        public string RC_Success => ReturnCode.Success.Value;

        [VarcharColumnInfo(6)]
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;

        [VarcharColumnInfo(6)]
        public string RC_SystemError => ReturnCode.SystemError.Value;
    }
}