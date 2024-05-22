using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart(CancellationToken cancellationToken)
        {
            return base.DoInitialJobOnStart(cancellationToken);
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMPTBetDetailMSLMockService)).Keyed<IIMPTBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}