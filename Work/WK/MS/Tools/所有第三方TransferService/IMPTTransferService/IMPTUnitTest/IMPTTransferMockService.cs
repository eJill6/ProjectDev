using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;

namespace IMPTUnitTest
{
    public class IMPTTransferMockService : IMPTTransferService.IMPTTransferService
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
            //IMPTDataBase.DLL.IMPTProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMPTBetDetailMSLMockService)).Keyed<IIMPTBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}