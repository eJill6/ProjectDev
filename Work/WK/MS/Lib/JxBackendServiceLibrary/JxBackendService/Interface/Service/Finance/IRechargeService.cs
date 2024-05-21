using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;

namespace JxBackendService.Interface.Service.Finance
{
    public interface IRechargeService
    {
        BaseReturnModel RechargeAllFromMiseLive();

        void RecheckOrdersFromMiseLive();
    }
}