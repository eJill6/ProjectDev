﻿using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Base
{
    public abstract class BaseTpGameUserInfoService<T> : BaseService, ITPGameUserInfoService where T : BaseTPGameUserInfo
    {
        public BaseTpGameUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public abstract ITPGameUserInfoRep<T> TPGameUserInfoRep { get; }

        public bool IsUserExists(int userId)
        {
            return TPGameUserInfoRep.IsUserExists(userId);
        }

        public bool CreateUser(int userId)
        {
            return TPGameUserInfoRep.CreateUser(userId);
        }

        public bool UpdateUserScores(int userId, UserScore userScore)
        {
            return TPGameUserInfoRep.UpdateUserScores(userId, userScore);
        }

        public BaseTPGameUserInfo GetTPGameUserInfo(int userId)
        {
            return TPGameUserInfoRep.GetDetail(userId);
        }

        public List<BaseTPGameUserInfo> GetUsersTransferedIn()
        {
            return TPGameUserInfoRep.GetUsersTransferedIn().ToList<BaseTPGameUserInfo>();
        }

        public string GetQuerySingleSQL() => TPGameUserInfoRep.GetQuerySingleSQL();

        public Type GetUserInfoType() => TPGameUserInfoRep.GetUserInfoType();

        public virtual List<string> AllAvailableScoresColumnNames => new List<string>() { "AvailableScores" };
    }
}