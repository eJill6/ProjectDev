using ProductTransferService.AgDataBase.DLL;
using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using ProductTransferService;
using ProductTransferService.AgDataBase.Common;
using UnitTestProject;
using JxBackendService.Model.Common;
using ProductTransferService.AgDataBase.DLL.FileService;

namespace UnitTestProject
{
    public class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsAgTransferProfitLossEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => false;

        //protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(AgMockApi)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(ParseAGXmlFileMockService)).AsImplementedInterfaces();            
            containerBuilder.RegisterType(typeof(AGRemoteOssXmlFileMockService)).Keyed<IAGRemoteXmlFileService>(SharedAppSettings.PlatformMerchant.Value);

            containerBuilder.RegisterType(typeof(TPGameAGApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));

            containerBuilder.RegisterType(typeof(TPGameAGApiMSLMockService))
                .Keyed<ITPGameApiReadService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}