using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Cache
{
    public interface IRedisService
    {
        T GetCache<T>(DbIndexes dbIndex, string key, bool isForceRefresh, double cacheSeconds, bool isSlidingExpiration, bool isDoSerialize, Func<T> getCacheData) where T : class;

        void SetCache<TItem>(DbIndexes dbIndex, string key, TItem value, double cacheSeconds, bool isDoSerialize);

        void RemoveCache(DbIndexes dbIndex, params string[] key);

        long Enqueue<T>(string key, T value);

        T Dequeue<T>(params string[] key);

        bool Expire(DbIndexes dbIndex, string key, int seconds);

        void DoWorkWithLock(string key, Action work);

        T DoWorkWithLock<T>(string key, Func<T> work);
    }
}