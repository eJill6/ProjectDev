using JxBackendService.Common.Util;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Castle.DynamicProxy;

namespace JxBackendServiceN6.Interceptors
{
    public class RequiredParameterInterceptor : IInterceptor
    {
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

                PropertyInfo[] properties = argument.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();

                    if (requiredAttribute == null)
                    {
                        continue;
                    }

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