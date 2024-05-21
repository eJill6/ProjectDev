using Castle.DynamicProxy;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JxBackendService.Interceptors.Base
{
    public abstract class BaseLogToDbInterceptor : IInterceptor
    {
        protected abstract HashSet<string> InterceptMethodFilters { get; }

        protected virtual object[] ConvertArguments(object[] arguments) => arguments;

        public void Intercept(IInvocation invocation)
        {
            object invocationTarget = invocation.InvocationTarget;
            string methodName = invocation.Method.Name;

            EnvironmentUser environmentUser = (invocationTarget as BaseEnvLoginUserService).EnvLoginUser;
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentUser.Application, SharedAppSettings.PlatformMerchant).Value;

            if ((InterceptMethodFilters.Any() && !InterceptMethodFilters.Contains(methodName)) ||
                 invocationTarget is BaseEnvLoginUserService == false ||
                 appSettingService.IsEnabledMethodInvocationLog == false)
            {
                invocation.Proceed(); //純執行不做log

                return;
            }

            string errorMsg = string.Empty;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
                logUtilService.Error(ex);
                errorMsg = ex.Message;

                throw ex;
            }
            finally
            {
                stopwatch.Stop();

                object[] arguments = ConvertArguments(invocation.Arguments);
                GetInvocationParamValues(arguments, out string correlationId, out int? userId);

                object returnValue = invocation.ReturnValue;

                var methodInvocationLogReadService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogReadService>(
                    environmentUser,
                    DbConnectionTypes.Slave).Value;

                IInsertMethodInvocationLogParam insertMethodInvocationLogParam = new InsertMethodInvocationLogParam()
                {
                    Arguments = arguments,
                    CorrelationId = correlationId,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ErrorMsg = errorMsg,
                    MethodName = methodName,
                    ReturnValue = returnValue,
                    UserID = environmentUser.LoginUser.UserId,
                    TypeName = invocationTarget.GetType().Name,
                    CreateDate = DateTime.Now,
                };

                if (userId.HasValue)
                {
                    insertMethodInvocationLogParam.UserID = userId.Value;
                }

                methodInvocationLogReadService.Enqueue(insertMethodInvocationLogParam);
            }
        }

        private void GetInvocationParamValues(object[] arguments, out string correlationId, out int? userId)
        {
            correlationId = string.Empty;
            userId = null;

            foreach (object argument in arguments)
            {
                if (argument is IInvocationParam)
                {
                    correlationId = (argument as IInvocationParam).CorrelationId;

                    if (argument is IInvocationUserParam)
                    {
                        userId = (argument as IInvocationUserParam).UserID;
                    }

                    break;
                }
            }
        }
    }
}