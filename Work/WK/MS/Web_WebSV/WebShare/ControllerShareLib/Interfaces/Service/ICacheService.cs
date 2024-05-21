namespace ControllerShareLib.Interfaces.Service
{
    public interface ICacheService
    {
        void Del(string key);

        T Get<T>(string key, DateTime? expiredTime = null, Func<T> getData = null);

        T Get<T>(string key, bool slideExpire, DateTime? expiredTime = null, Func<T> getData = null);

        T GetOrSetByRedis<T>(string key, Func<T> getData, DateTime expiredTime);

        T GetByRedis<T>(string key);

        Dictionary<string, T> GetByRedis<T>(string[] keys);

        bool Set<T>(string key, T value);


        bool Set<T>(string key, T value, DateTime expired);

        T GetByRedisRawData<T>(string key);

        T GetLocalCache<T>(string key,
            Func<T> getData = null, Func<T, DateTime> expiredTimeLogic = null);

        bool SetLocalCache<T>(string key, T value, DateTime expired);

        Task<T> GetLocalCacheAsync<T>(string key);

        Task<bool> SetLocalCacheAsync<T>(string key, T value, DateTime expired);

        Task<Dictionary<string, T>> GetByRedisAsync<T>(string[] keys);
        
        Task<bool> SAddAsync(string key, object[] values, DateTime expired);
        
        Task<T[]> SMembersAsync<T>(string key);
    }
}