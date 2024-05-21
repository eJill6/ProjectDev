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
    public class IMBGUserInfoService : BaseTpGameUserInfoService<IMBGUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMBGUserInfoRep> _imbgUserInfoRep;

        public IMBGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imbgUserInfoRep = ResolveJxBackendService<IIMBGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMBGUserInfo> TPGameUserInfoRep => _imbgUserInfoRep.Value;
    }
}