using JxBackendService.Common.Util.Cache;

namespace ControllerShareLib.Helpers.Cache;

public interface ICacheBase
{
    bool AddCache(string key, string value, int expireSeconds, bool isSliding);

    CacheObj CreateCacheObj(string key, string value, int expireSeconds, bool isSliding);

    bool DeleteCache(string key);

    Task<T> Get<T>(string key);

    string GetCache(string key);

    Dictionary<string, string> GetCache(string[] keys);

    Task<string> GetCacheAsync(string key);

    Task<Dictionary<string, string>> GetCacheAsync(string[] keys);
    
    Task<bool> SAddAsync(string key, object[] values);
    
    Task<bool> ExpireAsync(string key, int cacheSeconds);
    
    Task<T[]> SMembersAsync<T>(string key);
}