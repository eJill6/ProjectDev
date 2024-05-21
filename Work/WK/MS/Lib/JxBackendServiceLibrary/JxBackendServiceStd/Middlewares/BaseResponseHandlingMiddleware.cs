using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Middlewares
{
    public abstract class BaseResponseHandlingMiddleware<ResponseModel> : BaseMiddleware
    {
        protected abstract ResponseModel ConvertToResponseModel(string errorMsg);

        protected virtual HttpStatusCode ConvertHttpStatusCode(HttpStatusCode httpStatusCode) => httpStatusCode;

        protected abstract Endpoint GetEndpoint(HttpContext httpContext);

        private async Task WriteResponseContentAsync(HttpResponse httpResponse, string responseJson)
        {
            await httpResponse.WriteAsync(responseJson);
        }

        public BaseResponseHandlingMiddleware(RequestDelegate next) : base(next)
        {
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            HttpResponse response = httpContext.Response;
            Stream originalStream = response.Body;

            using (var readableBodyStream = new MemoryStream())
            {
                response.Body = readableBodyStream;

                try
                {
                    await Next(httpContext);
                }
                catch (HttpStatusException httpStatusException)
                {
                    httpContext.Response.StatusCode = (int)httpStatusException.HttpStatusCode;

                    if (httpStatusException.HttpStatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw httpStatusException;
                    }
                }
                finally
                {
                    response.Body = originalStream;
                    string responseJson;

                    switch (response.StatusCode)
                    {
                        case (int)HttpStatusCode.BadRequest:

                            ValidationError validationError = await GetResponseModelAsync<ValidationError>(readableBodyStream);

                            //api controller攔截的才會有
                            if (validationError == null || !validationError.Errors.AnyAndNotNull())
                            {
                                readableBodyStream.Seek(0, SeekOrigin.Begin);
                                await readableBodyStream.CopyToAsync(originalStream);

                                break;
                            }

                            string errorMsg = validationError.Errors.First().Value.First();
                            responseJson = ConvertToResponseModel(errorMsg).ToJsonString(isCamelCaseNaming: true);
                            response.StatusCode = (int)ConvertHttpStatusCode((HttpStatusCode)response.StatusCode);

                            await WriteResponseContentAsync(response, responseJson);

                            break;

                        case (int)HttpStatusCode.Unauthorized:

                            Endpoint endpoint = GetEndpoint(httpContext);
                            bool hasApiController = false;

                            if (endpoint != null)
                            {
                                hasApiController = endpoint.HasApiController();
                            }

                            if (httpContext.Request.IsAjaxRequest() || hasApiController)
                            {
                                response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";
                                responseJson = ConvertToResponseModel(HttpStatusCode.Unauthorized.ToString()).ToJsonString(isCamelCaseNaming: true);

                                await WriteResponseContentAsync(response, responseJson);
                            }
                            else
                            {
                                httpContext.Response.Redirect("/ReconnectTips");
                            }

                            break;

                        default:
                            readableBodyStream.Seek(0, SeekOrigin.Begin);
                            await readableBodyStream.CopyToAsync(originalStream);

                            break;
                    }
                }
            }
        }

        private async Task<T> GetResponseModelAsync<T>(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (var responseReader = new StreamReader(memoryStream, Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 1024,
                leaveOpen: true))
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
}