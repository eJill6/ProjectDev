using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.User
{
    public class UserInfoRep : BaseDbRepository<UserInfo>, IUserInfoRep
    {
        public UserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<UserInfo> GetUserInfos(List<int> userIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE UserID IN @userIds ";

            return DbHelper.QueryList<UserInfo>(sql, new { userIds });
        }

        public List<BaseUserInfoEntityModel> GetBaseBasicUserInfos(List<int> userIds)
        {
            var columnNames = new List<string>
            {
                nameof(BaseUserInfoEntityModel.UserID),
                nameof(BaseUserInfoEntityModel.UserName),
            };

            return GetUsersByBatchQuery<BaseUserInfoEntityModel>(columnNames, userIds);
        }

        public List<UserInfo> GetIdleScoreUsers(DateTime minScoreChangeDate, decimal minTransferToMiseAmount)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) +
                "WHERE AvailableScores >= @minTransferToMiseAmount AND ScoreChangeDate < @minScoreChangeDate ";

            return DbHelper.QueryList<UserInfo>(sql, new { minTransferToMiseAmount, minScoreChangeDate });
        }

        private List<T> GetUsersByBatchQuery<T>(List<string> columnNames, List<int> userIds)
        {
            string querySql = GetAllQuerySQL(InlodbType.Inlodb, columnNames) + "WHERE UserID IN @userIds ";

            return GetBaseUsersByBatchQuery<T>(querySql, userIds);
        }
    }
}