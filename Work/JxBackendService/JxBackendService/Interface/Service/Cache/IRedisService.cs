using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Cache
{
    public interface IRedisService
    {
        T GetCache<T>(DbIndexes dbIndex, string key, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, bool isDoSerialize, Func<T> getCacheData) where T : class;
        
        void SetCache<TItem>(DbIndexes dbIndex, string key, TItem value, int cacheSeconds, bool isDoSerialize);

        void RemoveCache(DbIndexes dbIndex, params string[] key);
    }
}
