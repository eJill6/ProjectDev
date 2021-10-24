using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;

namespace UnitTest.TransferTest
{
    public class IMSGTransferScheduleMockService : BaseIMLotteryTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override JxApplication Application => JxApplication.IMSGTransferService;

        protected override bool IsTransferInJobEnabled => false;

        protected override bool IsTransferOutJobEnabled => false;

        protected override bool IsRecheckProcessingStatusOrderJobEnabled => true;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => true;

        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override int RecheckProcessingStatusOrderJobIntervalSeconds => 3;
    }
}
