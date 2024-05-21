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
    public class EVEBUserInfoService : BaseTpGameUserInfoService<EVEBUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IEVEBUserInfoRep> _evebUserInfoRep;

        public EVEBUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _evebUserInfoRep = ResolveJxBackendService<IEVEBUserInfoRep>();
        }

        public override ITPGameUserInfoRep<EVEBUserInfo> TPGameUserInfoRep => _evebUserInfoRep.Value;
    }
}