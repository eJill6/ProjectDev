using JxBackendService.DependencyInjection;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace Web.Services.WebSV.WCF
{
    public class PublicApiWCFService : IPublicApiWebSVService
    {
        private readonly PublicApiService.IPublicApiService _publicApiService;

        public PublicApiWCFService()
        {
            _publicApiService = DependencyUtil.ResolveService<PublicApiService.IPublicApiService>();
        }

        public string GetRemoteCacheForOldService(string key)
            => _publicApiService.GetRemoteCacheForOldService(key);

        public bool IsAllowExecutingAction(string key, double cacheSeconds)
            => _publicApiService.IsAllowExecutingAction(key, cacheSeconds);

        public bool IsAllowExecutingActionWithLock(string key, double cacheSeconds)
            => _publicApiService.IsAllowExecutingActionWithLock(key, cacheSeconds);

        public void RemoveRemoteCacheForOldService(string key)
            => _publicApiService.RemoveRemoteCacheForOldService(key);

        public void SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam)
            => _publicApiService.SetRemoteCacheForOldService(setRemoteCacheParam);
    }
}