using JxBackendService.Interface.Repository.Base;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Finance
{
    public interface ICMoneyInInfoRep : IBaseCMoneyInfoRep<CMoneyInInfo>
    {
        BaseReturnModel ProcessCMoneyIn(ProProcessCMoneyInParam param);
    }
}