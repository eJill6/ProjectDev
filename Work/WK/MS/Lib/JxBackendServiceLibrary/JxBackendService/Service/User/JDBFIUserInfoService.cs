using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class JDBFIUserInfoService : BaseTpGameUserInfoService<JDBFIUserInfo>, ITPGameUserInfoService
    {
        private readonly IJDBFIUserInfoRep _jdbfiUserInfoRep;

        public JDBFIUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jdbfiUserInfoRep = ResolveJxBackendService<IJDBFIUserInfoRep>();
        }

        public override ITPGameUserInfoRep<JDBFIUserInfo> TPGameUserInfoRep => _jdbfiUserInfoRep;
    }
}