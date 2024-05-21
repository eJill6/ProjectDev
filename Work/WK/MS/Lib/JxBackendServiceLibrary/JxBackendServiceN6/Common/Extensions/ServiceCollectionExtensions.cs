using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace JxBackendServiceN6.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterNetCore(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            services.AddTransient((serviceProvider) =>
            {
                ActionContext? actionContext = serviceProvider.GetService<IActionContextAccessor>()?.ActionContext;
                IUrlHelper? urlHelper = serviceProvider.GetService<IUrlHelperFactory>()?.GetUrlHelper(actionContext);

                return urlHelper;
            });
        }
    }
}