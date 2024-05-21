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
    public class IMKYUserInfoService : BaseTpGameUserInfoService<IMKYUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMKYUserInfoRep> _imkyUserInfoRep;

        public IMKYUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imkyUserInfoRep = ResolveJxBackendService<IIMKYUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMKYUserInfo> TPGameUserInfoRep => _imkyUserInfoRep.Value;
    }
}