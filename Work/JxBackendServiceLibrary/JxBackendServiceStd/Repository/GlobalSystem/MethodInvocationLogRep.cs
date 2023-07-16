using JxBackendService.Interface.Repository.GlobalSystem;
using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.DBLog
{
    public class MethodInvocationLogRep : BaseDbRepository<MethodInvocationLog>, IMethodInvocationLogRep
    {
        public MethodInvocationLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public string CreateSEQID() => GetSequenceIdentity("SEQ_MethodInvocationLog_SEQID");
    }
}