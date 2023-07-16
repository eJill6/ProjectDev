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
    public class PGSLUserInfoService : BaseTpGameUserInfoService<PGSLUserInfo>, ITPGameUserInfoService
    {
        private readonly IPGSLUserInfoRep _pgslUserInfoRep;

        public PGSLUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _pgslUserInfoRep = ResolveJxBackendService<IPGSLUserInfoRep>();
        }

        public override ITPGameUserInfoRep<PGSLUserInfo> TPGameUserInfoRep => _pgslUserInfoRep;
    }
}
