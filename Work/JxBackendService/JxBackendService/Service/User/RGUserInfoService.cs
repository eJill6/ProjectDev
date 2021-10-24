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
    public class RGUserInfoService : BaseTpGameUserInfoService<RGUserInfo>, ITPGameUserInfoService
    {
        private readonly IRGUserInfoRep _rgUserInfoRep;

        public RGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _rgUserInfoRep = ResolveJxBackendService<IRGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<RGUserInfo> TPGameUserInfoRep => _rgUserInfoRep;
    }
}
