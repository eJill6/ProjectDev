using System;
using System.Reflection;
using System.ServiceProcess;

namespace IMPPTransferService
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IMPPTransferService()
            };
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }

        /// <summary>
        /// 屬性切換到主控台Debug
        /// </summary>
        public static void RunInteractive(ServiceBase[] services)
        {
            Console.WriteLine("Install the services in interactive mode.");
            //Start
            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var service in services)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            Console.WriteLine("Press a key to uninstall all services...");
            //Console.ReadKey();
            //Stop
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var service in services)
            {
                onStopMethod.Invoke(service, null);
            }

        }
    }
}
