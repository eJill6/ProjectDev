using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using ProductTransferService.Service;
using UnitTestProject;

namespace ProductTransferService
{
    [MockService]
    public partial class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveRemoteBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override bool IsTransferAllOutJobEnabled => true;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(TPGameWLBGApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));

            //containerBuilder.RegisterType(typeof(EnvironmentService)).AsImplementedInterfaces();
        }

        //protected override BaseReturnDataModel<List<WLBGBetLog>> ConvertToBetLogs(string apiResult)
        //{
        //    //return base.ConvertToBetLogs(apiResult);
        //    List<WLBGBetLog> betLogs = TransferSqlLiteBackupRepository.GetBetLogs<WLBGBetLog>(DateTime.Parse("2023-12-26"), rowCount: 1000);

        //    return new BaseReturnDataModel<List<WLBGBetLog>>(ReturnCode.Success, betLogs);
        //}
    }
}