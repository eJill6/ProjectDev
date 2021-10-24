using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class BTISUserInfoService : BaseTpGameUserInfoService<BTISUserInfo>, ITPGameUserInfoService
    {
        private readonly IBTISUserInfoRep _btisUserInfoRep;

        public BTISUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _btisUserInfoRep = ResolveJxBackendService<IBTISUserInfoRep>();
        }

        public override ITPGameUserInfoRep<BTISUserInfo> TPGameUserInfoRep => _btisUserInfoRep;
    }
}
