using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsAllUserTransferOutEnabled => false;

        protected override bool IsDoTransferCompensationJobEnabled => false;

        protected override bool IsUpdateTPGameUserScoreJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override bool IsTransferAllOutJobEnabled => false;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);

            containerBuilder.RegisterType(typeof(TPGameCQ9SLApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}