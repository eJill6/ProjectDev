using System.Collections.Generic;
using System.Data;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.VIP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.VIP
{
    public class VIPAgentScoreRep : BaseDbRepository<VIPAgentScore>, IVIPAgentScoreRep
    {
        public VIPAgentScoreRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
        }

        public List<VIPAgentScore> GetVIPAgentScores(List<int> userIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string>() 
                                        { 
                                            nameof(VIPAgentScore.UserID), 
                                            nameof(VIPAgentScore.AvailableScores), 
                                            nameof(VIPAgentScore.FreezeScores) 
                                        }) + "WHERE UserID IN @userIds ";

            return DbHelper.QueryList<VIPAgentScore>(sql, new { userIds });
        }
    }
}