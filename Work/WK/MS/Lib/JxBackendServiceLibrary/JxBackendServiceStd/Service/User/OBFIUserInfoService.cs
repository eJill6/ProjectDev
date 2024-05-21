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
    public class OBFIUserInfoService : BaseTpGameUserInfoService<OBFIUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IOBFIUserInfoRep> _obfiUserInfoRep;

        public OBFIUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _obfiUserInfoRep = ResolveJxBackendService<IOBFIUserInfoRep>();
        }

        public override ITPGameUserInfoRep<OBFIUserInfo> TPGameUserInfoRep => _obfiUserInfoRep.Value;
    }
}