﻿using JxBackendService.Common.Extensions;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

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
            string responseStr = string.Empty;

            try
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

                using (WebResponse response = request.GetResponse())
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    httpStatusCode = httpResponse.StatusCode;

                    if (response.Headers["Content-Encoding"] == "gzip")
                    {
                        // 对 GZIP 格式的数据进行解压缩
                        using (GZipStream gzipStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(gzipStream))
                            {
                                responseStr = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        // 对非 GZIP 格式的数据进行读取
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            responseStr = reader.ReadToEnd();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WebException webException = null;
                httpStatusCode = HttpStatusCode.ExpectationFailed;

                if (ex is WebException)
                {
                    webException = (WebException)ex;

                    if (webException.Response != null)
                    {
                        httpStatusCode = ((HttpWebResponse)webException.Response).StatusCode;
                    }
                }

                LogUtil.Error($"HttpStatusCode = {(int)httpStatusCode}. Request Param = {webRequestParam.ToJsonString(ignoreNull: true)}.");
                LogUtil.Error(ex.ToString());

                if (webException == null)
                {
                    throw ex;
                }

                if (webException.Response == null)
                {
                    throw new WebException($"purpose={webRequestParam.Purpose}, status={(int)httpStatusCode}, {ex}");
                }

                try
                {
                    using (WebResponse response = webException.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        httpStatusCode = httpResponse.StatusCode;

                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseStr = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception getResponseEx)
                {
                    throw new Exception($"purpose={webRequestParam.Purpose}, {getResponseEx}");
                }
            }

            if (!webRequestParam.IsResponseValidJson)
            {
                return responseStr;
            }
            else
            {
                if (webRequestParam.IsResponseValidJson && StringUtil.IsValidJson(responseStr))
                {
                    return responseStr;
                }
                else
                {
                    LogUtil.Error($"IsValidJson=false, Request Param = {webRequestParam.ToJsonString(ignoreNull: true)}");

                    throw new WebException(responseStr);
                }
            }
        }

        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                return false;
            }

            return (httpRequest["X-Requested-With"] == "XMLHttpRequest") ||
                ((httpRequest.Headers != null) &&
                (httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        //public static bool IsAjaxRequest(HttpContext httpContext)
        //{
        //    if (httpContext == null)
        //    {
        //        return false;
        //    }

        //    bool isAjax = httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        //    return isAjax;
        //}

        //public static bool IsHttpFileExist(string fileUrl)
        //{
        //    int retryCount = GlobalVariables.JenkinsAppReleaseTimeoutSeconds;

        //    for (int index = 1; index <= retryCount; index++)
        //    {
        //        Thread.Sleep(1000);

        //        try
        //        {
        //            //建立根據網路地址的請求物件
        //            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(fileUrl));
        //            httpWebRequest.Method = HttpMethod.Head.ToString();
        //            httpWebRequest.Timeout = 1000;
        //            //返回響應狀態是否是成功比較的布林值
        //            using (HttpWebResponse resp = (HttpWebResponse)httpWebRequest.GetResponse())
        //            {
        //                LogUtil.Error($"index={index},resp.StatusCode={resp.StatusCode},fileUrl={fileUrl}");
        //                if (resp.StatusCode == HttpStatusCode.OK)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (index == retryCount)
        //            {
        //                LogUtil.Error(ex);
        //                return false;
        //            }
        //            continue;
        //        }
        //    }

        //    return false;
        //}
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

        public static HttpMethod Delete = new HttpMethod(nameof(Delete));

        public static HttpMethod Get = new HttpMethod(nameof(Get));

        public static HttpMethod Head = new HttpMethod(nameof(Head));

        public static HttpMethod Options = new HttpMethod(nameof(Options));

        public static HttpMethod Patch = new HttpMethod(nameof(Patch));

        public static HttpMethod Post = new HttpMethod(nameof(Post));

        public static HttpMethod Put = new HttpMethod(nameof(Put));

        public static HttpMethod Trace = new HttpMethod(nameof(Trace));
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
}