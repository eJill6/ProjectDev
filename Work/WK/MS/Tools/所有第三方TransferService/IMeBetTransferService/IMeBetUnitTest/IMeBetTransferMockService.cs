using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;

namespace IMeBetUnitTest
{
    public class IMeBetTransferMockService : IMeBetTransferService.IMeBetTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //IMeBetDataBase.DLL.IMeBetProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMeBetBetDetailMockService)).Keyed<IIMeBetBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}