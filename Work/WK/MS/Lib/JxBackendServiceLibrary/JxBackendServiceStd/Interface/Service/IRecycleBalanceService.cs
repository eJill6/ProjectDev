using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service
{
    public interface IRecycleBalanceService
    {
        BaseReturnModel RecycleByAllProducts(IInvocationUserParam invocationUserParam, RoutingSetting routingSetting);

        BaseReturnModel RecycleBySingleProduct(IInvocationUserParam invocationUserParam, PlatformProduct product, RoutingSetting routingSetting);
    }
}