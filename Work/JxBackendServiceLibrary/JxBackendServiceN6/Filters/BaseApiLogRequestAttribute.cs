using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Filters.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text;

namespace JxBackendServiceN6.Filters
{
    public abstract class BaseApiLogRequestAttribute : BaseApiFilterAttribute
    {
        private string _correlationId;

        private readonly ILogUtilService _logUtilService;

        private Stopwatch _stopwatch;

        private string _url;

        private string _rawRequest;

        private readonly bool _isLogToDB;

        protected abstract int GetAffectedUserID(HttpContext httpContext);

        public BaseApiLogRequestAttribute(bool isLogToDB = false)
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _isLogToDB = isLogToDB;
        }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            try
            {
                _url = actionContext.HttpContext.Request.GetEncodedUrl();
                _correlationId = Guid.NewGuid().ToString();
                actionContext.HttpContext.SetItemValue(HttpContextItemKey.CorrelationId, _correlationId);
                HttpRequest request = actionContext.HttpContext.Request;

                if (request.Method == JxBackendService.Common.Util.HttpMethod.Post)
                {
                    request.Body.Position = 0;

                    using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                    {
                        _rawRequest = await reader.ReadToEndAsync();
                    }
                }

                _logUtilService.ForcedDebug($"LogRequest id={_correlationId} url={_url} rawRequest = {_rawRequest} ");
            }
            catch (Exception ex)
            {
                _logUtilService.Error(ex);
            }

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            await base.OnActionExecutionAsync(actionContext, next);
        }

        /// <summary>
        /// 記錄response
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            _stopwatch.Stop();

            string? response = (actionExecutedContext.Result as ObjectResult)?.Value
                .ToJsonString(ignoreNull: true, isFormattingNone: true);

            _logUtilService.ForcedDebug($"response id={_correlationId} content={response}");

            if (_isLogToDB)
            {
                LogToDB(actionExecutedContext, response);
            }
        }

        private void LogToDB(ActionExecutedContext actionExecutedContext, string? response)
        {
            EnvironmentUser environmentUser = GetEnvironmentUser(actionExecutedContext.HttpContext);

            var methodInvocationLogService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogService>(
                environmentUser,
                DbConnectionTypes.Master);

            IInsertMethodInvocationLogParam insertMethodInvocationLogParam = new InsertMethodInvocationLogParam()
            {
                Arguments = new { url = _url, rawRequest = _rawRequest },
                CorrelationId = _correlationId,
                ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds,
                MethodName = actionExecutedContext.HttpContext.Request.GetEncodedPathAndQuery(),
                ReturnValue = response,
                UserID = GetAffectedUserID(actionExecutedContext.HttpContext)
            };

            JxTask.Run(environmentUser, () =>
            {
                methodInvocationLogService.Create(insertMethodInvocationLogParam);
            });
        }
    }
}