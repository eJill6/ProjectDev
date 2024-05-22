using Newtonsoft.Json;
using System.Text;

namespace FakeMSSeal.Middleware
{
    public class CustomLoggerMiddleware
    {

        private readonly RequestDelegate _next = null;

        private readonly ILogger<CustomLoggerMiddleware> _logger = null;

        public CustomLoggerMiddleware(RequestDelegate next, ILogger<CustomLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (string.Equals(context.Request.Method, nameof(HttpMethod.Post),
                    StringComparison.OrdinalIgnoreCase) &&
                context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var textBody = Encoding.UTF8.GetString(buffer);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation($"Header:{JsonConvert.SerializeObject(context.Request.Headers)}, Body:{textBody}");
            }
            else if (string.Equals(context.Request.Method, nameof(HttpMethod.Get),
                StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation($"Header:{JsonConvert.SerializeObject(context.Request.Headers)}, QueryString:{JsonConvert.SerializeObject(context.Request.QueryString)}");
            }
            await _next.Invoke(context);
        }
    }



    public static class CustomLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomLogger(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomLoggerMiddleware>();
        }
    }
}
