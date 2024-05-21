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
    public class PMBGUserInfoService : BaseTpGameUserInfoService<PMBGUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IPMBGUserInfoRep> _pmbgUserInfoRep;

        public PMBGUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _pmbgUserInfoRep = ResolveJxBackendService<IPMBGUserInfoRep>();
        }

        public override ITPGameUserInfoRep<PMBGUserInfo> TPGameUserInfoRep => _pmbgUserInfoRep.Value;
    }
}