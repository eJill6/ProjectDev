using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web;

namespace ControllerShareLib.Services
{
    public class BaseByteArrayApiService : IByteArrayApiService
    {
        public string EncBytesHeader => "enc-bytes";

        private static readonly string s_isEncodingValue = "true";

        private static string EncodePathHeader => "enc-path";

        private static readonly bool s_isAllowUnencryptedRequest;

        private static readonly HashSet<string> s_alwaysAllowPaths = new() { "/check.html" };

        static BaseByteArrayApiService()
        {
            var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            s_isAllowUnencryptedRequest = configUtilService.Value.Get("AllowUnencryptedRequest") == "true";
        }

        public virtual bool IsEncodingRequired(HttpRequest httpRequest)
        {
            return httpRequest.Headers[EncBytesHeader] == s_isEncodingValue;
        }

        public bool IsUseEncodingPath(IHeaderDictionary headers)
        {
            return headers.ContainsKey(EncodePathHeader);
        }

        public async Task DecodeToBodyAsync(HttpRequest httpRequest)
        {
            if (httpRequest.Method != JxBackendService.Common.Util.HttpMethod.Post)
            {
                return;
            }

            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
            {
                string requestBody = await reader.ReadToEndAsync();

                if (requestBody.IsNullOrEmpty())
                {
                    return;
                }

                requestBody = ProcessRequestBody(requestBody);

                string json = IsUseEncodingPath(httpRequest.Headers)
                    ? XorEncryptTool.XorDecryptToString(requestBody)
                    : Encoding.UTF8.GetString(Convert.FromBase64String(requestBody));

                json = ProcessRequestBodyJson(json);

                httpRequest.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));
            }
        }

        protected virtual string ProcessRequestBodyJson(string json)
        {
            //do nothing;
            return json;
        }

        protected virtual string ProcessRequestBody(string requestBody)
        {
            //do nothing;
            return requestBody;
        }

        public async Task EncodeResponseAsync(HttpResponse httpResponse, string json, IHeaderDictionary headers)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await EncodeResponseAsync(httpResponse, bytes, headers);
        }

        public async Task EncodeResponseAsync(HttpResponse httpResponse, byte[] responseBytes,
            IHeaderDictionary headers)
        {
            if (!responseBytes.Any())
            {
                return;
            }

            var bytesString = IsUseEncodingPath(headers)
                ? XorEncryptTool.XorEncryptToString(responseBytes)
                : Convert.ToBase64String(responseBytes);

            httpResponse.ContentType = "text/plain";
            httpResponse.Headers.Add(EncBytesHeader, s_isEncodingValue);

            await httpResponse.WriteAsync(bytesString);
        }

        public void DecodeEncodingPath(HttpRequest httpRequest)
        {
            string decodePath = GetDecodePath(httpRequest);

            if (decodePath.IsNullOrEmpty())
            {
                return;
            }

            string[] decodeData = decodePath.Split('?', 2);
            httpRequest.Path = decodeData[0];

            if (decodeData.Length <= 1)
            {
                return;
            }

            //這邊使用原始split拆解而不套用其他原生helper(例如QueryHelpers)，讓處理的效能最大化
            var queryBuilder = new QueryBuilder();

            foreach (string value in decodeData[1].Split('&'))
            {
                string[] queryKeyValue = value.Split('=', 2);
                queryBuilder.Add(queryKeyValue[0], queryKeyValue[1]);
            }

            httpRequest.QueryString = queryBuilder.ToQueryString();
        }

        protected virtual string GetDecodePath(HttpRequest httpRequest)
        {
            if (!httpRequest.Headers.TryGetValue(EncodePathHeader, out StringValues encodePath)
                || encodePath.IsNullOrEmpty())
            {
                return string.Empty;
            }

            //因為傳過來的是url path + query string，需要額外做url decode
            return HttpUtility.UrlDecode(XorEncryptTool.XorDecryptToString(encodePath));
        }

        public virtual bool CheckAllowUnencryptedRequest(HttpRequest httpRequest)
            => s_isAllowUnencryptedRequest
               || s_alwaysAllowPaths.Contains(httpRequest.Path);

        /// <summary>M不開放從ep傳入enc-path</summary>
        public virtual bool IsEncodingPathFromQueryString(HttpRequest request)
        {
            return false;
        }
    }
}