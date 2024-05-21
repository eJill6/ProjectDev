using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using static System.Net.WebRequestMethods;

namespace MS.Core.Helpers.RestRequestHelpers
{
    public class RestRequestHelper
    {
        RestClient client = null;
        RestRequest http = null;

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
        public RestRequestHelper Get(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.GET, action);
            return this;
        }
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RestRequestHelper Put(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.PUT, action);
            return this;
        }
        public RestRequestHelper Post(Action<RestRequestParameterHelper> action = null)
        {
            SetHttpRequest(Method.POST, action);
            return this;
        }
        private void SetHttpRequest(Method method, Action<RestRequestParameterHelper> action = null)
        {
            http = new RestRequest(method);

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
        public async Task<RestResponseModel<T>> ResponseAsync<T>(int timeout = 5000)
        {
            T? result = default;

            IRestResponse response = null;
            
            client.Timeout = timeout;
            
            RestRequestModel request = new RestRequestModel
            {
                BaseUrl = client.BaseUrl,
                Parameters = http.Parameters,
                Method = http.Method.ToString()
            };
            try
            {
                response = await client.ExecuteAsync(http);
                
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
            catch(Exception e) 
            {
                return new RestResponseModel<T>
                {
                    Response = response,
                    Result = result,
                    Request = request,
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
            
        }
    }

}
