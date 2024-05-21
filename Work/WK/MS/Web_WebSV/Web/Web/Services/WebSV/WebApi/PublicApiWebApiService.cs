using JxBackendService.Model.Entity;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System.Collections.Generic;
using Web.Services.WebSV.Base;

namespace Web.Services.WebSV.WebApi
{
    public class PublicApiWebApiService : BaseWebSVService, IPublicApiWebSVService
    {
        protected override string RemoteControllerName => "PublicApiService";

        public string GetRemoteCacheForOldService(string key)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(queryStringParts);
        }

        public bool IsAllowExecutingAction(string key, double cacheSeconds)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
                { "cacheSeconds", cacheSeconds.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(queryStringParts) == "true";
        }

        public bool IsAllowExecutingActionWithLock(string key, double cacheSeconds)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
                { "cacheSeconds", cacheSeconds.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(queryStringParts) == "true";
        }

        public void RemoveRemoteCacheForOldService(string key)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "key", key },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            GetHttpGetResponse<FrontsideMenu>(queryStringParts);
        }

        public void SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam)
        {
            GetHttpPostResponse<object>(setRemoteCacheParam);
        }
    }
}