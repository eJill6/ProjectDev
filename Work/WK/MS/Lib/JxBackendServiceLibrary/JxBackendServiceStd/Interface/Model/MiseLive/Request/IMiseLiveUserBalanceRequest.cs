using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveUserBalanceRequest : IMiseLiveSaltRequest, IMiseLiveUserColumn
    {
    }

    public interface IMiseLiveUserBalanceRequestParam : IMiseLiveRequestParam, IMiseLiveUserColumn, IInvocationUserParam
    {
    }
}