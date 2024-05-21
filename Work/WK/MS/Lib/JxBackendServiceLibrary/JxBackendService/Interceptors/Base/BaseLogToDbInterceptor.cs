using Castle.DynamicProxy;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interceptors.Base
{
    public abstract class BaseLogToDbInterceptor : IInterceptor
    {
        protected virtual HashSet<string> InterceptMethodFilters { get; } = new HashSet<string>();

        protected virtual object[] ConvertArguments(object[] arguments) => arguments;

        public void Intercept(IInvocation invocation)
        {
            object invocationTarget = invocation.InvocationTarget;
            string methodName = invocation.Method.Name;

            if ((InterceptMethodFilters.Any() && !InterceptMethodFilters.Contains(methodName)) ||
                 invocationTarget is BaseService == false)
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
                LogUtil.Error(ex);
                errorMsg = ex.Message;
            }
            finally
            {
                stopwatch.Stop();

                object[] arguments = ConvertArguments(invocation.Arguments);
                string correlationId = GetCorrelationId(arguments);

                object returnValue = invocation.ReturnValue;
                EnvironmentUser environmentUser = (invocationTarget as BaseService).EnvLoginUser;

                var methodInvocationLogService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogService>(
                    environmentUser,
                    DbConnectionTypes.Master);

                IInsertMethodInvocationLogParam insertMethodInvocationLogParam = new InsertMethodInvocationLogParam()
                {
                    Arguments = arguments,
                    CorrelationId = correlationId,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ErrorMsg = errorMsg,
                    MethodName = $"{invocationTarget.GetType().Name}.{methodName}",
                    ReturnValue = returnValue,
                    UserID = environmentUser.LoginUser.UserId,
                };

                JxTask.Run(environmentUser, () =>
                {
                    methodInvocationLogService.Create(insertMethodInvocationLogParam);
                });
            }
        }

        private string GetCorrelationId(object[] arguments)
        {
            string correlationId = string.Empty;

            foreach (object argument in arguments)
            {
                if (argument is IInvocationParam)
                {
                    correlationId = (argument as IInvocationParam).CorrelationId;

                    break;
                }
            }

            return correlationId;
        }
    }
}