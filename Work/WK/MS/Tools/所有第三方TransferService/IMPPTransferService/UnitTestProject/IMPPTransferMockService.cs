using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;

namespace IMPPUnitTest
{
    public class IMPPTransferMockService : IMPPTransferService.IMPPTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => true;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //IMPPDataBase.DLL.IMPPProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMPPBetDetailMockService)).Keyed<IIMPPBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}