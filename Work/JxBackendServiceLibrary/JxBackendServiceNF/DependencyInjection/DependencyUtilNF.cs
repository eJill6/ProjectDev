using Autofac;
using JxBackendService.DependencyInjection;

namespace JxBackendServiceNF.DependencyInjection
{
    public class DependencyUtilNF
    {
        public static ContainerBuilder GetJxBackendServiceContainerBuilder(string assemblyPath, ContainerBuilder builder)
        {
            return DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(assemblyPath,
                "JxBackendServiceNF.dll", builder, processTypes: null);
        }
    }
}