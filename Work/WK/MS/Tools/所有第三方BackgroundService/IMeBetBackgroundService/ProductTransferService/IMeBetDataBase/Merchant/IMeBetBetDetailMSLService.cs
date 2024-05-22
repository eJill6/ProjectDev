using IMeBetDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMBGDataBase.Merchant
{
    public class IMeBetBetDetailMSLService : IMeBetBetDetailMerchantService
    {
        protected override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMeBetApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}