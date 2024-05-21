using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Finance
{
    public class ProfitLossRep : BaseDbRepository<ProfitLoss>, IProfitLossRep
    {
        public ProfitLossRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public string CreateProfitLossID() => GetSequenceIdentity("SEQ_ProfitLoss_ProfitLossID");
    }
}