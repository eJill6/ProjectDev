using Autofac;
using BatchService.Interface;
using BatchService.Model.Enum;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNF.Common.Util;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace BatchServiceUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.BatchService,
            LoginUser = new BasicUserInfo
            {
                UserId = 0
            }
        };

        public UnitTest1()
        {
            string assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";

            // 加上autofac
            var builder = new ContainerBuilder();
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            DependencyUtil.SetContainer(builder.Build());
        }

        //[TestMethod]
        //public void TestBatchServiceMockService()
        //{
        //    ReflectUtil.RunInteractive(new BatchServiceMockService() );
        //}

        [TestMethod]
        public void TestBatchMainMockService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BatchMainMockService()
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

        [TestMethod]
        public void TestJobSettingMSService()
        {
            var JobSettingMSService = DependencyUtil.ResolveKeyed<IJobSettingService>(PlatformMerchant.MiseLiveStream);
            List<JobSetting> actual = JobSettingMSService.GetAll();
            List<JobSetting> expected = new List<JobSetting>()
            {
                JobSetting.StoredProcedureErrorNotice,
            };

            string actualJsonString = actual.ToJsonString();
            string expectedJsonString = expected.ToJsonString();
            Assert.AreEqual(expectedJsonString, actualJsonString);
        }

        [TestMethod]
        public void TestCrawlOBEBAnchors()
        {
            var service = DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(_environmentUser, DbConnectionTypes.Master);
            BaseReturnModel baseReturnDataModel = service.CrawlAnchors();

            LogUtil.ForcedDebug(baseReturnDataModel.ToJsonString());

            List<IMiseLiveAnchor> anchorsResult = service.GetAnchors();

            Assert.IsTrue(anchorsResult.AnyAndNotNull());
        }
    }
}