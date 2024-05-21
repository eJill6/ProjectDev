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
    public class IMUserInfoService : BaseTpGameUserInfoService<IMUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMUserInfoRep> _imUserInfoRep;

        public IMUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imUserInfoRep = ResolveJxBackendService<IIMUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMUserInfo> TPGameUserInfoRep => _imUserInfoRep.Value;
    }
}