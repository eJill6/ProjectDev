using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class OBEBUserInfoService : BaseTpGameUserInfoService<OBEBUserInfo>, ITPGameUserInfoService
    {
        private readonly IOBEBUserInfoRep _obebUserInfoRep;

        public OBEBUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _obebUserInfoRep = ResolveJxBackendService<IOBEBUserInfoRep>();
        }

        public override ITPGameUserInfoRep<OBEBUserInfo> TPGameUserInfoRep => _obebUserInfoRep;
    }
}
