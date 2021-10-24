using JxBackendService.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface ICachedLoginUserInfoReadService
    {
        List<CachedLoginUserInfo> GetTopNDataByOverTime(int topN, DateTime dateTime);
    }

    public interface ICachedLoginUserInfoService
    {
        bool UpdateCachedLoginUserInfoTime(string userInfoCacheKey);
        bool DeleteCachedLoginUserInfo(string userInfoCacheKey);
    }
}
