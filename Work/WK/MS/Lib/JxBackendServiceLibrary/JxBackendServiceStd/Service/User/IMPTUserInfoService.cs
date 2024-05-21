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
    public class IMPTUserInfoService : BaseTpGameUserInfoService<IMPTUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMPTUserInfoRep> _imPTUserInfoRep;

        public IMPTUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imPTUserInfoRep = ResolveJxBackendService<IIMPTUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMPTUserInfo> TPGameUserInfoRep => _imPTUserInfoRep.Value;
    }
}