using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.Finance
{
    public interface IWithdrawService
    {
        void RecheckWithdrawOrdersFromMiseLive();

        BaseReturnModel WithdrawToMiseLive(decimal amount);
    }
}