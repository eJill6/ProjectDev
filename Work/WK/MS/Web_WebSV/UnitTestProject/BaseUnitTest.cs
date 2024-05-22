using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendServiceNF.Service.Net;
using System;
using UnitTestProject.MockService;

namespace UnitTestProject
{
    public class BaseUnitTest
    {
        public BaseUnitTest()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "\\";

            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            //註冊MockService
            builder.RegisterType(typeof(ThirdPartyApiMockService)).AsImplementedInterfaces();
            builder.RegisterType(typeof(IpUtilMockService)).AsImplementedInterfaces();
            //builder.RegisterType(typeof(SLPolyGameMockService)).AsImplementedInterfaces();

            DependencyUtil.SetContainer(builder.Build());
        }

        //protected virtual EnvironmentUser CreateEnvironmentUser()
        //{
        //    return new EnvironmentUser()
        //    {
        //        Application = JxApplication.FrontSideWeb,
        //        LoginUser = new BasicUserInfo() { UserId = 6251, UserKey = LoginKeyUtil.Create(JxApplication.FrontSideWeb, 6251) }
        //    };
        //}
    }
}