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
    public class IMVRUserInfoService : BaseTpGameUserInfoService<IMVRUserInfo>, ITPGameUserInfoService
    {
        private readonly IIMVRUserInfoRep _imvrUserInfoRep;

        public IMVRUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imvrUserInfoRep = ResolveJxBackendService<IIMVRUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMVRUserInfo> TPGameUserInfoRep => _imvrUserInfoRep;
    }
}
