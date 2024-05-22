using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;

namespace SportTransferService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ServiceBase ServicesToRun = new SportTransferService();
            if (Environment.UserInteractive)
            {
                AllocConsole();
                RunServiceInteractive(ref ServicesToRun);
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
            Console.ReadKey();
            //Stop
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var service in services)
            {
                onStopMethod.Invoke(service, null);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Exception ex = e.ExceptionObject as Exception;
                SportDataBase.Common.LogsManager.Info("体育服务崩溃，错误信息:"+ ex.Message 
                + "\n" + "InnerException:" + ex.InnerException
                + "\n" + "Source:" + ex.Source
                + "\n" + "TargetSite:" + ex.TargetSite
                + "\n" + "StackTrace:" + ex.StackTrace);
            }

        }
        static void RunServiceInteractive(ref ServiceBase serviceToRun)
        {
            Console.WriteLine("Running service in interactive mode.\n");
            MethodInfo OnStartMethodInfo = typeof(SportTransferService).GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo OnStopMethodInfo = typeof(SportTransferService).GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                OnStartMethodInfo.Invoke(serviceToRun, new object[] { new string[] { } });
                Console.WriteLine("Service started: {0}", serviceToRun.ServiceName);
                Console.WriteLine("\nPress any key to stop the services and end the process...\n");
                Console.ReadKey();

                OnStopMethodInfo.Invoke(serviceToRun, null);
                Console.WriteLine("Service stopped: {0}", serviceToRun.ServiceName);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
            }
            Console.WriteLine("\nAll services stopped.");
            Console.ReadKey();
        }

        //Open console window in application.
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
    }
}
