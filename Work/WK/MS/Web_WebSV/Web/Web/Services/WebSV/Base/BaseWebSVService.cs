using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Web.Extensions;
using Web.Helpers.Security;
using JxBackendService.Common.Extensions;
using JxBackendService.Interface.Service.Config;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Web.Services.WebSV.Base
{
    public abstract class BaseWebSVService
    {
        private readonly IHttpWebRequestUtilService _httpWebRequestUtilService;

        private readonly IConfigUtilService _configUtilService;

        private readonly MessageInspector _messageInspector;

        protected abstract string RemoteControllerName { get; }

        private static readonly HashSet<string> s_ignoreMethodNames = new HashSet<string>()
        {
            nameof(GetHttpPostResponse),
            nameof(GetHttpPostResponseString),
            nameof(GetHttpGetResponse),
            nameof(GetHttpGetResponseString),
            nameof(GetDefaultApiPath),
            nameof(GetCallerMethodName)
        };

        public BaseWebSVService()
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _messageInspector = new MessageInspector();
        }

        protected T GetHttpGetResponse<T>(params string[] queryStringParts)
        {
            return GetHttpGetResponseString(queryStringParts).Deserialize<T>();
        }

        protected string GetHttpGetResponseString(params string[] queryStringParts)
        {
            WebRequestParam webRequestParam = CreateHttpGetRequestParam(GetCallerMethodName(), queryStringParts);
            string response = _httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            return response;
        }

        protected async Task<T> GetHttpGetResponseStringAsync<T>(string actionName, params string[] queryStringParts)
        {
            return (await GetHttpGetResponseStringAsync(actionName, queryStringParts)).Content.Deserialize<T>();
        }

        protected Task<ResponseInfo> GetHttpGetResponseStringAsync(string actionName, params string[] queryStringParts)
        {
            WebRequestParam webRequestParam = CreateHttpGetRequestParam(actionName, queryStringParts);
            Task<ResponseInfo> responseInfo = _httpWebRequestUtilService.GetResponseAsync(webRequestParam);

            return responseInfo;
        }

        protected T GetHttpPostResponse<T>(object bodyModel)
        {
            string apiPath = GetDefaultApiPath();

            return GetHttpPostResponseString(apiPath, bodyModel).Deserialize<T>();
        }

        //protected T GetHttpPostResponse<T>(string apiPath, object bodyModel)
        //{
        //    return GetHttpPostResponseString(apiPath, bodyModel).Deserialize<T>();
        //}

        protected string GetHttpPostResponseString(object bodyModel)
        {
            string apiPath = GetDefaultApiPath();

            return GetHttpPostResponseString(apiPath, bodyModel);
        }

        protected string[] GetQueryStringParts(Dictionary<string, string> queryStringMap)
        {
            return queryStringMap.Select(s => $"{s.Key}={HttpUtility.UrlEncode(s.Value)}").ToArray();
        }

        private string GetHttpPostResponseString(string apiPath, object bodyModel)
        {
            string coreServiceUrl = _configUtilService.Get("CoreServiceUrl");

            var webRequestParam = new WebRequestParam()
            {
                Url = _httpWebRequestUtilService.CombineUrl(coreServiceUrl, apiPath),
                Body = bodyModel.ToJsonString(),
                IsResponseValidJson = false,
                Method = HttpMethod.Post,
                Purpose = apiPath,
                Headers = _messageInspector.CreateRequestHeader()
            };

            string response = _httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            return response;
        }

        private WebRequestParam CreateHttpGetRequestParam(string actionName, params string[] queryStringParts)
        {
            string apiPath = GetDefaultApiPath(actionName);

            if (queryStringParts.AnyAndNotNull())
            {
                apiPath += "?";
            }

            string coreServiceUrl = _configUtilService.Get("CoreServiceUrl");
            var totalParts = new List<string>() { coreServiceUrl, apiPath };
            totalParts.AddRange(queryStringParts);
            string fullUrl = _httpWebRequestUtilService.CombineUrl(totalParts.ToArray());

            var webRequestParam = new WebRequestParam()
            {
                Url = fullUrl,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = false,
                Method = HttpMethod.Get,
                Purpose = apiPath,
                Headers = _messageInspector.CreateRequestHeader()
            };

            return webRequestParam;
        }

        private string GetCallerMethodName()
        {
            StackTrace stackTrace = new StackTrace();

            for (int i = 1; i < stackTrace.GetFrames().Length; i++)
            {
                StackFrame stackFrame = stackTrace.GetFrame(i);
                MethodBase methodBase = stackFrame.GetMethod();

                if (s_ignoreMethodNames.Contains(methodBase.Name))
                {
                    continue;
                }

                return methodBase.Name;
            }

            throw new ArgumentOutOfRangeException();
        }

        private string GetDefaultApiPath() => GetDefaultApiPath(GetCallerMethodName());

        private string GetDefaultApiPath(string actionName) => $"/{RemoteControllerName}/{actionName}";
    }
}