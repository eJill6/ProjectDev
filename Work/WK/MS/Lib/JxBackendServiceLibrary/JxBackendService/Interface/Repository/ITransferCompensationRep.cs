using System.Collections.Generic;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Param.ThirdParty;

namespace JxBackendService.Interface.Repository
{
    public interface ITransferCompensationRep : IBaseDbRepository<TransferCompensation>
    {
        List<TransferCompensation> GetUnProcessedCompensations(SearchProductCompensationParam searchParam);

        bool HasUnProcessedCompensation(SearchUserCompensationParam searchParam);

        List<TransferCompensation> GetUserUnProcessedCompensations(SearchUserCompensationParam searchParam);
    }
}