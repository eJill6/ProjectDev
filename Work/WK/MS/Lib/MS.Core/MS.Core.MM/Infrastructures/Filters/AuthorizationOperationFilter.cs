using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MS.Core.MM.Infrastructures.Filters
{
    /// <summary>
    /// 實作 <see cref="IOperationFilter"/> 判斷是否需要驗證
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context?.MethodInfo?.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (authAttributes?.Any() == true)
            {
                // 有要驗證再加上相關選項
                var securityRequirement = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                };

                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };

                var status401UnauthorizedKey = StatusCodes.Status401Unauthorized.ToString();

                if (!operation.Responses.ContainsKey(status401UnauthorizedKey))
                {
                    operation.Responses.Add(status401UnauthorizedKey, new OpenApiResponse() { Description = "Unauthorized" });
                }
            }
        }
    }
}