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
    public class IMSGUserInfoService : BaseTpGameUserInfoService<IMSGUserInfo>, ITPGameUserInfoService
    {
        private readonly IIMSGUserInfoRep _imsgUserInfoRep;

        public IMSGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imsgUserInfoRep = ResolveJxBackendService<IIMSGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMSGUserInfo> TPGameUserInfoRep => _imsgUserInfoRep;
    }
}
