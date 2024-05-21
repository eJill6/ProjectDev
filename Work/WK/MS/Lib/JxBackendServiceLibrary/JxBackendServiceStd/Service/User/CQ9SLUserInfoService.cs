using System;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class CQ9SLUserInfoService : BaseTpGameUserInfoService<CQ9SLUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<ICQ9SLUserInfoRep> _cq9slUserInfoRep;

        public CQ9SLUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _cq9slUserInfoRep = ResolveJxBackendService<ICQ9SLUserInfoRep>();
        }

        public override ITPGameUserInfoRep<CQ9SLUserInfo> TPGameUserInfoRep => _cq9slUserInfoRep.Value;
    }
}