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
    public class FYESUserInfoService : BaseTpGameUserInfoService<FYESUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IFYESUserInfoRep> _fyesUserInfoRep;

        public FYESUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _fyesUserInfoRep = ResolveJxBackendService<IFYESUserInfoRep>();
        }

        public override ITPGameUserInfoRep<FYESUserInfo> TPGameUserInfoRep => _fyesUserInfoRep.Value;
    }
}