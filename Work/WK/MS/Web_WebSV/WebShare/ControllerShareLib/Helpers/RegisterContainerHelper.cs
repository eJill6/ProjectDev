using Autofac;
using ControllerShareLib.Interfaces.Service.Setting;
using ControllerShareLib.Services.Setting;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using System.Reflection;

namespace ControllerShareLib.Helpers
{
    public static class RegisterContainerHelper
    {
        public static void RegisterDllFiles(string assemblyPath, string[] dllFileNames, ContainerBuilder containerBuilder)
        {
            foreach (string dllFileName in dllFileNames)
            {
                DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(assemblyPath, dllFileName, containerBuilder, processTypes: null);

                Assembly serviceAssmbly = Assembly.LoadFrom(Path.Combine(assemblyPath, dllFileName));
                RegisterPlayConfig(serviceAssmbly, containerBuilder);
            }

            foreach (PlatformMerchant platformMerchant in PlatformMerchant.GetAll())
            {
                string mobileApiServiceKey = DependencyUtil.GetRegisterKey(JxApplication.MobileApi.Value, platformMerchant.Value);
                containerBuilder.RegisterType<ControllerJobSettingMobileApiService>().Keyed<IControllerJobSettingService>(mobileApiServiceKey);

                string webServiceKey = DependencyUtil.GetRegisterKey(JxApplication.FrontSideWeb.Value, platformMerchant.Value);
                containerBuilder.RegisterType<ControllerJobSettingWebService>().Keyed<IControllerJobSettingService>(webServiceKey);
            }
        }

        public static void RegisterPlayConfig(Assembly assembly, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Adapter") || t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("PlayConfig") || t.Name.EndsWith("SpaTrendHelper"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}