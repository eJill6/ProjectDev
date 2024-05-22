using ProductTransferService.IMBGDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace ProductTransferService.IMBGDataBase.Merchant
{
    public class IMBGBetDetailMSLService : IMBGBetDetailMerchantService
    {
        protected override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMBGApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}