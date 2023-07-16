using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Common.Util
{
    public static class HttpWebRequestUtil
    {
        public static string GetResponse<T>(string purpose, HttpMethod httpMethod, string url, T bodyModel)
        {
            return GetResponse(purpose, httpMethod, url, null, bodyModel, true);
        }

        public static string GetResponse<T>(string purpose, HttpMethod httpMethod, string url, Dictionary<string, string> headers, T bodyModel, bool isResponseValidJson)
        {
            bool isCamelCaseNaming = bodyModel.GetType().GetCustomAttributes(true).Where(w => w is CamelCaseNamingAttribute).Any();
            string body = bodyModel.ToJsonString(isCamelCaseNaming: isCamelCaseNaming);

            return GetResponse(
                new WebRequestParam()
                {
                    Purpose = purpose,
                    Method = httpMethod,
                    Url = url,
                    Body = body,
                    ContentType = HttpWebRequestContentType.Json,
                    Headers = headers,
                    IsResponseValidJson = isResponseValidJson
                },
                out HttpStatusCode httpStatusCode);
        }

        public static string GetResponse(string purpose, HttpMethod httpMethod, string url, string body = null)
        {
            return GetResponse(new WebRequestParam()
            {
                Purpose = purpose,
                Method = httpMethod,
                Url = url,
                Body = body,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true
            },
            out HttpStatusCode httpStatusCode);
        }

        public static string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            httpStatusCode = HttpStatusCode.OK;
            ResponseInfo responseInfo = null;

            try
            {
                HttpWebRequest request = CreateHttpRequest(webRequestParam);

                using (WebResponse response = request.GetResponse())
                {
                    responseInfo = ReadToEnd(response);
                }
            }
            catch (Exception ex)
            {
                responseInfo = ProcessException(webRequestParam, ex);
                httpStatusCode = responseInfo.HttpStatusCode;
            }

            if (!webRequestParam.IsResponseValidJson)
            {
                return responseInfo.Content;
            }
            else
            {
                if (webRequestParam.IsResponseValidJson && StringUtil.IsValidJson(responseInfo.Content))
                {
                    return responseInfo.Content;
                }
                else
                {
                    var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                    logUtilService.Error($"IsValidJson=false, Request Param = {webRequestParam.ToJsonString(ignoreNull: true)}");

                    throw new WebException(responseInfo.Content);
                }
            }
        }

        public static async Task<ResponseInfo> GetResponseAsync(WebRequestParam webRequestParam)
        {
            ResponseInfo responseInfo = null;

            try
            {
                HttpWebRequest request = CreateHttpRequest(webRequestParam);

                using (WebResponse response = await request.GetResponseAsync())
                {
                    responseInfo = await ReadToEndAsync(response);
                }
            }
            catch (Exception ex)
            {
                responseInfo = await ProcessExceptionAsync(webRequestParam, ex);
            }

            if (!webRequestParam.IsResponseValidJson)
            {
                return responseInfo;
            }
            else
            {
                if (webRequestParam.IsResponseValidJson && StringUtil.IsValidJson(responseInfo.Content))
                {
                    return responseInfo;
                }
                else
                {
                    var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                    logUtilService.Error($"IsValidJson=false, Request Param = {webRequestParam.ToJsonString(ignoreNull: true)}");

                    throw new WebException(responseInfo.Content);
                }
            }
        }

        private static ResponseInfo ProcessException(WebRequestParam webRequestParam, Exception ex)
        {
            var responseInfo = PreProcessException(webRequestParam, ex);

            if (ex is WebException == false)
            {
                return responseInfo;
            }

            try
            {
                using (WebResponse response = (ex as WebException).Response)
                {
                    responseInfo = ReadToEnd(response);
                    responseInfo.HttpStatusCode = (response as HttpWebResponse).StatusCode;
                }
            }
            catch (Exception getResponseEx)
            {
                throw new Exception($"purpose={webRequestParam.Purpose}, {getResponseEx}");
            }

            return responseInfo;
        }

        private static async Task<ResponseInfo> ProcessExceptionAsync(WebRequestParam webRequestParam, Exception ex)
        {
            var responseInfo = PreProcessException(webRequestParam, ex);

            if (ex is WebException == false)
            {
                return responseInfo;
            }

            try
            {
                using (WebResponse response = (ex as WebException).Response)
                {
                    responseInfo = await ReadToEndAsync(response);
                    responseInfo.HttpStatusCode = (response as HttpWebResponse).StatusCode;
                }
            }
            catch (Exception getResponseEx)
            {
                throw new Exception($"purpose={webRequestParam.Purpose}, {getResponseEx}");
            }

            return responseInfo;
        }

        private static ResponseInfo PreProcessException(WebRequestParam webRequestParam, Exception ex)
        {
            WebException webException = null;
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

            var responseInfo = new ResponseInfo()
            {
                HttpStatusCode = HttpStatusCode.ExpectationFailed
            };

            if (ex is WebException)
            {
                webException = (WebException)ex;

                if (webException.Response != null)
                {
                    responseInfo.HttpStatusCode = ((HttpWebResponse)webException.Response).StatusCode;
                }
            }

            logUtilService.Error($"HttpStatusCode = {(int)responseInfo.HttpStatusCode}. Request Param = {webRequestParam.ToJsonString(ignoreNull: true)}.");
            logUtilService.Error(ex.ToString());

            if (webException == null)
            {
                throw ex;
            }

            if (webException.Response == null)
            {
                throw new WebException($"purpose={webRequestParam.Purpose}, status={(int)responseInfo.HttpStatusCode}, {ex}");
            }

            return responseInfo;
        }

        private static HttpWebRequest CreateHttpRequest(WebRequestParam webRequestParam)
        {
            if (webRequestParam.ContentType == null)
            {
                webRequestParam.ContentType = HttpWebRequestContentType.Json;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webRequestParam.Url);
            request.Method = webRequestParam.Method.Value;
            request.ContentType = webRequestParam.ContentType.Value;

            if (webRequestParam.TimeOut.HasValue)
            {
                request.Timeout = webRequestParam.TimeOut.Value;
            }

            if (webRequestParam.Headers.AnyAndNotNull())
            {
                foreach (var header in webRequestParam.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (webRequestParam.Method != HttpMethod.Get && !webRequestParam.Body.IsNullOrEmpty())
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(webRequestParam.Body);

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return request;
        }

        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            if (httpRequest.Headers == null)
            {
                return false;
            }

            return httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private static ResponseInfo ReadToEnd(WebResponse response)
        {
            HttpWebResponse httpResponse = (HttpWebResponse)response;

            var responseInfo = new ResponseInfo()
            {
                HttpStatusCode = httpResponse.StatusCode
            };

            using (Stream stream = GetStreamByContentEncoding(response))
            {
                responseInfo.Content = DoRead(stream);
            }

            return responseInfo;
        }

        private static async Task<ResponseInfo> ReadToEndAsync(WebResponse response)
        {
            HttpWebResponse httpResponse = (HttpWebResponse)response;

            var responseInfo = new ResponseInfo()
            {
                HttpStatusCode = httpResponse.StatusCode
            };

            using (Stream stream = GetStreamByContentEncoding(response))
            {
                responseInfo.Content = await DoReadAsync(stream);
            }

            return responseInfo;
        }

        private static Stream GetStreamByContentEncoding(WebResponse response)
        {
            if (response.Headers["Content-Encoding"] == "gzip")
            {
                // 对 GZIP 格式的数据进行解压缩
                return new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                return response.GetResponseStream();
            }
        }

        private static string DoRead(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string content = reader.ReadToEnd();

                return content;
            }
        }

        private static async Task<string> DoReadAsync(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string content = await reader.ReadToEndAsync();

                return content;
            }
        }
    }

    public class HttpWebRequestContentType : BaseStringValueModel<HttpWebRequestContentType>
    {
        private HttpWebRequestContentType()
        { }

        public static HttpWebRequestContentType Json = new HttpWebRequestContentType() { Value = "application/json" };

        //※※※ 注意，"application/x-www-form-urlencoded" ，不能直接丟 body.ToJsonString 要宣告 NameValueCollection 來傳入 ※※※
        public static HttpWebRequestContentType WwwFormUrlencoded = new HttpWebRequestContentType() { Value = "application/x-www-form-urlencoded" };

        public static HttpWebRequestContentType TextPlain = new HttpWebRequestContentType() { Value = "text/plain" };
    }

    public class HttpMethod : BaseStringValueModel<HttpMethod>
    {
        private HttpMethod(string method)
        {
            Value = method;
        }

        public static HttpMethod Delete = new HttpMethod("DELETE");

        public static HttpMethod Get = new HttpMethod("GET");

        public static HttpMethod Head = new HttpMethod("HEAD");

        public static HttpMethod Options = new HttpMethod("OPTIONS");

        public static HttpMethod Patch = new HttpMethod("PATCH");

        public static HttpMethod Post = new HttpMethod("POST");

        public static HttpMethod Put = new HttpMethod("PUT");

        public static HttpMethod Trace = new HttpMethod("TRACE");
    }

    public class WebRequestParam
    {
        public string Purpose { get; set; }

        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public string Body { get; set; }

        public HttpWebRequestContentType ContentType { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public bool IsResponseValidJson { get; set; }

        public int? TimeOut { get; set; }
    }

    public class ResponseInfo
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public string Content { get; set; }
    }
}