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
    public class ABEBUserInfoService : BaseTpGameUserInfoService<ABEBUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IABEBUserInfoRep> _abebUserInfoRep;

        public ABEBUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _abebUserInfoRep = ResolveJxBackendService<IABEBUserInfoRep>();
        }

        public override ITPGameUserInfoRep<ABEBUserInfo> TPGameUserInfoRep => _abebUserInfoRep.Value;
    }
}