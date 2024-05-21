using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTPGameUserInfoRep<T> : BaseDbRepository<T>, ITPGameUserInfoRep<T> where T : BaseTPGameUserInfo, new()
    {
        public BaseTPGameUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public T GetDetail(int userId)
        {
            T tpGameUserInfo = new T
            {
                UserID = userId
            };

            return GetSingleByKey(InlodbType.Inlodb, tpGameUserInfo);
        }

        public List<T> GetUsersTransferedIn()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE TransferIn > 0 ";

            return DbHelper.QueryList<T>(sql, param: null);
        }

        public bool IsUserExists(int userId)
        {
            string sql = $"SELECT TOP 1 1 {GetFromTableSQL(InlodbType.Inlodb)} WHERE userId = @userId ";

            return DbHelper.QuerySingleOrDefault<int?>(sql, new { userId }).HasValue;
        }

        public string GetQuerySingleSQL() => DapperSqlMapperExtensions.GetByKeySQL(InlodbType.Inlodb, GetUserInfoType());

        public Type GetUserInfoType() => typeof(T);

        public bool CreateUser(int userId)
        {
            string insertSQL = ReflectUtil.GenerateInsertSQL<T>(InlodbType.Inlodb.Value, "dbo", ModelUtil.GetTableName(typeof(T)));

            T param = new T
            {
                UserID = userId
            };

            return DbHelper.Execute(insertSQL, param) > 0;
        }

        public bool UpdateUserScores(int userId, UserScore userScore)
        {
            //配合CreateUser先用AD HOC, 未來有需要調整連同INSERT也一起調整, 需要調整第三方範本工具
            string updateSQL = $@"
                UPDATE {InlodbType.Inlodb.Value}.dbo.{ModelUtil.GetTableName(typeof(T))}
                SET
                    AvailableScores = CASE WHEN @AvailableScores IS NULL THEN AvailableScores ELSE @AvailableScores END,
		            FreezeScores = CASE WHEN @FreezeScores IS NULL THEN FreezeScores ELSE @FreezeScores END,
                    LastUpdateTime = GETDATE()
                WHERE UserID = @UserID ";

            T param = new T
            {
                UserID = userId,
                AvailableScores = userScore.AvailableScores,
                FreezeScores = userScore.FreezeScores
            };

            return DbHelper.Execute(updateSQL, param) > 0;
        }
    }
}