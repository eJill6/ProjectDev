using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface IMethodInvocationLogService
    {
        BaseReturnDataModel<long> Create(IInsertMethodInvocationLogParam param);
    }
}