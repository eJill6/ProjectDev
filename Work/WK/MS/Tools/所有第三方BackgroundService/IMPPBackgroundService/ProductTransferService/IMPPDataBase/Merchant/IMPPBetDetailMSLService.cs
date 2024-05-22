using IMPPDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMBGDataBase.Merchant
{
    public class IMPPBetDetailMSLService : IMPPBetDetailMerchantService
    {
        protected override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMPPApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}