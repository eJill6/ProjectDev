using Autofac;
using JxBackendService.Model.Enums;
using LCDataBase.Merchant;

namespace LCUnitTest
{
    public class LCTransferMockService : LCTransferService.LCTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => true;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //LCDataBase.DLL.LCProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            //containerBuilder.RegisterType(typeof(LCBetDetailMockService)).Keyed<ILCBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}