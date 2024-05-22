using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;

namespace IMUnitTest
{
    public class IMTransferMockService : IMTransferService.IMTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override int SaveBetLogToPlatformJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //IMDataBase.DLL.IMProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMBetDetailMSLMockService)).Keyed<IIMBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}