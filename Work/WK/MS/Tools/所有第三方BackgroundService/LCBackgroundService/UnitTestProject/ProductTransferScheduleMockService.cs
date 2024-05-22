using Autofac;
using Autofac.Extras.DynamicProxy;
using JxBackendService.DependencyInjection;
using JxBackendService.Interceptors;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using ProductTransferService.LCDataBase.Merchant;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => true;

        protected override bool IsDoTransferCompensationJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(LCBetDetailMockService)).Keyed<ILCBetDetailService>(PlatformMerchant.MiseLiveStream.Value);

            string productMerchantKey = DependencyUtil.GetRegisterKey(Product.Value, Merchant.Value);
            containerBuilder.RegisterType<TPGameLCApiMSLMockService>()
                .Keyed<ITPGameApiService>(productMerchantKey)
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TPGameApiServiceInterceptor));
        }
    }
}