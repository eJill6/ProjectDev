using ControllerShareLib.Interfaces.Service;
using JxBackendService.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Core.Infrastructure.Filters;

public class SwaggerHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var byteArrayApiService = DependencyUtil.ResolveService<IByteArrayApiService>().Value;
        operation.Parameters.Add(CreateOpenApiParameter(byteArrayApiService.EncBytesHeader, defaultValue: "false"));

        var headers = new List<OpenApiParameter>();

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