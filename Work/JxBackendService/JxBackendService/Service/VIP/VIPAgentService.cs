using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.VIP
{
    public class VIPAgentService : BaseService, IVIPAgentService
    {
        private readonly IVIPAgentScoreRep _vipAgentScoreRep;

        public VIPAgentService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
            _vipAgentScoreRep = ResolveJxBackendService<IVIPAgentScoreRep>();
        }

        public List<VIPAgentScore> GetVIPAgentScores(List<int> userIds)
        {
            return _vipAgentScoreRep.GetVIPAgentScores(userIds);
        }
    }
}