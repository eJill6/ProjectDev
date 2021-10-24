using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository
{
    public class CachedLoginUserInfoRep : BaseDbRepository<CachedLoginUserInfo>, ICachedLoginUserInfoRep
    {
        public CachedLoginUserInfoRep(EnvironmentUser environmentUser, DbConnectionTypes dbConnectionType) : base(environmentUser, dbConnectionType) { }

        public List<CachedLoginUserInfo> GetTopNDataByOverTime(int topN, DateTime dateTime)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, topN) + " WHERE CreateDateTime < @createDateTime ";
            return DbHelper.QueryList<CachedLoginUserInfo>(sql, new { CreateDateTime = dateTime });
        }

        public bool UpdateCachedLoginUserInfoTime(string userInfoCacheKey)
        {
            string sql = $"UPDATE {InlodbType.Inlodb}.dbo.{nameof(CachedLoginUserInfo)} SET CreateDateTime = GETDATE() WHERE UserInfoCacheKey = @userInfoCacheKey";
            return DbHelper.Execute(sql, new { userInfoCacheKey = userInfoCacheKey.ToVarchar(32) }) > 0;
        }

        public bool DeleteCachedLoginUserInfo(string userInfoCacheKey)
        {
            string sql = $"DELETE {InlodbType.Inlodb}.dbo.{nameof(CachedLoginUserInfo)} WHERE UserInfoCacheKey = @userInfoCacheKey";
            return DbHelper.Execute(sql, new { userInfoCacheKey = userInfoCacheKey.ToVarchar(32) }) > 0;
        }
    }
}
