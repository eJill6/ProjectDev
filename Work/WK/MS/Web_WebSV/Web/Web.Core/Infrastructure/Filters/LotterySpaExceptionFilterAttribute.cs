using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Core.Infrastructure.Filters
{
    public class LotterySpaExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        protected readonly Lazy<ILogger<LotterySpaExceptionFilterAttribute>> _logger;

        public LotterySpaExceptionFilterAttribute()
        {
            _logger = DependencyUtil.ResolveService<ILogger<LotterySpaExceptionFilterAttribute>>();
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            _logger.Value.LogError(exceptionContext.Exception, exceptionContext.Exception.Message);

            if (exceptionContext.HttpContext.Request.IsAjaxRequest())
            {
                exceptionContext.Result = LotterySpaUtil.CreateJsonResult(errorMessage: "应用程序发生错误，请重新登录或稍候重试！");
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = "Error"
                };

                exceptionContext.Result = result;
            }

            exceptionContext.ExceptionHandled = true;
        }
    }
}