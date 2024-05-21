using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.BackSideUser
{
    public class UserAuthenticatorRep : BaseDbRepository<BWUserAuthenticator>, IBWUserAuthenticatorRep
    {
        public UserAuthenticatorRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
    }
}
