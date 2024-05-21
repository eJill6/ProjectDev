using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameUserInfoService
    {
        bool IsUserExists(int userId);

        bool CreateUser(int userId);

        BaseTPGameUserInfo GetTPGameUserInfo(int userId);

        string GetQuerySingleSQL();

        Type GetUserInfoType();

        List<string> AllAvailableScoresColumnNames { get; }
    }
}