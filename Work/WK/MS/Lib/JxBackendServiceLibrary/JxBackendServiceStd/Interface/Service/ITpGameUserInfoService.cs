using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameUserInfoService
    {
        bool IsUserExists(int userId);

        bool CreateUser(int userId);

        bool UpdateUserScores(int userId, UserScore userScore);

        BaseTPGameUserInfo GetTPGameUserInfo(int userId);

        List<BaseTPGameUserInfo> GetUsersTransferedIn();

        string GetQuerySingleSQL();

        Type GetUserInfoType();

        List<string> AllAvailableScoresColumnNames { get; }
    }
}