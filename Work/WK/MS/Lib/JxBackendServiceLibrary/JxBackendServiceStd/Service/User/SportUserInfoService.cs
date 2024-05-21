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
    public class SportUserInfoService : BaseTpGameUserInfoService<SportUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<ISportUserInfoRep> _sportUserInfoRep;

        public SportUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _sportUserInfoRep = ResolveJxBackendService<ISportUserInfoRep>();
        }

        public override ITPGameUserInfoRep<SportUserInfo> TPGameUserInfoRep => _sportUserInfoRep.Value;
    }
}