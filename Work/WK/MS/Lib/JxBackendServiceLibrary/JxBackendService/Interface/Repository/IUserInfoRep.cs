using JxBackendService.Model.Entity;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IUserInfoRep : IBaseDbRepository<UserInfo>
    {
        List<BaseUserInfoEntityModel> GetBaseBasicUserInfos(List<int> userIds);

        List<UserInfo> GetUserInfos(List<int> userIdsByRule);

        List<UserInfo> GetIdleScoreUsers(DateTime minScoreChangeDate, decimal minTransferToMiseAmount);
    }
}