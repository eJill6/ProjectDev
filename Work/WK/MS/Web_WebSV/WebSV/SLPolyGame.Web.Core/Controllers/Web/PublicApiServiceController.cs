using JxBackendService.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class PublicApiServiceController : BaseApiController, IPublicApiWebSVService
    {
        private readonly Lazy<IPublicApiWebSVService> _publicApiWebSVService;

        public PublicApiServiceController()
        {
            _publicApiWebSVService = DependencyUtil.ResolveService<IPublicApiWebSVService>();
        }

        [HttpGet]
        public async Task<string> GetRemoteCacheForOldService(string key)
            => await _publicApiWebSVService.Value.GetRemoteCacheForOldService(key);

        [HttpPost]
        public async Task<Dictionary<string, string>> GetRemoteCacheForOldService(string[] keys)
            => await _publicApiWebSVService.Value.GetRemoteCacheForOldService(keys);

        [HttpGet]
        public async Task<bool> IsAllowExecutingAction(string key, double cacheSeconds)
            => await _publicApiWebSVService.Value.IsAllowExecutingAction(key, cacheSeconds);

        [HttpGet]
        public async Task<bool> IsAllowExecutingActionWithLock(string key, double cacheSeconds)
            => await _publicApiWebSVService.Value.IsAllowExecutingActionWithLock(key, cacheSeconds);

        [HttpGet]
        public async Task RemoveRemoteCacheForOldService(string key)
            => await _publicApiWebSVService.Value.RemoveRemoteCacheForOldService(key);

        [HttpPost]
        public async Task SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam)
            => await _publicApiWebSVService.Value.SetRemoteCacheForOldService(setRemoteCacheParam);
    }
}