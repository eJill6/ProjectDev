using Autofac;
using JxBackendService.DependencyInjection;

namespace MobileApiCreateSign
{
    /// <summary>
    /// 原專案廢除了簽章和ECDH加密的機制，此專案僅當備用/供參考
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            //RegisterConfiguration(containerBuilder);
            DependencyUtil.SetContainer(containerBuilder.Build());

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Generate());
        }
    }
}