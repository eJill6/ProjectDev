using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Repository.GlobalSystem
{
    public interface IMethodInvocationLogRep : IBaseDbRepository<MethodInvocationLog>
    {
        BaseReturnModel AddMultipleMethodInvocationLog(ProAddMultipleMethodInvocationLogParam param);
    }
}