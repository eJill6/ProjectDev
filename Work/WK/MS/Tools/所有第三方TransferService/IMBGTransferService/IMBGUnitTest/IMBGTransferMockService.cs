using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;

namespace IMBGUnitTest
{
    public class IMBGTransferMockService : IMBGTransferService.IMBGTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //IMBGDataBase.DLL.IMBGProfitLossInfo.InIt();

            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMBGBetDetailMockService)).Keyed<IIMBGBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}