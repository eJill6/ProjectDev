using System;

namespace JxBackendService.Interface.Model.GlobalSystem
{
    public interface IInsertMethodInvocationLogParam
    {
        object Arguments { get; set; }

        string CorrelationId { get; set; }

        decimal ElapsedMilliseconds { get; set; }

        string ErrorMsg { get; set; }

        string MethodName { get; set; }

        string TypeName { get; set; }

        object ReturnValue { get; set; }

        int UserID { get; set; }

        DateTime CreateDate { get; set; }
    }
}