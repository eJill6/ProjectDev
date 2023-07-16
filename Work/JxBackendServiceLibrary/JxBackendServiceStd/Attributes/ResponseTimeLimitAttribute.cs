using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;

namespace JxBackendService.Attributes
{
    /// <summary>
    /// 延遲回應client的filter
    /// </summary>
    public class ResponseTimeLimitAttribute : ActionFilterAttribute
    {
        private readonly int _waitMilliSeconds;

        public ResponseTimeLimitAttribute(int waitMilliSeconds)
        {
            _waitMilliSeconds = waitMilliSeconds;
        }

        /// <summary>
        /// 執行 Action 之後執行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (_waitMilliSeconds > 0)
            {
                Thread.Sleep(_waitMilliSeconds);
            }
        }
    }
}