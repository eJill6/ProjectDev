﻿using System;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class IMVRUserInfoService : BaseTpGameUserInfoService<IMVRUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMVRUserInfoRep> _imvrUserInfoRep;

        public IMVRUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imvrUserInfoRep = ResolveJxBackendService<IIMVRUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMVRUserInfo> TPGameUserInfoRep => _imvrUserInfoRep.Value;
    }
}