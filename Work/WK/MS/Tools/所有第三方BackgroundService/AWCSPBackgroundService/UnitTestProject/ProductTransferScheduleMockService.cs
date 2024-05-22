using Autofac;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterType(typeof(TPGameAWCSPApiMSLMockService))
            //    .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}