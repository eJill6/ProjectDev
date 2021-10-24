using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.User;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class AGUserInfoService : BaseTpGameUserInfoService<AGUserInfo>, ITPGameUserInfoService
    {
        private readonly IAGUserInfoRep _agUserInfoRep;

        public AGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _agUserInfoRep = ResolveJxBackendService<IAGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<AGUserInfo> TPGameUserInfoRep => _agUserInfoRep;
    }
}
