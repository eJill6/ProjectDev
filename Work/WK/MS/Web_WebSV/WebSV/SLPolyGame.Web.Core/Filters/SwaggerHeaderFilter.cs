using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using JxBackendService.Common.Util;

namespace SLPolyGame.Web.Core.Filters
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Tags.Any(a => a.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var headers = new List<OpenApiParameter>
            {
                CreateOpenApiParameter("p1", "UserID"),
                CreateOpenApiParameter("p2", "UserKey"),
                //CreateOpenApiParameter("ip"),
                //CreateOpenApiParameter("UserAgent")
            };

            foreach (var header in headers)
            {
                operation.Parameters.Add(header);
            }
        }

        private OpenApiParameter CreateOpenApiParameter(string name, string description = null)
        {
            if (description.IsNullOrEmpty())
            {
                description = name;
            }

            return new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = false,
                Schema = new OpenApiSchema { Type = "String" }
            };
        }
    }
}