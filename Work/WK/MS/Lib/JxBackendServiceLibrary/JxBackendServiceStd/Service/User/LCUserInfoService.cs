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
    public class LCUserInfoService : BaseTpGameUserInfoService<LCUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<ILCUserInfoRep> _lcUserInfoRep;

        public LCUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _lcUserInfoRep = ResolveJxBackendService<ILCUserInfoRep>();
        }

        public override ITPGameUserInfoRep<LCUserInfo> TPGameUserInfoRep => _lcUserInfoRep.Value;
    }
}