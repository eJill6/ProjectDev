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
    public class PtUserInfoService : BaseTpGameUserInfoService<PtUserInfo>, ITPGameUserInfoService
    {
        private readonly IPtUserInfoRep _ptUserInfoRep;

        public PtUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _ptUserInfoRep = ResolveJxBackendService<IPtUserInfoRep>();
        }

        public override ITPGameUserInfoRep<PtUserInfo> TPGameUserInfoRep => _ptUserInfoRep;
    }
}
