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
    public class IMPPUserInfoService : BaseTpGameUserInfoService<IMPPUserInfo>, ITPGameUserInfoService
    {
        private readonly Lazy<IIMPPUserInfoRep> _imPPUserInfoRep;

        public IMPPUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _imPPUserInfoRep = ResolveJxBackendService<IIMPPUserInfoRep>();
        }

        public override ITPGameUserInfoRep<IMPPUserInfo> TPGameUserInfoRep => _imPPUserInfoRep.Value;
    }
}