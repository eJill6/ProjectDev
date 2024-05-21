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
    public class IMeBETUserInfoService : BaseTpGameUserInfoService<IMeBETUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMeBETUserInfoRep> _imeBETUserInfoRep;

        public IMeBETUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imeBETUserInfoRep = ResolveJxBackendService<IIMeBETUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMeBETUserInfo> TPGameUserInfoRep => _imeBETUserInfoRep.Value;
    }
}