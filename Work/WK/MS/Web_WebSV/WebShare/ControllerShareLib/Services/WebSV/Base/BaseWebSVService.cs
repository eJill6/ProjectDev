using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Interfaces.Service.Security;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.ViewModel;
using System.Net;
using System.Web;

namespace ControllerShareLib.Services.WebSV.Base
{
    public abstract class BaseWebSVService
    {
        private static readonly int s_unauthorizedCacheMilliseconds = 1000;

        private static readonly HashSet<string> s_checkCacheByPassapiPath = new HashSet<string>
        {
            "/SLPolyGameService/ValidateLogin"
        };

        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<IHeaderInspectorService> _headerInspector;

        private readonly Lazy<IWebSVServiceSettingService> _webSVServiceSettingService;

        private readonly Lazy<ICache> _cache;

        private readonly Lazy<IHttpContextUserService> _httpContextUserService;

        protected abstract string RemoteControllerName { get; }

        public BaseWebSVService()
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _headerInspector = DependencyUtil.ResolveService<IHeaderInspectorService>();
            _webSVServiceSettingService = DependencyUtil.ResolveService<IWebSVServiceSettingService>();
            _cache = DependencyUtil.ResolveService<ICache>();
            _httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>();
        }

        protected T GetHttpGetResponse<T>(string actionName, params string[] queryStringParts)
        {
            return GetHttpGetResponseString(actionName, queryStringParts).Deserialize<T>();
        }

        protected string GetHttpGetResponseString(string actionName, params string[] queryStringParts)
        {
            WebRequestParam webRequestParam = CreateHttpGetRequestParam(actionName, queryStringParts);
            string response = _httpWebRequestUtilService.Value.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            //因為interface跟api,service耦合一起,故這邊直接用拋出例外的方式讓middleware判斷
            if (httpStatusCode != HttpStatusCode.OK)
            {
                if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    SetSVUnauthorizedCache();
                }

                throw new HttpStatusException(httpStatusCode);
            }

            return response;
        }

        protected async Task<T> GetHttpGetResponseStringAsync<T>(string actionName, params string[] queryStringParts)
        {
            return (await GetHttpGetResponseStringAsync(actionName, queryStringParts)).Content.Deserialize<T>();
        }

        protected Task<ResponseInfo> GetHttpGetResponseStringAsync(string actionName, params string[] queryStringParts)
        {
            WebRequestParam webRequestParam = CreateHttpGetRequestParam(actionName, queryStringParts);
            Task<ResponseInfo> responseInfo = _httpWebRequestUtilService.Value.GetResponseAsync(webRequestParam);

            return responseInfo;
        }

        protected T GetHttpPostResponse<T>(string actionName, object bodyModel)
        {
            string apiPath = GetApiPath(actionName);

            return BaseGetHttpPostResponseString(apiPath, bodyModel).Deserialize<T>();
        }

        //protected T GetHttpPostResponse<T>(string apiPath, object bodyModel)
        //{
        //    return GetHttpPostResponseString(apiPath, bodyModel).Deserialize<T>();
        //}

        protected string GetHttpPostResponseString(string actionName, object bodyModel)
        {
            string apiPath = GetApiPath(actionName);

            return BaseGetHttpPostResponseString(apiPath, bodyModel);
        }

        protected string[] GetQueryStringParts(Dictionary<string, string> queryStringMap)
        {
            return queryStringMap.Select(s => $"{s.Key}={HttpUtility.UrlEncode(s.Value)}").ToArray();
        }

        private string BaseGetHttpPostResponseString(string apiPath, object bodyModel)
        {
            string coreServiceUrl = _configUtilService.Value.Get("CoreServiceUrl");
            Dictionary<string, string> headers = GetHeaders(apiPath);

            var webRequestParam = new WebRequestParam()
            {
                Url = _httpWebRequestUtilService.Value.CombineUrl(coreServiceUrl, apiPath),
                Body = bodyModel.ToJsonString(),
                IsResponseValidJson = false,
                Method = JxBackendService.Common.Util.HttpMethod.Post,
                Purpose = apiPath,
                Headers = headers,
                TimeOut = _webSVServiceSettingService.Value.WebRequestWebSVWaitMilliSeconds,
            };

            string response = _httpWebRequestUtilService.Value.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            //因為interface跟api,service耦合一起,故這邊直接用拋出例外的方式讓middleware判斷
            if (httpStatusCode != HttpStatusCode.OK)
            {
                if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    SetSVUnauthorizedCache();
                }

                throw new HttpStatusException(httpStatusCode);
            }

            return response;
        }

        /// <summary>
        /// 取得Header資料，並檢查是否有401的異常
        /// </summary>
        /// <returns>Header資料</returns>
        /// <exception cref="HttpStatusException">有401會拋401的異常例外</exception>
        private Dictionary<string, string> GetHeaders(string apiPath)
        {
            BasicUserInfo basicUserInfo = _httpContextUserService.Value.GetBasicUserInfo();
            Dictionary<string, string> headers = _headerInspector.Value.CreateRequestHeader(basicUserInfo);

            if (!s_checkCacheByPassapiPath.Contains(apiPath))
            {
                string cacheKey = CacheKeyHelper.SVUnauthorized(basicUserInfo);
                var isCacheUnauthorized = _cache.Value.Get<bool?>(cacheKey);

                if (isCacheUnauthorized.HasValue && isCacheUnauthorized.Value)
                {
                    throw new HttpStatusException(HttpStatusCode.Unauthorized);
                }
            }

            return headers;
        }

        private WebRequestParam CreateHttpGetRequestParam(string actionName, params string[] queryStringParts)
        {
            string apiPath = GetApiPath(actionName);

            if (queryStringParts.AnyAndNotNull())
            {
                apiPath += "?";
            }

            string coreServiceUrl = _configUtilService.Value.Get("CoreServiceUrl");
            var totalParts = new List<string>() { coreServiceUrl, apiPath };

            if (queryStringParts.AnyAndNotNull())
            {
                totalParts.AddRange(queryStringParts);
            }

            string fullUrl = _httpWebRequestUtilService.Value.CombineUrl(totalParts.ToArray());

            var webRequestParam = new WebRequestParam()
            {
                Url = fullUrl,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = false,
                Method = JxBackendService.Common.Util.HttpMethod.Get,
                Purpose = apiPath,
                Headers = GetHeaders(apiPath),
                TimeOut = _webSVServiceSettingService.Value.WebRequestWebSVWaitMilliSeconds,
            };

            return webRequestParam;
        }

        private string GetApiPath(string actionName) => $"/{RemoteControllerName}/{actionName}";

        private void SetSVUnauthorizedCache()
        {
            BasicUserInfo basicUserInfo = _httpContextUserService.Value.GetBasicUserInfo();
            string cacheKey = CacheKeyHelper.SVUnauthorized(basicUserInfo);
            _cache.Value.Set(cacheKey, obj: true, expired: DateTime.Now.AddMilliseconds(s_unauthorizedCacheMilliseconds));
        }
    }
}