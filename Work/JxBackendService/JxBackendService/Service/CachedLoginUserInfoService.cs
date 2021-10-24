using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service
{
    public class CachedLoginUserInfoService : BaseService, ICachedLoginUserInfoService, ICachedLoginUserInfoReadService
    {
        private readonly ICachedLoginUserInfoRep _cachedLoginUserInfoRep;

        public CachedLoginUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _cachedLoginUserInfoRep = new CachedLoginUserInfoRep(envLoginUser, dbConnectionType);
        }

        public List<CachedLoginUserInfo> GetTopNDataByOverTime(int topN, DateTime dateTime)
        {
            return _cachedLoginUserInfoRep.GetTopNDataByOverTime(topN, dateTime);
        }

        public bool UpdateCachedLoginUserInfoTime(string userInfoCacheKey)
        {
            return _cachedLoginUserInfoRep.UpdateCachedLoginUserInfoTime(userInfoCacheKey);
        }

        public bool DeleteCachedLoginUserInfo(string userInfoCacheKey)
        {
            return _cachedLoginUserInfoRep.DeleteCachedLoginUserInfo(userInfoCacheKey);
        }

    }
}
