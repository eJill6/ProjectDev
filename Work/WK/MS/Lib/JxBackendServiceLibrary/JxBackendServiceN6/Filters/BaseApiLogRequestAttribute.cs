using Castle.Core.Internal;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.Param.Filter;
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
        private readonly Lazy<ILogUtilService> _logUtilService;

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
            EnvironmentUser environmentUser = GetEnvironmentUser(actionContext.HttpContext);
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentUser.Application, SharedAppSettings.PlatformMerchant).Value;

            if (!appSettingService.IsEnabledMethodInvocationLog)
            {
                await base.OnActionExecutionAsync(actionContext, next);

                return;
            }

            var apiLogRequestHttpContextItem = new ApiLogRequestHttpContextItem();
            actionContext.HttpContext.SetItemValue(HttpContextItemKey.ApiLogRequestItem, apiLogRequestHttpContextItem);

            try
            {                
                apiLogRequestHttpContextItem.Url = actionContext.HttpContext.Request.GetEncodedUrl();
                apiLogRequestHttpContextItem.CorrelationId = GetOrCreateCorrelationId(actionContext.ActionArguments);
                
                HttpRequest request = actionContext.HttpContext.Request;

                if (request.Method == JxBackendService.Common.Util.HttpMethod.Post)
                {
                    request.Body.Position = 0;

                    using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                    {
                        apiLogRequestHttpContextItem.RawRequest = await reader.ReadToEndAsync();                         
                    }
                }

                _logUtilService.Value.ForcedDebug($"LogRequest id={apiLogRequestHttpContextItem.CorrelationId} " +
                    $"url={apiLogRequestHttpContextItem.Url} rawRequest = {apiLogRequestHttpContextItem.RawRequest} ");
            }
            catch (Exception ex)
            {
                _logUtilService.Value.Error(ex);
            }

            apiLogRequestHttpContextItem.Stopwatch = new Stopwatch();            
            apiLogRequestHttpContextItem.Stopwatch.Start();

            await base.OnActionExecutionAsync(actionContext, next);
        }

        private static string GetOrCreateCorrelationId(IDictionary<string, object> actionArguments)
        {
            string correlationId = null;

            foreach (KeyValuePair<string, object> argument in actionArguments)
            {
                if (argument.Value != null && argument.Value is IInvocationParam)
                {
                    correlationId = (argument.Value as IInvocationParam).CorrelationId;

                    break;
                }
            }

            if (correlationId.IsNullOrEmpty())
            {
                correlationId = Guid.NewGuid().ToString();
            }

            return correlationId;
        }

        /// <summary>
        /// 記錄response
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            EnvironmentUser environmentUser = GetEnvironmentUser(actionExecutedContext.HttpContext);
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentUser.Application, SharedAppSettings.PlatformMerchant).Value;

            if (!appSettingService.IsEnabledMethodInvocationLog)
            {
                return;
            }

            var apiLogRequestHttpContextItem = actionExecutedContext.HttpContext
                .GetItemValue<ApiLogRequestHttpContextItem>(HttpContextItemKey.ApiLogRequestItem);
            
            apiLogRequestHttpContextItem.Stopwatch.Stop();

            string? response = (actionExecutedContext.Result as ObjectResult)?.Value
                .ToJsonString(ignoreNull: true, isFormattingNone: true);

            _logUtilService.Value.ForcedDebug($"response id={apiLogRequestHttpContextItem.CorrelationId} content={response}");

            if (_isLogToDB)
            {
                LogToDB(actionExecutedContext, response, apiLogRequestHttpContextItem);
            }
        }

        private void LogToDB(ActionExecutedContext actionExecutedContext, string? response, ApiLogRequestHttpContextItem apiLogRequestItem)
        {
            EnvironmentUser environmentUser = GetEnvironmentUser(actionExecutedContext.HttpContext);

            var methodInvocationLogReadService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogReadService>(
                environmentUser,
                DbConnectionTypes.Slave).Value;

            IInsertMethodInvocationLogParam insertMethodInvocationLogParam = new InsertMethodInvocationLogParam()
            {
                Arguments = new { apiLogRequestItem.Url, apiLogRequestItem.RawRequest },
                CorrelationId = apiLogRequestItem.CorrelationId,
                ElapsedMilliseconds = apiLogRequestItem.Stopwatch.ElapsedMilliseconds,
                MethodName = actionExecutedContext.HttpContext.Request.GetEncodedPathAndQuery(),
                ReturnValue = response,
                UserID = GetAffectedUserID(actionExecutedContext.HttpContext),
                TypeName = GetType().Name,
                CreateDate = DateTime.Now,
                ErrorMsg = string.Empty,
            };

            methodInvocationLogReadService.Enqueue(insertMethodInvocationLogParam);
        }
    }
}