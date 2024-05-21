using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UnitTestN6
{
    public abstract class BaseUnitTest
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected virtual EnvironmentUser EnvironmentUser => new EnvironmentUser
        {
            Application = s_environmentService.Value.Application,
            LoginUser = new BasicUserInfo
            {
                UserId = 888,
            }
        };

        protected static JxApplication Application => s_environmentService.Value.Application;

        public BaseUnitTest()
        {
            var containerBuilder = new ContainerBuilder();
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            containerBuilder = DependencyUtilN6.RegisterHttpContextAccessor(containerBuilder);
            DependencyUtilN6.RegisterConfiguration(containerBuilder);
            containerBuilder.RegisterType<UnitTestFrontSideWebEnvironmentService>().AsImplementedInterfaces();
            AppendServiceToContainerBuilder(containerBuilder);
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

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
        }
    }

    public class UnitTestFrontSideWebEnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.FrontSideWeb;
    }

    public class UnitTestBatchEnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.BatchService;
    }

    public class UnitTestMobileApiEnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.MobileApi;
    }
}