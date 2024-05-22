using Autofac;
using JxBackendService.Model.Enums;
using SportDataBase.Merchant;

namespace SportUnitTest
{
    public class SportTransferMockService : SportTransferService.SportTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => true;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override int SaveBetLogToPlatformJobIntervalSeconds => 1;

        protected override bool DoInitialJobOnStart()
        {
            //if (!INITAppSettings())
            //{
            //    return false;
            //}

            //SportDataBase.DLL.SportProfitLossInfo.InIt();

            return base.DoInitialJobOnStart();
        }

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(SportBetDetailMSLMockService)).Keyed<ISportBetDetailService>(PlatformMerchant.MiseLiveStream.Value);
        }
    }
}