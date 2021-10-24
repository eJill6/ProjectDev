using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.VIP;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.VIP
{
    public interface IVIPAgentScoreRep : IBaseDbRepository<VIPAgentScore>
    {
        List<VIPAgentScore> GetVIPAgentScores(List<int> userIds);
    }
}