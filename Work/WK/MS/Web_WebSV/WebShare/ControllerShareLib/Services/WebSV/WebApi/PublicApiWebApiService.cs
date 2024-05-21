using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Model.Entity;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services.WebSV.WebApi
{
    public class PublicApiWebApiService : BaseWebSVService, IPublicApiWebSVService
    {
        protected override string RemoteControllerName => "PublicApiService";

        public async Task<string> GetRemoteCacheForOldService(string key)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponseString(nameof(GetRemoteCacheForOldService), queryStringParts));
        }

        public async Task<Dictionary<string, string>> GetRemoteCacheForOldService(string[] keys)
        {
            return await Task.FromResult(GetHttpPostResponse<Dictionary<string, string>>(nameof(GetRemoteCacheForOldService), keys));
        }

        public async Task<bool> IsAllowExecutingAction(string key, double cacheSeconds)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
                { "cacheSeconds", cacheSeconds.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponseString(nameof(IsAllowExecutingAction), queryStringParts) == "true");
        }

        public async Task<bool> IsAllowExecutingActionWithLock(string key, double cacheSeconds)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
                { "cacheSeconds", cacheSeconds.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponseString(nameof(IsAllowExecutingActionWithLock), queryStringParts) == "true");
        }

        public async Task RemoveRemoteCacheForOldService(string key)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            GetHttpGetResponse<FrontsideMenu>(nameof(RemoveRemoteCacheForOldService), queryStringParts);

            await Task.CompletedTask;
        }

        public async Task SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam)
        {
            GetHttpPostResponse<object>(nameof(SetRemoteCacheForOldService), setRemoteCacheParam);
            await Task.CompletedTask;
        }
    }
}