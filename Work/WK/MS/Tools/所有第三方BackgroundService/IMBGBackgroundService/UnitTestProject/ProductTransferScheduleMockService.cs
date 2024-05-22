using Autofac;
using JxBackendService.Model.Enums;
using ProductTransferService.IMBGDataBase.Merchant;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => true;

        protected override bool IsDoTransferCompensationJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMBGBetDetailMockService)).Keyed<IIMBGBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}