using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Middlewares;
using System.Text;

namespace M.Core.Middlewares
{
    public class InvokeOuterEncryptMobileApiMiddleware : BaseMiddleware
    {
        private static readonly string s_host = "http://192.168.104.41:10500";

        public InvokeOuterEncryptMobileApiMiddleware(RequestDelegate next) : base(next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string encPath = XorEncryptTool.XorEncryptToString(context.Request.Path);

            HttpResponseMessage response = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("enc-bytes", "true");
                httpClient.DefaultRequestHeaders.Add("enc-path", encPath);

                if (context.Request.Method == System.Net.Http.HttpMethod.Get.ToString())
                {
                    response = await httpClient.GetAsync(s_host);
                }
                else if (context.Request.Method == System.Net.Http.HttpMethod.Post.ToString())
                {
                    string requestBody = await GetBody(context.Request);
                    string xorBody = XorEncryptTool.XorEncryptToString(requestBody);
                    var content = new StringContent(xorBody, Encoding.UTF8, "application/json");

                    response = await httpClient.PostAsync(s_host, content);
                }
            }

            if (response.IsSuccessStatusCode)
            {
                context.Response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";

                string xorResponseContent = await response.Content.ReadAsStringAsync();
                string responseContent = XorEncryptTool.XorDecryptToString(xorResponseContent);

                object responseObj = responseContent.Deserialize<object>();
                string newResponse = new { host = s_host, response = responseObj }.ToJsonString(isCamelCaseNaming: true);

                await context.Response.WriteAsync(newResponse);
            }
            else
            {
                context.Response.StatusCode = (int)response.StatusCode;
            }
        }

        private async Task<string> GetBody(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}