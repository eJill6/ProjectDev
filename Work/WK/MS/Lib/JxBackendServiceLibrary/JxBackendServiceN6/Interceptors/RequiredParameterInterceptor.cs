using Castle.DynamicProxy;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JxBackendServiceN6.Interceptors
{
    public class RequiredParameterInterceptor : IInterceptor
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<string>> s_RequiredPropertiesMap = new();

        protected virtual object[] ConvertArguments(object[] arguments) => arguments;

        public void Intercept(IInvocation invocation)
        {
            object[] arguments = ConvertArguments(invocation.Arguments);

            foreach (object argument in arguments)
            {
                if (argument is IRequiredParam == false)
                {
                    continue;
                }

                Type argumentType = argument.GetType();
                RuntimeTypeHandle runtimeTypeHandle = argument.GetType().TypeHandle;

                if (!s_RequiredPropertiesMap.TryGetValue(runtimeTypeHandle, out List<string> requiredPropertyNames))
                {
                    requiredPropertyNames = new List<string>();

                    foreach (PropertyInfo property in argument.GetType().GetProperties())
                    {
                        var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();

                        if (requiredAttribute != null)
                        {
                            requiredPropertyNames.Add(property.Name);
                        }
                    }

                    s_RequiredPropertiesMap.TryAdd(runtimeTypeHandle, requiredPropertyNames);
                }


                foreach (string requiredPropertyName in requiredPropertyNames)
                {
                    PropertyInfo property = argumentType.GetProperty(requiredPropertyName);

                    object value = property.GetValue(argument);

                    if (RequiredUtil.IsValidRequired(value) == false)
                    {
                        Type returnType = invocation.Method.ReturnType;
                        string errorMessage = string.Format(MessageElement.FieldIsNotAllowEmpty + "(※)", property.Name);

                        if (returnType == typeof(BaseReturnModel))
                        {
                            invocation.ReturnValue = new BaseReturnModel(errorMessage);

                            return;
                        }

                        throw new ArgumentNullException(errorMessage);
                    }
                }
            }

            // 繼續執行原方法
            invocation.Proceed();
        }
    }
}