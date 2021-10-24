using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.VIP
{
    public interface IVIPAgentService
    {
        List<VIPAgentScore> GetVIPAgentScores(List<int> userIds);
    }
}