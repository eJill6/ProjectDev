using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using JxBackendService.Model.Enums;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTestN6
{
    public abstract class BaseUnitTest
    {
        protected abstract JxApplication Application { get; }

        protected virtual EnvironmentUser EnvironmentUser => new EnvironmentUser
        {
            Application = Application,
            LoginUser = new BasicUserInfo
            {
                UserId = 888,
            }
        };

        public BaseUnitTest()
        {
            var containerBuilder = new ContainerBuilder();
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            containerBuilder = DependencyUtilN6.RegisterHttpContextAccessor(containerBuilder);
            RegisterConfiguration(containerBuilder);
            DependencyUtil.SetContainer(containerBuilder.Build());
        }

        protected async Task RunHostedServiceAsync<THostedService>() where THostedService : class, IHostedService
        {
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<THostedService>();
                })
            .Build();

            await host.RunAsync();
        }

        private void RegisterConfiguration(ContainerBuilder containerBuilder)
        {
            string environment = GetDotNetEnvironment();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            configuration = configurationBuilder.Build(); //rebuild

            containerBuilder.RegisterInstance(configuration).As<IConfiguration>();
        }

        private string GetDotNetEnvironment()
        {
            //因為單元測試起來的時候Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")抓不到值,所以這邊直接parse launchSettings.json
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("launchSettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();
            string environment = configuration["profiles:UnitTestProject:environmentVariables:DOTNET_ENVIRONMENT"];

            return environment;
        }
    }
}