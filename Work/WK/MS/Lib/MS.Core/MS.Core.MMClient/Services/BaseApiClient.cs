using Newtonsoft.Json;
using SportGame.Client.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MS.Core.MMClient.Models;
using System.Web;
using RestSharp.Authenticators;
using System.IO;

namespace MS.Core.MMClient.Services
{
    public abstract class BaseApiClient
    {
        /// <summary>
        /// Service Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Header 資訊
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// TimeOut時間
        /// </summary>
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);

        public string BaseUrl { get; set; }

        protected async Task<T> GetJsonAsync<T>(string url, IDictionary<string, string> headers = null, TimeSpan? timeout = null)
        {
            var responesText = await GetJsonAsync(url, headers, timeout);

            return JsonConvert.DeserializeObject<T>(responesText);
        }

        protected async Task<string> GetJsonAsync(string url, IDictionary<string, string> headers = null, TimeSpan? timeout = null)
        {
            var req = new RestRequest(Method.GET);
            var res = await GetResponseAsync(url, req, headers, timeout);

            return res.Content;
        }

        protected async Task<T> PostJsonAsync<T>(string url, IDictionary<string, string> headers = null, object requestBody = null, TimeSpan? timeout = null)
        {
            var responesText = await PostJsonAsync(url, headers, requestBody, timeout);

            return JsonConvert.DeserializeObject<T>(responesText);
        }

        protected async Task<string> PostJsonAsync(string url, IDictionary<string, string> headers = null, object requestBody = null, TimeSpan? timeout = null)
        {
            var req = new RestRequest(Method.POST);
            req.AddHeader("Content-type", "application/json");
            req.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);

            var res = await GetResponseAsync(url, req, headers, timeout);

            return res.Content;
        }

        protected async Task<T> PostFormDataAsync<T>(string url, IDictionary<string, string> headers = null, object requestBody = null, TimeSpan? timeout = null)
        {
            var responesText = await PostFormDataAsync(url, headers, requestBody, timeout);

            return JsonConvert.DeserializeObject<T>(responesText);
        }

        protected async Task<string> PostFormDataAsync(string url, IDictionary<string, string> headers = null, object requestBody = null, TimeSpan? timeout = null)
        {
            const string contentType = "application/x-www-form-urlencoded";
            var dic = ObjectUtil.ToDictionary(requestBody);
            var req = new RestRequest(Method.POST);
            var forms = dic.Select(f => $"{f.Key}={HttpUtility.UrlEncode(f.Value)}");

            req.AddHeader("content-type", contentType);
            req.AddParameter(contentType, string.Join("& ", forms));

            var res = await GetResponseAsync(url, req, headers, timeout);

            return res.Content;
        }

        protected virtual IRestClient GetDefalutRestClient()
        {
            var client = new RestClient();

            if (!string.IsNullOrEmpty(Token))
            {
                client.Authenticator = new JwtAuthenticator(Token);
            }

            if (Headers?.Any() == true)
            {
                AddHeader(client, Headers);
            }

            return client;
        }

        protected void AddHeader(IRestClient client, IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                client.AddDefaultHeader(header.Key, header.Value);
            }
        }

        protected string UrlCombine(params string[] path)
        {
            return Path.Combine(path).Replace("\\", "/");
        }

        protected string GetUrl(string controllerName, string actionPath)
        {
            return UrlCombine(BaseUrl, controllerName, actionPath);
        }

        protected string GetUrlRoute(string controllerName, string actionPath, string optional)
        {
            return UrlCombine(BaseUrl, controllerName, actionPath, optional);
        }

        private async Task<IRestResponse> GetResponseAsync(string url, IRestRequest request, IDictionary<string, string> headers = null, TimeSpan? timeOut = null)
        {
            var client = GetDefalutRestClient();
            client.Timeout = (int)(timeOut.HasValue ? timeOut.Value.TotalMilliseconds : TimeOut.TotalMilliseconds);
            var traceId = Guid.NewGuid().ToString();
            client.BaseUrl = new Uri(url);

            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }
            headers.Add("traceId", traceId);

            AddHeader(client, headers);

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                throw new ClientException(JsonConvert.SerializeObject(new ClientErrorMessage
                {
                    Url = url,
                    TraceId = traceId,
                    RequestHeaders = ConvertToHeaderParameters(headers)
                }), response.ErrorException);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ClientException(JsonConvert.SerializeObject(new ClientErrorMessage
                {
                    Url = response.ResponseUri.ToString(),
                    TraceId = traceId,
                    StatusCode = response.StatusCode,
                    Response = response.Content,
                    RequestHeaders = response.Headers
                }));
            }

            return response;
        }

        private IList<Parameter> ConvertToHeaderParameters(IDictionary<string, string> headers)
        {
            IList<Parameter> result;
            result = headers?.Select(s => new Parameter(s.Key, s.Value, ParameterType.HttpHeader)).ToList();
            return result;
        }
    }
}