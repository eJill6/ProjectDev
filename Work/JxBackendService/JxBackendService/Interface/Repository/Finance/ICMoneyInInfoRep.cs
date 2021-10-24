using System;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Param.Finance;

namespace JxBackendService.Interface.Repository.Finance
{
    public interface ICMoneyInInfoRep : IBaseDbRepository<CMoneyInInfo>
    {
        DespositInfo GetDepositDoneOrderInfo(int userId, DateTime startDate, DateTime endDate);
    }
}
