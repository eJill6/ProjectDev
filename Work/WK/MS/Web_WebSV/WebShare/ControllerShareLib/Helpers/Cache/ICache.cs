namespace ControllerShareLib.Helpers.Cache
{
    public interface ICache
    {
        T Get<T>(string key);

        bool Set<T>(string key, T obj);

        bool Set<T>(string key, T obj, DateTime expired);

        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T obj, DateTime expired);

        Task<T> GetOrAddAsync<T>(string funcName, string key, Func<Task<T>> invoke, DateTime expired);

        bool Del(string key);
    }

    public interface ICacheRemote : ICache
    {

        Dictionary<string, T> Get<T>(string[] key);

        Task<Dictionary<string, T>> GetAsync<T>(string[] key);
        
        Task<bool> SAddAsync(string key, object[] values, DateTime expired);
        
        Task<T[]> SMembersAsync<T>(string key);
    }
}
