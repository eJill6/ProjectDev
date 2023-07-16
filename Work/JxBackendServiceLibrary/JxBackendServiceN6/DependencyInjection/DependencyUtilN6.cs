using Autofac;
using Autofac.Extras.DynamicProxy;
using JxBackendService.DependencyInjection;
using JxBackendServiceN6.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace JxBackendServiceN6.DependencyInjection
{
    public class DependencyUtilN6
    {
        public static ContainerBuilder GetJxBackendServiceContainerBuilder(string assemblyPath, ContainerBuilder containerBuilder)
        {
            containerBuilder = DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(
                assemblyPath,
                "JxBackendServiceN6.dll",
                containerBuilder,
                (filterTypes) =>
                {
                    filterTypes
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(RequiredParameterInterceptor));
                });

            var interceptorTypes = new List<Type>()
            {
                typeof(RequiredParameterInterceptor),
            };

            interceptorTypes.ForEach(f => containerBuilder.RegisterType(f));

            return containerBuilder;
        }

        public static ContainerBuilder RegisterConfiguration(ContainerBuilder containerBuilder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            containerBuilder.RegisterInstance(configuration).As<IConfiguration>();

            return containerBuilder;
        }

        public static ContainerBuilder RegisterHttpContextAccessor(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}