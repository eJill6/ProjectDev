using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Common.Util
{
    public static class EndPointUtil
    {
        public static bool HasApiController(this Endpoint endpoint)
        {
            return endpoint.HasActionCustomAttribute<ApiControllerAttribute>();
        }

        public static bool HasActionCustomAttribute<T>(this Endpoint endpoint)
        {
            if (endpoint == null || endpoint?.Metadata == null)
            {
                return false;
            }

            ControllerActionDescriptor controllerActionDescriptor = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();

            if (controllerActionDescriptor == null)
            {
                return false;
            }

            TypeInfo typeInfo = controllerActionDescriptor.ControllerTypeInfo;

            if (typeInfo == null)
            {
                return false;
            }

            if (endpoint == null)
            {
                return false;
            }

            TypeInfo controllerTypeInfo = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerTypeInfo;

            if (controllerTypeInfo != null)
            {
                return controllerTypeInfo.GetCustomAttributes(typeof(T), true).Any();
            }

            return false;
        }
    }
}