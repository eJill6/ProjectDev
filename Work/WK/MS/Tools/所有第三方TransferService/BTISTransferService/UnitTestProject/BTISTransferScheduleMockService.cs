using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using ProductTransferService;

namespace UnitTestProject
{
    public class BTISTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => base.RecheckProcessingStatusOrderJobIntervalSeconds; //10

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(TPGameBTISApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}