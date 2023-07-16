using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace UnitTest.Base
{
    public class BaseTest
    {
        protected string InlodbConnectionString = "Data Source=amd2-dev-db.ark88.net;Initial Catalog=InLodb;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;";

        protected string InlodbBakConnectionString = "Data Source=amd2-dev-db.ark88.net;Initial Catalog=InLodb_bak;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;";
        //protected string InlodbConnectionString = "Data Source=jx-sit-db.ark88.net;Initial Catalog=InLodb;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;";
        //protected string InlodbBakConnectionString = "Data Source=jx-sit-db.ark88.net;Initial Catalog=InLodb_bak;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;";

        private EnvironmentUser _environmentUser;

        private readonly int _userId = 6251;//36;

        protected virtual JxApplication Application => JxApplication.FrontSideWeb;

        protected static PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        protected virtual EnvironmentUser EnvLoginUser
        {
            get
            {
                _environmentUser = AssignValueOnceUtil.GetAssignValueOnce(_environmentUser,
                    () =>
                    {
                        return new EnvironmentUser()
                        {
                            Application = Application,
                            LoginUser = new BasicUserInfo()
                            {
                                UserId = _userId,
                                UserKey = null// LoginKeyUtil.Create(_application, _userId)
                            }
                        };
                    });

                return _environmentUser;
            }
        }

        protected virtual void RegisterService(ContainerBuilder containerBuilder)
        {
        }

        public BaseTest()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            if (ConfigUtil.IsRegisterMockService)
            {
                //builder.RegisterType(typeof(IpUtilMockService)).AsImplementedInterfaces();
            }

            RegisterService(builder);

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