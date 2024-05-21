using Autofac;
using Autofac.Extras.DynamicProxy;
using JxBackendService.Common.Util;
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
                    filterTypes.SetInterceptorAndRegister(containerBuilder, typeof(RequiredParameterInterceptor));
                });

            return containerBuilder;
        }

        public static ContainerBuilder RegisterConfiguration(ContainerBuilder containerBuilder)
        {
            IConfigurationRoot configuration = BuildNewConfiguration();
            containerBuilder.RegisterInstance(configuration).As<IConfiguration>();

            return containerBuilder;
        }

        public static IConfigurationRoot BuildNewConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string dotnetEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            if (dotnetEnvironment.IsNullOrEmpty())
            {
                dotnetEnvironment = GetDotNetEnvironmentFromLaunchSettings();
            }

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{dotnetEnvironment}.json", optional: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            return configuration;
        }

        public static ContainerBuilder RegisterHttpContextAccessor(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();

            return containerBuilder;
        }

        private static string GetDotNetEnvironmentFromLaunchSettings()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string lunchSettingFileName = "launchSettings.json";

            if (!File.Exists(Path.Combine(directoryPath, lunchSettingFileName)))
            {
                return null;
            }

            //因為單元測試起來的時候Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")抓不到值,所以這邊直接parse launchSettings.json
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(directoryPath)
               .AddJsonFile(lunchSettingFileName, optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            string environment = configuration["profiles:UnitTestProject:environmentVariables:DOTNET_ENVIRONMENT"];

            return environment;
        }
    }
}