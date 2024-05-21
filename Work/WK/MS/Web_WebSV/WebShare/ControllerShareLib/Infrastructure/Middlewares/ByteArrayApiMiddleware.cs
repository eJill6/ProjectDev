using System.Net;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Middlewares;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Http;

namespace ControllerShareLib.Infrastructure.Middlewares
{
    public class ByteArrayApiMiddleware : BaseMiddleware
    {
        private readonly Lazy<IByteArrayApiService> _byteArrayApiService;

        public ByteArrayApiMiddleware(RequestDelegate next) : base(next)
        {
            _byteArrayApiService = DependencyUtil.ResolveService<IByteArrayApiService>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!_byteArrayApiService.Value.IsEncodingRequired(httpContext.Request))
            {
                if (!_byteArrayApiService.Value.CheckAllowUnencryptedRequest(httpContext.Request))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    
                    return;
                }

                await Next(httpContext);

                return;
            }

            //IsEncodingPathFromQueryString必須在DecodeEncodingPath之前執行，因為DecodeEncodingPath中會覆寫QueryString的關係，故不要異動順序
            bool isEncodingPathFromQueryString = _byteArrayApiService.Value.IsEncodingPathFromQueryString(httpContext.Request);
            _byteArrayApiService.Value.DecodeEncodingPath(httpContext.Request);

            if (isEncodingPathFromQueryString)
            {
                await Next(httpContext);
                
                return;
            }

            await _byteArrayApiService.Value.DecodeToBodyAsync(httpContext.Request);

            HttpResponse response = httpContext.Response;
            Stream originalStream = response.Body;

            using (var readableBodyStream = new MemoryStream())
            {
                response.Body = readableBodyStream;

                try
                {
                    await Next(httpContext);
                }
                finally
                {
                    response.Body = originalStream;
                    byte[] bytes = readableBodyStream.ToArray();

                    await _byteArrayApiService.Value.EncodeResponseAsync(response, bytes, httpContext.Request.Headers);
                }
            }
        }
    }
}