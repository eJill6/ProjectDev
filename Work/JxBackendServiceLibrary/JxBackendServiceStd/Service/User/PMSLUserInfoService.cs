using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.User;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class PMSLUserInfoService : BaseTpGameUserInfoService<PMSLUserInfo>, ITPGameUserInfoService
    {
        private readonly IPMSLUserInfoRep _pmslUserInfoRep;

        public PMSLUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _pmslUserInfoRep = ResolveJxBackendService<IPMSLUserInfoRep>();
        }

        public override ITPGameUserInfoRep<PMSLUserInfo> TPGameUserInfoRep => _pmslUserInfoRep;
    }
}