using JxBackendService.Common.Util;
using JxBackendService.Model.MiseLive.Response;
using SLPolyGame.Web.Filters.Base;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SLPolyGame.Web.Filters
{
    /// <summary>
    /// ApiExceptionFilter
    /// </summary>
    public class ApiExceptionFilterAttribute : BaseApiFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// ExecuteExceptionFilterAsync
        /// </summary>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            ErrorMsgUtil.ErrorHandle(actionExecutedContext.Exception, EnvironmentUser);

            var errorResponse = new BaseMiseLiveResponse()
            {
                Success = false,
                Error = actionExecutedContext.Exception.Message
            };

            actionExecutedContext.Response = new HttpResponseMessage
            {
                Content = new StringContent(errorResponse.ToJsonString(isCamelCaseNaming: true))
            };

            return Task.CompletedTask;
        }
    }
}