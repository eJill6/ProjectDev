using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => true;

        protected override bool IsDoTransferCompensationJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        //protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        //{
        //    base.AppendServiceToContainerBuilder(containerBuilder);
        //    containerBuilder.RegisterType(typeof(IMeBetBetDetailMockService)).Keyed<IIMeBetBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        //}
    }
}