using AgDataBase.Common;
using AgDataBase.DLL;
using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using UnitTestProject;

namespace UnitTestProject1
{
    public class AgTransferMockService : AgTransferService.AgTransferService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsAgTransferProfitLossEnabled => false;

        protected override bool IsDownloadRemoteLostAndFoundProfitLossEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsRepaireAgAvailableScoresEnabled => false;

        protected override bool IsRefreshAgAvailableScoresEnabled => false;
        //protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 1;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            containerBuilder.RegisterType(typeof(AgMockApi)).AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(ParseAGXmlFileMockService)).AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(TPGameAGApiMSLMockService))
                .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));

            containerBuilder.RegisterType(typeof(TPGameAGApiMSLMockService))
                .Keyed<ITPGameApiReadService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}