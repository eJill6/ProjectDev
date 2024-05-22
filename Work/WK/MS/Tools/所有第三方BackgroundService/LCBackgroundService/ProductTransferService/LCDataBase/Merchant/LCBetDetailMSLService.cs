using JxBackendService.Model.ViewModel.ThirdParty.Old;
using ProductTransferService.LCDataBase.Model;

namespace ProductTransferService.LCDataBase.Merchant
{
    public class LCBetDetailMSLService : LCBetDetailMerchantService
    {
        protected override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(LCApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}