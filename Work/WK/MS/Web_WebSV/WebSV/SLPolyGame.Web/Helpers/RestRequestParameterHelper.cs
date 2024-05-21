using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Helpers
{
    public class RestRequestParameterHelper
    {
        RestRequest http = null;
        public RestRequestParameterHelper(RestRequest http)
        {
            this.http = http;
        }
        #region Header設定

        public RestRequestParameterHelper AddHeader(string key, string value)
        {
            http.AddHeader(key, value);

            return this;
        }
        public RestRequestParameterHelper AddHeader(Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                http.AddHeaders(headers);
            }

            return this;
        }
        #endregion
        #region Parameter設定

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(string key, object value)
        {
            switch (http.Method)
            {
                case Method.Get:
                    {
                        http.AddQueryParameter(key, value.ToString());
                    }
                    break;
                default:
                    {
                        http.AddParameter(key, value.ToString());
                    }
                    break;
            }

            return this;
        }
        /// <summary>
        /// TextPlain
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddTextPlainBody(string body)
        {
            http.AddHeader("Content-Type", "text/plain");
            http.AddParameter("text/plain", body, ParameterType.RequestBody);
            return this;
        }

        public RestRequestParameterHelper AddParameter(object body)
        {
            if (body == null)
            {
                return this;
            }
            switch (http.Method)
            {
                case Method.Get:
                    {
                        foreach (PropertyInfo item in body.GetType().GetProperties())
                        {
                            object value = item.GetValue(body);
                            http.AddQueryParameter(item.Name, value?.ToString() ?? string.Empty);
                        }
                    }
                    break;
                default:
                    {
                        http.AddJsonBody(body);
                    }
                    break;
            }

            return this;
        }

        /// <summary>
        /// key,value設定參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequestParameterHelper AddParameter(Dictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    AddParameter(item.Key, item.Value);
                }
            }

            return this;
        }

        #endregion
    }

    public class RestRequestHelper
    {
        RestClient client = null;
        RestRequest http = null;

        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authenticator">HttpBasicAuthenticator</param>
        public RestRequestHelper(string url, IAuthenticator authenticator = null)
        {
            client = new RestClient(url);
            
            if (authenticator != null)
            {
                client.Authenticator = authenticator;
            }
        }
        public static RestRequestHelper Request(string url)
        {
            RestRequestHelper helper = new RestRequestHelper(url);
            return helper;
        }
        public static RestRequestHelper Request(string url, string username, string password)
        {
            RestRequestHelper helper = new RestRequestHelper(url, new HttpBasicAuthenticator(username, password));
            return helper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="authenticator">HttpBasicAuthenticator</param>
        /// <returns></returns>
        public static RestRequestHelper Request(string baseUrl, string apiUrl)
        {
            RestRequestHelper helper = new RestRequestHelper($"{baseUrl}{apiUrl}", null);
            return helper;
        }

        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RestRequestHelper Get(Action<RestRequestParameterHelper> action = null, TimeSpan? timeOut = null)
        {
            SetHttpRequest(Method.Get, action, timeOut);
            return this;
        }
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RestRequestHelper Put(Action<RestRequestParameterHelper> action = null, TimeSpan? timeOut = null)
        {
            SetHttpRequest(Method.Put, action, timeOut);
            return this;
        }
        public RestRequestHelper Post(Action<RestRequestParameterHelper> action = null, TimeSpan? timeOut = null)
        {
            SetHttpRequest(Method.Post, action, timeOut);
            return this;
        }
        private void SetHttpRequest(Method method, Action<RestRequestParameterHelper> action = null, TimeSpan? timeOut = null)
        {
            http = new RestRequest(string.Empty, method);
            http.Timeout = (int)(timeOut.HasValue ? timeOut.Value.TotalMilliseconds : TimeOut.TotalMilliseconds);
            if (action != null)
            {
                action(new RestRequestParameterHelper(http));
            }
        }

        /// <summary>
        /// 自訂回傳類型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<RestResponseModel<T>> ResponseAsync<T>()
        {
            T result = default;
            RestResponse response = null;
            RestRequestModel request = new RestRequestModel
            {
                BaseUrl = client.BuildUri(http),
                Parameters = http.Parameters.ToList(),
                Method = http.Method.ToString()
            };
            try
            {
                response = await client.ExecuteAsync(http);

                if(response.IsSuccessful == false)
                {
                    return new RestResponseModel<T>
                    {
                        Response = response,
                        Request = request,
                        IsSuccess = false,
                    };
                }

                if (typeof(T) == typeof(string))
                {
                    result = (T)(object)response.Content;
                }
                else
                {
                    result = JsonConvert.DeserializeObject<T>(response.Content);
                }
                return new RestResponseModel<T>
                {
                    Response = response,
                    Result = result,
                    Request = request,
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new RestResponseModel<T>
                {
                    Response = response,
                    Result = result,
                    Request = request,
                    IsSuccess = false,
                    Exception = e,
                };
            }

        }
    }
    public class RestResponseModel<T>
    {
        public RestResponse Response { get; set; }

        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public RestRequestModel Request { get; set; }
    }
    public class RestRequestModel
    {
        public Uri BaseUrl { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public string Method { get; set; }
    }
}