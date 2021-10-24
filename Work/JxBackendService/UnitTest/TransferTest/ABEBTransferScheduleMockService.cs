using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;

namespace UnitTest.TransferTest
{
    public class ABEBTransferScheduleMockService : BaseABTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.ABEB;

        public override JxApplication Application => JxApplication.ABEBTransferService;

        protected override bool IsTransferInJobEnabled => false;

        protected override bool IsTransferOutJobEnabled => false;

        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => true;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => base.RecheckProcessingStatusOrderJobIntervalSeconds; //10        
    }
}
