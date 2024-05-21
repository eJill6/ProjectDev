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
    public class WLBGUserInfoService : BaseTpGameUserInfoService<WLBGUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IWLBGUserInfoRep> _wlbgUserInfoRep;

        public WLBGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _wlbgUserInfoRep = ResolveJxBackendService<IWLBGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<WLBGUserInfo> TPGameUserInfoRep => _wlbgUserInfoRep.Value;
    }
}