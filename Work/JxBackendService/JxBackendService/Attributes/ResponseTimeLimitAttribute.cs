using System;
using System.Threading;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    /// <summary>
    /// 延遲回應client的filter
    /// </summary>
    public class ResponseTimeLimitAttribute : ActionFilterAttribute
    {
        private readonly int waitMilliSeconds;

        public ResponseTimeLimitAttribute(int waitSecond = 1)
        {
            waitMilliSeconds = waitSecond * 1000;
        }

        /// <summary>
        /// 執行 Action 之後執行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (waitMilliSeconds > 0)
            {
                Thread.Sleep(waitMilliSeconds);
            }
        }
    }
}

