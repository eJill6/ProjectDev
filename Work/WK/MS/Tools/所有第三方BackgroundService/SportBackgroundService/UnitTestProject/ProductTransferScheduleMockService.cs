using Autofac;
using JxBackendService.Model.Enums;
using ProductTransferService.SportDataBase.Merchant;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsTransferAllOutJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            return base.DoInitialJobOnStart(cancellationToken);
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(SportBetDetailMSLMockService)).Keyed<ISportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}