using BatchService.Service;
using JxBackendServiceNF.Common.Util;
using System;
using System.Reflection;
using System.ServiceProcess;

namespace ProductTransferService
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new BatchMainService()
            };

            if (Environment.UserInteractive)
            {
                ReflectUtilNF.RunInteractive(ServicesToRun);
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}