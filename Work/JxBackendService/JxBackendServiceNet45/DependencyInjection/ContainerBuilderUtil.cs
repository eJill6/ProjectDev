using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Model.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace JxBackendServiceNet45.DependencyInjection
{
    public static class ContainerBuilderUtil
    {
        public static ContainerBuilder GetJxBackendServiceContainerBuilder(string assemblyPath, ContainerBuilder builder)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            Assembly serviceAssmbly = Assembly.LoadFrom(string.Concat(assemblyPath, "JxBackendServiceNet45.dll"));

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemblyTypes = builder.RegisterAssemblyTypes(serviceAssmbly);
            assemblyTypes.Where(t => t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase) ||
                                     t.Name.EndsWith("Rep", StringComparison.OrdinalIgnoreCase) ||
                                     t.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces();

            foreach (AuthenticatorType authenticatorType in AuthenticatorType.GetAll())
            {
                if (authenticatorType.AuthenticatorServiceType != null)
                {
                    builder.RegisterType(authenticatorType.AuthenticatorServiceType).Keyed<IAuthenticatorService>(authenticatorType.Value);
                }
            }

            return builder;
        }
    }
}
