using System.Collections.Generic;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface IMethodInvocationLogReadService
    {
        void Enqueue(IInsertMethodInvocationLogParam insertMethodInvocationLogParam);
    }

    public interface IMethodInvocationLogService
    {
        BaseReturnModel BatchInsertLogs(List<IInsertMethodInvocationLogParam> logParams);
    }
}