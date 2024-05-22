using ControllerShareLib.Interfaces.Service;
using JxBackendService.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace M.Core.Filters
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
               .Any(attr => attr.GetType() == typeof(MobileApiAuthorize));

            //如果action有掛匿名要排除
            bool hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true)
                .Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

            var headers = new List<OpenApiParameter>();

            if (hasAuthorizeAttribute && !hasAllowAnonymous)
            {
                var openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "mwt"
                    }
                };

                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        { openApiSecurityScheme, new List<string>() }
                    }
                };

                //headers.Add(CreateOpenApiParameter(headerDeviceIdentifier, defaultValue: defaultHeaderValueMap.TryGetValue(headerDeviceIdentifier)),
                //headers.Add(CreateOpenApiParameter(headerDeviceBrand));
                //headers.Add(CreateOpenApiParameter(headerOperatingSystem));
                //headers.Add(CreateOpenApiParameter(headerAppVersion, defaultValue: defaultHeaderValueMap.TryGetValue(headerAppVersion)));

                
            }

            var byteArrayApiService = DependencyUtil.ResolveService<IByteArrayApiService>().Value;
            operation.Parameters.Add(CreateOpenApiParameter(byteArrayApiService.EncBytesHeader, defaultValue: "false"));

            foreach (var header in headers)
            {
                operation.Parameters.Add(header);
            }
        }

        private OpenApiParameter CreateOpenApiParameter(string name, string? defaultValue = null, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                description = name;
            }

            return new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = false,
                Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(defaultValue) },
            };
        }
    }
}