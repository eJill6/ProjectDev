using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNet45.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Base
{
    public class BaseTest
    {
        protected virtual EnvironmentUser EnvLoginUser => new EnvironmentUser()
        {
            Application = JxApplication.FrontSideWeb,
            LoginUser = new BasicUserInfo()
            {
                UserId = 69778,
                UserName = "jackson"
            }
        };

        public BaseTest()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";

            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = ContainerBuilderUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            DependencyUtil.SetContainer(builder.Build());
        }

        /// <summary>
        /// 屬性切換到主控台Debug
        /// </summary>
        protected static void RunInteractive(ServiceBase[] services)
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
    }
}
