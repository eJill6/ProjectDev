using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructure.Redis.Models.Settings.Redis;
using Newtonsoft.Json;
using System.Reflection;

namespace MS.Core.Infrastructure.Redis
{
    /// <summary>
    /// 與Redis溝通的元件
    /// </summary>
    public class RedisService : IRedisService
    {
        /// <inheritdoc cref="ILogger{RedisClient}"/>
        private readonly ILogger<RedisService> _logger;

        /// <inheritdoc cref="IOptionsMonitor{RedisConnections}"/>
        private readonly IOptionsMonitor<RedisConnections> _setting = null;

        /// <inheritdoc cref="Lazy{FreeRedis.RedisClient}"/>
        private Lazy<FreeRedis.RedisClient> _cli = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisService"/> class.
        /// </summary>
        /// <param name="logger">日誌</param>
        /// <param name="setting">Redis相關設定檔</param>
        public RedisService(ILogger<RedisService> logger,
            IOptionsMonitor<RedisConnections> setting)
        {
            _logger = logger;
            _setting = setting;
            _cli = new Lazy<FreeRedis.RedisClient>(() =>
            {
                var setting = _setting.CurrentValue;
                if (setting.MasterSlave != null && setting.MasterSlave.Master != null)
                {
                    var r = new FreeRedis.RedisClient(
                        setting.MasterSlave.Master,
                        setting.MasterSlave.Slaves.Select(x => { return FreeRedis.ConnectionStringBuilder.Parse(x); }).ToArray());

                    r.Serialize = obj => JsonConvert.SerializeObject(obj);
                    r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
                    return r;
                }
                else if (setting.Sentinel != null)
                {
                    var r = new FreeRedis.RedisClient(
                            setting.Sentinel.ConnectionString,
                            setting.Sentinel.Sentinel.ToArray(),
                            setting.Sentinel.IsReadWritwSeparation);

                    r.Serialize = obj => JsonConvert.SerializeObject(obj);
                    r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
                    return r;
                }

                return null;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="func"></param>
        /// <param name="cacheIndexes"></param>
        /// <returns></returns>
        public async Task<T> GetOrSetAsync<T>(string key, TimeSpan timeSpan, Func<Task<T>> func, int cacheIndexes = 5)
        {
            var result = default(T);
            if (_cli.Value == null)
            {
                return result;
            }

            var cli = _cli.Value.GetDatabase(cacheIndexes);

            if (cli.Exists(key))
            {
                return cli.Get<T>(key);
            }
            result = await func();
            cli.Set(key, result, timeSpan);
            return result;
        }
        /// <summary>
        /// 計數器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="cacheIndexes"></param>
        /// <returns></returns>
        public async Task<long> IncrAsync(string key, int expireSeconds, int cacheIndexes = 5)
        {
            var cli = _cli.Value.GetDatabase(cacheIndexes);
            long times = await cli.IncrAsync(key);
            if(expireSeconds > 0)
            {
                await cli.ExpireAsync(key, expireSeconds);
            }
            return times;
        }
        /// <summary>
        /// 計數器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="cacheIndexes"></param>
        /// <returns></returns>
        public long Incr(string key, int expireSeconds, int cacheIndexes = 5)
        {
            var cli = _cli.Value.GetDatabase(cacheIndexes);
            long times = cli.Incr(key);
            if (times <= 1 && expireSeconds > 0)
            {
                cli.Expire(key, expireSeconds);
            }
            return times;
        }
        public async Task<T> GetCache<T>(int cacheIndexes, string cacheKey, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, Func<Task<T>> create) where T : class
        {
            var result = default(T);
            if (_cli.Value == null)
            {
                return result;
            }

            var cli = _cli.Value.GetDatabase(cacheIndexes);

            try
            {
                if (isForceRefresh)
                {
                    if (create != null)
                    {
                        result = await create();

                        if (result != null)
                        {
                            await cli.SetAsync(cacheKey, result, cacheSeconds);
                        }
                    }
                }
                else
                {
                    result = await cli.GetAsync<T>(cacheKey);
                    if (result == null)
                    {
                        if (create != null)
                        {
                            result = await create();

                            if (result != null)
                            {
                                await cli.SetAsync(cacheKey, result, cacheSeconds);
                            }
                        }
                    }
                    else if (isSlidingExpiration)
                    {
                        await cli.SetAsync(cacheKey, result, cacheSeconds);
                    }
                }
            }
            catch (Exception ex)
            {
                var method = MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fail!, key:{cacheKey}");
            }

            return result;
        }

        public async Task<T1> GetCache<T, T1>(int cacheIndexes, string cacheKey, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, Func<Task<T1>> create) where T1 : IEnumerable<T>
        {
            var result = default(T1);
            if (_cli.Value == null)
            {
                return result;
            }

            var cli = _cli.Value.GetDatabase(cacheIndexes);

            try
            {
                if (isForceRefresh)
                {
                    if (create != null)
                    {
                        result = await create();

                        if (result != null)
                        {
                            await cli.SetAsync(cacheKey, result, cacheSeconds);
                        }
                    }
                }
                else
                {
                    result = await cli.GetAsync<T1>(cacheKey);
                    if (result == null)
                    {
                        if (create != null)
                        {
                            result = await create();

                            if (result != null && result.Count() > 0)
                            {
                                await cli.SetAsync(cacheKey, result, cacheSeconds);
                            }
                        }
                    }
                    else if (isSlidingExpiration)
                    {
                        await cli.SetAsync(cacheKey, result, cacheSeconds);
                    }
                }
            }
            catch (Exception ex)
            {
                var method = MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fail!, key:{cacheKey}");
            }

            return result;
        }

        public async Task RemoveCache(int cacheIndexes, string cacheKey)
        {

            if (_cli.Value == null)
            {
                return;
            }

            var cli = _cli.Value.GetDatabase(cacheIndexes);

            try
            {
                await cli.DelAsync(cacheKey);
            }
            catch (Exception ex)
            {
                var method = MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fail!, key:{cacheKey}");
            }

        }
    }
}