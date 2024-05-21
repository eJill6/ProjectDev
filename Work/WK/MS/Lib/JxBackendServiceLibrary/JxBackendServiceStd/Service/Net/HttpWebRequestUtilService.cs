using Flurl;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Service.Net
{
    public class HttpWebRequestUtilService : IHttpWebRequestUtilService
    {
        public string CombineUrl(params string[] parts)
        {
            return Url.Combine(parts);
        }

        public virtual string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            return HttpWebRequestUtil.GetResponse(webRequestParam, out httpStatusCode);
        }

        public virtual Task<ResponseInfo> GetResponseAsync(WebRequestParam webRequestParam)
        {
            return HttpWebRequestUtil.GetResponseAsync(webRequestParam);
        }
    }

    [MockService]
    public class HttpWebRequestUtilMockService : HttpWebRequestUtilService
    {
        public override string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            httpStatusCode = HttpStatusCode.OK;
            string content = string.Empty;

            return content;
        }

        //public string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        //{
        //    httpStatusCode = HttpStatusCode.OK;

        //    return new ABBetLogResponseModel().ToJsonString();
        //}
    }

    [MockService]
    public class HttpWebRequestUtilLogCurlMockService : HttpWebRequestUtilService
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        public HttpWebRequestUtilLogCurlMockService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public override string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            string curlScript = GenerateCurlScript(webRequestParam);
            _logUtilService.Value.ForcedDebug($"curlScript: {curlScript}");

            return base.GetResponse(webRequestParam, out httpStatusCode);
        }

        private static string GenerateCurlScript(WebRequestParam webRequestParam)
        {
            var curlScript = new StringBuilder("curl ");

            if (webRequestParam.Method != HttpMethod.Get)
            {
                curlScript.Append($"-X {webRequestParam.Method} ");
            }

            curlScript.Append($"\"{webRequestParam.Url}\" ");

            if (webRequestParam.Method != HttpMethod.Get)
            {
                curlScript.Append($" -H \"Content-Type: {webRequestParam.ContentType}\"");
            }

            if (webRequestParam.Headers.AnyAndNotNull())
            {
                foreach (KeyValuePair<string, string> header in webRequestParam.Headers)
                {
                    curlScript.Append($" -H \"{header.Key}: {header.Value}\"");
                }
            }

            if (webRequestParam.Method == HttpMethod.Post && !webRequestParam.Body.IsNullOrEmpty())
            {
                string escapedBody = webRequestParam.Body.Replace("\"", "\\\"");
                curlScript.Append($" -d \"{escapedBody}\"");
            }

            return curlScript.ToString();
        }
    }
}