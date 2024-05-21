using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace JxBackendService.Attributes.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAllowAnonymous(this FilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(nameof(context));
            }

            if (context.ActionDescriptor.EndpointMetadata.Any(item => item is IAllowAnonymous) ||
                context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return true;
            }

            return false;
        }
    }
}