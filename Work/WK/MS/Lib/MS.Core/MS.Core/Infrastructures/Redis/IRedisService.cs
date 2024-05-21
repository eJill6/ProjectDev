namespace MS.Core.Infrastructure.Redis
{
    /// <summary>
    /// 與RedisDb溝通的介面
    /// </summary>
    public interface IRedisService
    {
        Task<T> GetCache<T>(int cacheIndexes, string cacheKey, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, Func<Task<T>> value) where T : class;
        Task<T1> GetCache<T, T1>(int cacheIndexes, string cacheKey, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, Func<Task<T1>> value) where T1 : IEnumerable<T>;
        Task<T> GetOrSetAsync<T>(string key, TimeSpan timeSpan, Func<Task<T>> func, int cacheIndexes = 5);
        long Incr(string key, int expireSeconds, int cacheIndexes = 5);

        /// <summary>
        /// 計數器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="cacheIndexes"></param>
        /// <returns></returns>
        Task<long> IncrAsync(string key, int expireSeconds = 3, int cacheIndexes = 5);
        Task RemoveCache(int cacheIndexes, string cacheKey);
    }
}