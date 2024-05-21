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
    public class IMSportUserInfoService : BaseTpGameUserInfoService<IMSportUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMSportUserInfoRep> _imSportUserInfoRep;

        public IMSportUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imSportUserInfoRep = ResolveJxBackendService<IIMSportUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMSportUserInfo> TPGameUserInfoRep => _imSportUserInfoRep.Value;
    }
}