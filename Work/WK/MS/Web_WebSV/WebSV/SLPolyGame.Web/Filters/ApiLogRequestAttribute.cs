using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.MiseLive.Response;
using Newtonsoft.Json;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Extensions;
using SLPolyGame.Web.Filters.Base;
using SLPolyGame.Web.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

namespace SLPolyGame.Web.Filters
{
    /// <summary>
    /// ApiLogRequestAttribute
    /// </summary>
    public class ApiLogRequestAttribute : BaseApiFilterAttribute
    {
        private string _correlationId;

        private readonly ILogUtilService _logUtilService;

        private Stopwatch _stopwatch;

        private string _url;

        private string _rawRequest;

        private string _returnValueJson;

        private readonly bool _isLogToDB;

        public ApiLogRequestAttribute(bool isLogToDB = false)
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _isLogToDB = isLogToDB;
        }

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                base.OnActionExecuting(actionContext);

                _url = actionContext.Request.RequestUri.ToString();
                _correlationId = actionContext.Request.GetCorrelationId().ToString();
                actionContext.ActionArguments["CorrelationId"] = _correlationId;

                using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
                {
                    stream.BaseStream.Position = 0;
                    _rawRequest = stream.ReadToEnd();
                }

                _logUtilService.ForcedDebug($"LogRequest id={_correlationId} url={_url} rawRequest = {_rawRequest} ");
            }
            catch (Exception ex)
            {
                _logUtilService.Error(ex);
            }

            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        /// <summary>
        /// 記錄response
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _stopwatch.Stop();

            string response = actionExecutedContext.Response.Content.ToJsonString(isCamelCaseNaming: true);
            ResponseContent responseContent = response.Deserialize<ResponseContent>();
            _logUtilService.ForcedDebug($"response id={_correlationId} content={ JsonConvert.SerializeObject(responseContent.Value, Formatting.None)}");

            if (_isLogToDB)
            {
                LogToDB(actionExecutedContext);
            }
        }

        private void LogToDB(HttpActionExecutedContext actionExecutedContext)
        {
            EnvironmentUser environmentUser = GetEnvironmentUser(actionExecutedContext.Request);

            var methodInvocationLogService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogService>(
                environmentUser,
                DbConnectionTypes.Master);

            IHttpRouteData routeData = actionExecutedContext.Request.GetRequestContext().RouteData;

            IInsertMethodInvocationLogParam insertMethodInvocationLogParam = new InsertMethodInvocationLogParam()
            {
                Arguments = new { url = _url, rawRequest = _rawRequest },
                CorrelationId = _correlationId,
                ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds,
                MethodName = actionExecutedContext.Request.RequestUri.PathAndQuery,
                ReturnValue = _returnValueJson,
                UserID = environmentUser.LoginUser.UserId
            };

            JxTask.Run(EnvironmentUser, () =>
            {
                methodInvocationLogService.Create(insertMethodInvocationLogParam);
            });
        }

        private EnvironmentUser GetEnvironmentUser(HttpRequestMessage request)
        {
            var environmentUser = request.GetRequestPropertyValue<EnvironmentUser>(RequestMessagePropertyKey.EnvironmentUser);

            if (environmentUser != null)
            {
                return environmentUser;
            }

            return EnvironmentUser;
        }

        private class ResponseContent
        {
            public object Value { get; set; }
        }
    }
}