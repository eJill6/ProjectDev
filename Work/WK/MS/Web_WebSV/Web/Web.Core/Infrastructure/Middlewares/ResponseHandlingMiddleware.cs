using JxBackendService.Common.Util;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.ReturnModel;
using System.Net;

namespace Web.Core.Infrastructure.Middlewares
{
    public class ResponseHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            HttpRequest request = httpContext.Request;
            HttpResponse response = httpContext.Response;
            Stream originalStream = response.Body;

            using var readableBodyStream = new MemoryStream();
            response.Body = readableBodyStream;

            try
            {
                await _next(httpContext);
            }
            catch (HttpStatusException httpStatusException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            finally
            {
                response.Body = originalStream;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    string responseJson = new BaseReturnModel(HttpStatusCode.Unauthorized.ToString()).ToJsonString(isCamelCaseNaming: true);
                    await response.WriteAsync(responseJson);
                }
                else
                {
                    readableBodyStream.Seek(0, SeekOrigin.Begin);
                    await readableBodyStream.CopyToAsync(originalStream);
                }
            }
        }

        private async Task<T?> GetResponseModelAsync<T>(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (var responseReader = new StreamReader(memoryStream, leaveOpen: true))
            {
                string responseBody = await responseReader.ReadToEndAsync();

                if (responseBody.IsNullOrEmpty())
                {
                    return default;
                }

                return responseBody.Deserialize<T>();
            }
        }
    }

    public class ValidationError
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

        public Dictionary<string, string[]> Errors { get; set; }
    }
}