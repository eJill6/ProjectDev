using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class OBSPUserInfoService : BaseTpGameUserInfoService<OBSPUserInfo>, ITPGameUserInfoService
    {
        private readonly IOBSPUserInfoRep _obspUserInfoRep;

        public OBSPUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _obspUserInfoRep = ResolveJxBackendService<IOBSPUserInfoRep>();
        }

        public override ITPGameUserInfoRep<OBSPUserInfo> TPGameUserInfoRep => _obspUserInfoRep;
    }
}
