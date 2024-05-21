using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace SLPolyGame.Web.MSSeal.Models
{
    public abstract class BaseRequest
    {
        public abstract string GetResource();

        public abstract Method GetMethod();

        public virtual bool IsSkipSignSalt() => true;

        /// <summary>
        /// 時間戳
        /// </summary>
        [JsonIgnore]
        public long Ts { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 秘鑰
        /// </summary>
        [JsonIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// 取得簽章
        /// </summary>
        /// <returns>簽章</returns>
        public string GetSign()
        {
            return ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(GetSignRaw()))).ToLower();
        }

        public string GetSignRaw()
        {
            var elements = GetType().GetProperties().OrderBy(x => x.Name).ToArray();
            var list = new List<string>();
            foreach (var element in elements)
            {
                if (IsSkipSignSalt() && string.Equals(element.Name, nameof(Salt)))
                {
                    continue;
                }

                if (element.PropertyType.IsArray)
                {
                    continue;
                }
                list.Add($"{ToCamelCase(element.Name)}={element.GetValue(this)}");
            }
            list.Add($"{ToCamelCase(nameof(Salt))}={Salt}");
            return string.Join("&", list);
        }

        public RestRequest GetRequest()
        {
            var request = new RestRequest(GetResource(), GetMethod());
            string sign = GetSign();
            string ts = Ts.ToString();

            request.AddHeader("sign", sign);
            request.AddHeader("ts", ts);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = 5000;

            if (GetMethod() == Method.Get)
            {
                var elements = GetType().GetProperties().OrderBy(x => x.Name).ToArray();
                foreach (var element in elements)
                {
                    if (string.Equals(element.Name, nameof(Salt)) || string.Equals(element.Name, nameof(Ts)))
                    {
                        continue;
                    }

                    request.AddParameter(ToCamelCase(element.Name), element.GetValue(this), ParameterType.GetOrPost);
                }
            }
            else
            {
                request.AddBody(this);
            }

            return request;
        }

        private string ToHexString(byte[] vs)
        {
            return string.Join(string.Empty, vs.Select(x => x.ToString("X2")));
        }

        /// <summary>
        /// 轉成camel
        /// </summary>
        /// <param name="text">元字串</param>
        /// <returns>camel字串</returns>
        private string ToCamelCase(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return char.ToLowerInvariant(text[0]) + text.Substring(1);
            }
            return text.ToLowerInvariant();
        }

        /// <summary>
        /// WebReqeust發起請求
        /// </summary>
        /// <param name="url">網址</param>
        /// <param name="httpStatusCode">回傳的http status</param>
        /// <param name="timeout">timeout的設定</param>
        /// <returns></returns>
        public string GetResponse(string url, out HttpStatusCode httpStatusCode, int? timeout = null)
        {
            httpStatusCode = HttpStatusCode.OK;
            string responseStr = string.Empty;
            var bodyRaw = string.Empty;
            var paramMsg = string.Empty;
            try
            {
                paramMsg = JsonConvert.SerializeObject(new
                {
                    Header = new
                    {
                        sign = GetSign(),
                        Ts = Ts.ToString()
                    },
                    Body = JsonConvert.SerializeObject(this)
                });
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Join("", url, GetResource()));
                request.Method = GetMethod().ToString();

                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }

                request.Headers.Add("sign", GetSign());
                request.Headers.Add("ts", Ts.ToString());

                bodyRaw = JsonConvert.SerializeObject(this, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.None
                });

                if (request.Method != nameof(HttpMethod.Get))
                {
                    request.ContentType = "application/json";
                    using (var reqStream = new StreamWriter(request.GetRequestStream()))
                    {
                        reqStream.Write(bodyRaw);
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebResponse response = request.GetResponse())
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    httpStatusCode = httpResponse.StatusCode;

                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = reader.ReadToEnd();
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

                if (webException == null)
                {
                    throw new Exception(paramMsg, ex);
                }

                if (webException.Response == null)
                {
                    throw new WebException($"status={(int)httpStatusCode}, param:{paramMsg}", ex);
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
                    throw new Exception(paramMsg, getResponseEx);
                }
            }

            return responseStr;
        }
    }
}