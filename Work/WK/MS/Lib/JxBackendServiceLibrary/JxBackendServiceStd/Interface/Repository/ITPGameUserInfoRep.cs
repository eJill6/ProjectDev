using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface ITPGameUserInfoRep<T>
    {
        bool CreateUser(int userId);

        bool UpdateUserScores(int userId, UserScore userScore);

        T GetDetail(int userId);

        List<T> GetUsersTransferedIn();

        bool IsUserExists(int userId);

        string GetQuerySingleSQL();

        Type GetUserInfoType();
    }
}