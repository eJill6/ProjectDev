﻿using Autofac;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using UnitTestProject;

namespace ProductTransferService
{
    public partial class PMSLTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected override bool IsSaveBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 10;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterType(typeof(TPGamePMSLApiMSLMockService))
            //             .Keyed<ITPGameApiService>(DependencyUtil.GetRegisterKey(Product.Value, PlatformMerchant.MiseLiveStream.Value));
        }
    }
}