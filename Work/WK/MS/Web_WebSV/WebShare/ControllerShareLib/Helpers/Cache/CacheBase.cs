using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using Newtonsoft.Json;
using System.Reflection;

namespace ControllerShareLib.Helpers.Cache;

public class CacheBase : ICacheBase
{
    /// <inheritdoc cref="Lazy{FreeRedis.RedisClient}"/>
    private Lazy<FreeRedis.RedisClient> _cli = null;
    private readonly IAppSettingService _appSettingService;
    private readonly Lazy<ILogUtilService> _logUtilService;

    public CacheBase()
    {
        var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;
        _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentService.Application, SharedAppSettings.PlatformMerchant).Value;
        _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

        _cli = new Lazy<FreeRedis.RedisClient>(() =>
        {
            string redisConnection = _appSettingService.GetRedisConnectionString(DbIndexes.Default);

            var r = new FreeRedis.RedisClient(redisConnection);

            r.Serialize = obj => JsonConvert.SerializeObject(obj);
            r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
            return r;
        });
    }

    // 增加缓存数据
    public bool AddCache(string key, string value, int expireSeconds, bool isSliding)
    {
        bool result = true;
        CacheObj obj = CreateCacheObj(key, value, expireSeconds, isSliding);
        _cli.Value.Set(obj.Key, obj, obj.ExpireSeconds);

        return result;
    }
    
    public CacheObj CreateCacheObj(string key, string value, int expireSeconds, bool isSliding)
    {
        var obj = new CacheObj
        {
            Key = key,
            Value = value,
            ExpireSeconds = expireSeconds,
            IsSliding = isSliding,
            InsertTime = DateTime.Now
        };

        return obj;
    }

    // 删除缓存数据
    public bool DeleteCache(string key)
    {
        bool result = true;

        _cli.Value.Del(key);

        return result;
    }

    /// <summary>
    /// 获取缓存数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetCache(string key)
    {
        string result = null;
        var cacheData = _cli.Value.Get<CacheObj>(key);

        if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
        {
            result = cacheData.Value;

            if (cacheData.IsSliding)
            {
                AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
            }
        }

        return result;
    }

    /// <summary>
    /// 取得cache資料，泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<T> Get<T>(string key)
    {
        T result = default(T);
        await Task.Run(() =>
        {
            var method = MethodBase.GetCurrentMethod();
            try
            {
                result = _cli.Value.Get<T>(key);
            }
            catch (Exception ex)
            {
                _logUtilService.Value.Error($"{method.Name} fail!, key:{key}, ex:{ex}");
            }
        });

        return result;
    }

    public Dictionary<string, string> GetCache(string[] keys)
    {
        var result = new Dictionary<string, string>();
        var cacheDatas = _cli.Value.MGet<CacheObj>(keys);
        foreach (var item in cacheDatas)
        {
            var cacheData = item;
            if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
            {
                result[item.Key] = cacheData.Value;

                if (cacheData.IsSliding)
                {
                    AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
                }
            }
        }

        return result;
    }



    /// <summary>
    /// 获取缓存数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> GetCacheAsync(string key)
    {
        string result = null;
        var cacheData = await _cli.Value.GetAsync<CacheObj>(key);

        if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
        {
            result = cacheData.Value;

            if (cacheData.IsSliding)
            {
                AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
            }
        }

        return result;
    }



    public async Task<Dictionary<string, string>> GetCacheAsync(string[] keys)
    {
        var result = new Dictionary<string, string>();
        var cacheDatas = await _cli.Value.MGetAsync<CacheObj>(keys);
        foreach (var item in cacheDatas)
        {
            var cacheData = item;
            if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
            {
                result[item.Key] = cacheData.Value;

                if (cacheData.IsSliding)
                {
                    AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
                }
            }
        }

        return result;
    }

    public async Task<bool> SAddAsync(string key, object[] values)
    {
        return Convert.ToBoolean(
            await _cli.Value.SAddAsync(key, values));
    }

    public async Task<bool> ExpireAsync(string key, int cacheSeconds)
    {
        return await _cli.Value.ExpireAsync(key, cacheSeconds);
    }
    
    public async Task<T[]> SMembersAsync<T>(string key)
    {
        return await _cli.Value.SMembersAsync<T>(key);
    }
}