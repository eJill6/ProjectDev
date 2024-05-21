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
    public class AWCSPUserInfoService : BaseTpGameUserInfoService<AWCSPUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IAWCSPUserInfoRep> _awcspUserInfoRep;

        public AWCSPUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _awcspUserInfoRep = ResolveJxBackendService<IAWCSPUserInfoRep>();
        }

        public override ITPGameUserInfoRep<AWCSPUserInfo> TPGameUserInfoRep => _awcspUserInfoRep.Value;
    }
}