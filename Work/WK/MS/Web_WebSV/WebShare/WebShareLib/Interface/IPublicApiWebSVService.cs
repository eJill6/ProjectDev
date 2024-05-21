using SLPolyGame.Web.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Interface
{
    public interface IPublicApiWebSVService
    {
        Task<string> GetRemoteCacheForOldService(string key);

        Task<Dictionary<string, string>> GetRemoteCacheForOldService(string[] keys);

        Task<bool> IsAllowExecutingAction(string key, double cacheSeconds);

        Task<bool> IsAllowExecutingActionWithLock(string key, double cacheSeconds);

        Task RemoveRemoteCacheForOldService(string key);

        Task SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam);
    }
}