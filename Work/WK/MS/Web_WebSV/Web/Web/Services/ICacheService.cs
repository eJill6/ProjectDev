using System;

namespace Web.Services
{
    public interface ICacheService
    {
        void Del(string key);
        T Get<T>(string key, DateTime? expiredTime = null, Func<T> getData = null);
        T Get<T>(string key, bool slideExpire, DateTime? expiredTime = null, Func<T> getData = null);
        T GetOrSetByRedis<T>(string key, Func<T> getData, DateTime expiredTime);
        T GetByRedis<T>(string key);
        bool Set<T>(string key, T value);
        bool Set<T>(string key, T value, DateTime expired);
        T GetByRedisRawData<T>(string key);
    }
}