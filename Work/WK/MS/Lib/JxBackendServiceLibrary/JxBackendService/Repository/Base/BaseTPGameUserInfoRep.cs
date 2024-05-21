using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Extensions;
using System;

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
    }
}