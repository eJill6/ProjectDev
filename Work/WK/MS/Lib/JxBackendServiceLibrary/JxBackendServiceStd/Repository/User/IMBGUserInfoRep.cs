using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.User
{
    public class IMBGUserInfoRep : BaseTPGameUserInfoRep<IMBGUserInfo>, IIMBGUserInfoRep
    {
        public IMBGUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
    }
}
