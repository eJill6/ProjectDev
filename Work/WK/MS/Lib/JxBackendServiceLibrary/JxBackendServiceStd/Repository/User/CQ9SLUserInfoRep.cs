using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.User
{
    public class CQ9SLUserInfoRep : BaseTPGameUserInfoRep<CQ9SLUserInfo>, ICQ9SLUserInfoRep
    {
        public CQ9SLUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}