using JxBackendService.Interface.Repository.Base;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;

namespace JxBackendService.Interface.Repository.Finance
{
    public interface ICMoneyOutInfoRep : IBaseCMoneyInfoRep<CMoneyOutInfo>
    {
        BaseReturnModel CreateCMoneyOutInfo(ProCreateCMoneyOutInfoParam param);

        BaseReturnModel ProcessCMoneyOut(ProProcessCMoneyOutParam param);
    }
}