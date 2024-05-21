using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MMModel.Models;
using Newtonsoft.Json;

namespace MMService.Attributes
{
    /// <summary>
    /// 錯誤集中處理
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate Next;

        /// <inheritdoc/>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // 调用下一个中间件或处理程序
                await Next(context);
            }
            catch (MMException ex)
            {
                context.Response.StatusCode = 200;

                context.Response.ContentType = "application/json";

                var response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = ex.Code
                };

                var jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                // 设置响应内容
                context.Response.ContentType = "application/json";
                var errorMessage = $"An error occurred.{ex.Message}";
                await context.Response.WriteAsync(errorMessage);
            }
        }
    }
}
