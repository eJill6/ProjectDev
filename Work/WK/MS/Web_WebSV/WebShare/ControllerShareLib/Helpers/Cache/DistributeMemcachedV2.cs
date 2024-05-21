using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using System.Collections.Concurrent;

namespace ControllerShareLib.Helpers.Cache;

/// <summary>
/// V2版本將透過SV存取Redis改為直接對Redis做存取，不透過SV
/// </summary>
public class DistributeMemcachedV2 : ICacheRemote
{
    private readonly Lazy<ICacheBase> _cacheBase;
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

    public DistributeMemcachedV2()
    {
        _cacheBase = DependencyUtil.ResolveService<ICacheBase>();
    }

    public bool Del(string key)
    {
        if (key.IsNullOrEmpty())
        {
            return false;
        }

        _cacheBase.Value.DeleteCache(key);

        // redisService這邊是void 因此這邊直接回true
        return true;
    }

    public T Get<T>(string key)
    {
        if (key.IsNullOrEmpty())
        {
            return default;
        }

        string cacheValue = _cacheBase.Value.GetCache(key);

        if (cacheValue.IsNullOrEmpty())
        {
            return default;
        }

        return cacheValue.Deserialize<T>();
    }

    public Dictionary<string, T> Get<T>(string[] keys)
    {
        var result = new Dictionary<string, T>();
        if (keys.Length <= 0)
        {
            return result;
        }

        var cacheValues = _cacheBase.Value.GetCache(keys);

        foreach (var item in cacheValues)
        {
            var cacheValue = item.Value;
            if (!cacheValue.IsNullOrEmpty())
            {
                result[item.Key] = cacheValue.Deserialize<T>();
            }
        }

        return result;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        if (key.IsNullOrEmpty())
        {
            return default;
        }

        string cacheValue = await _cacheBase.Value.GetCacheAsync(key);

        if (cacheValue.IsNullOrEmpty())
        {
            return default;
        }

        return cacheValue.Deserialize<T>();
    }

    public async Task<Dictionary<string, T>> GetAsync<T>(string[] keys)
    {
        var result = new Dictionary<string, T>();
        if (keys.Length <= 0)
        {
            return result;
        }

        var cacheValues = await _cacheBase.Value.GetCacheAsync(keys);

        foreach (var item in cacheValues)
        {
            var cacheValue = item.Value;
            if (!cacheValue.IsNullOrEmpty())
            {
                result[item.Key] = cacheValue.Deserialize<T>();
            }
        }

        return result;
    }

    public async Task<T> GetOrAddAsync<T>(string funcName, string key, Func<Task<T>> invoke, DateTime expired)
    {
        if (string.IsNullOrEmpty(key))
        {
            return default;
        }
        T data = default;
        var locker = _locks.GetOrAdd<SemaphoreSlim>(funcName, (key, old) => old, new SemaphoreSlim(1, 1));
        try
        {
            await locker.WaitAsync();
            var remoteObj = Get<T>(key);

            if (remoteObj != null)
            {
                return remoteObj;
            }

            data = await invoke();

            if (data != null)
            {
                Set(key, data, expired);
            }
        }
        finally
        {
            locker.Release();
        }
        return data;
    }

    public bool Set<T>(string key, T obj)
    {
        return Set(key, obj, DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays)); //預設30天
    }

    public bool Set<T>(string key, T obj, DateTime expired)
    {
        if (key.IsNullOrEmpty())
        {
            return false;
        }

        if (obj == null)
        {
            return true;
        }

        // 計算距離expired還有多少秒
        double cacheSeconds = expired.Subtract(DateTime.Now).TotalSeconds;

        // 因為這邊的cache時間都是用截止時間去計算的, 不是用延長的方式, 所以 isSlidingExpiration 都設成false
        bool isSlidingExpiration = false;
        string value = obj.ToJsonString();

        _cacheBase.Value.AddCache(
            key,
            value,
            Convert.ToInt32(cacheSeconds),
            isSlidingExpiration);

        // redisService這邊是void 因此這邊直接回true
        return true;
    }

    public Task<bool> SetAsync<T>(string key, T obj, DateTime expired)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> SAddAsync(string key, object[] values, DateTime expired)
    {
        if (key.IsNullOrEmpty())
        {
            return false;
        }

        if (values == null || values.Length == 0)
        {
            return true;
        }

        bool isSAdd = await _cacheBase.Value.SAddAsync(key, values);
        
        // 計算距離expired還有多少秒
        int cacheSeconds = (int)expired.Subtract(DateTime.Now).TotalSeconds;
        return isSAdd && await _cacheBase.Value.ExpireAsync(key, cacheSeconds);
    }
    
    public async Task<T[]> SMembersAsync<T>(string key)
    {
        if (key.IsNullOrEmpty())
        {
            return default;
        }
        
        return await _cacheBase.Value.SMembersAsync<T>(key);
    }
}