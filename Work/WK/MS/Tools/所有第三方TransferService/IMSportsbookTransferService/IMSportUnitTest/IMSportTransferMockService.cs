using Autofac;
using IMBGDataBase.Merchant;
using JxBackendService.Model.Enums;

namespace IMSportUnitTest
{
    public class IMSportTransferMockService : IMSportsbookTransferService.IMSportsbookTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override int SaveBetLogToPlatformJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //IMSportsbookDataBase.DLL.IMSportsbookProfitLossInfo.InIt();
            //return InitAppSettings();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(IMSportBetDetailMockService)).Keyed<IIMSportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}