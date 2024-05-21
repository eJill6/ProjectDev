using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.SMS;
using System;

namespace UnitTestN6
{
    [TestClass]
    public class SMSTest : BaseUnitTest
    {
        private readonly Lazy<ISendSMSManagerService> _sendSMSManagerService;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UnitTestBatchEnvironmentService>().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(SendSMSManagerMockService)).AsImplementedInterfaces();
        }

        public SMSTest()
        {
            _sendSMSManagerService = DependencyUtil.ResolveEnvLoginUserService<ISendSMSManagerService>(EnvironmentUser);
        }

        [TestMethod]
        public void CreateSMS()
        {
            _sendSMSManagerService.Value.SendSMS(new SendUserSMSParam
            {
                ContentParamInfo = "TEST",
                PhoneNo = "1566999999",
                Usage = SMSUsages.ValidateCode
            });

            Console.Read();
        }

        [TestMethod]
        public void CreateTencentCloudSMS()
        {
            var smsService = DependencyUtil.ResolveKeyed<ISMSService>(SMSServiceProvider.TencentCloud);

            BaseReturnModel returnModel = smsService.Value.SendSMS(new SendUserSMSParam()
            {
                CountryCode = CountryCode.Taiwan.Value,
                PhoneNo = "",
                ContentParamInfo = "888888",
                Usage = SMSUsages.ValidateCode
            });

            Console.Read();
        }

        [TestMethod]
        public void CreateMxtongSMSChina()
        {
            var smsService = DependencyUtil.ResolveKeyed<ISMSService>(SMSServiceProvider.Mxtong);

            BaseReturnModel returnModel = smsService.Value.SendSMS(new SendUserSMSParam()
            {
                CountryCode = CountryCode.China.Value,
                PhoneNo = "",
                ContentParamInfo = "886688",
                Usage = SMSUsages.ValidateCode
            });

            Console.Read();
        }

        [TestMethod]
        public void CreateMxtongSMSInternational()
        {
            var smsService = DependencyUtil.ResolveKeyed<ISMSService>(SMSServiceProvider.Mxtong);

            BaseReturnModel returnModel = smsService.Value.SendSMS(new SendUserSMSParam()
            {
                CountryCode = CountryCode.Taiwan.Value,
                PhoneNo = "",
                ContentParamInfo = "886688",
                Usage = SMSUsages.ValidateCode
            });

            Console.Read();
        }
    }
}