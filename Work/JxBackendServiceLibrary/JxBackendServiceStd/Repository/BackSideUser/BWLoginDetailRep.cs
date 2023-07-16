using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.BackSideUser
{
    public class BWLoginDetailRep : BaseDbRepository<BWLoginDetail>, IBWLoginDetailRep
    {
        public BWLoginDetailRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
    }
}
