using JxBackendService.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface ICachedLoginUserInfoRep
    {
        List<CachedLoginUserInfo> GetTopNDataByOverTime(int topN, DateTime dateTime);
        bool UpdateCachedLoginUserInfoTime(string userInfoCacheKey);
        bool DeleteCachedLoginUserInfo(string userInfoCacheKey);
    }
}
