using IMBGDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMBGDataBase.Merchant
{
    public class IMBGBetDetailMSLService : IMBGBetDetailMerchantService
    {
        protected override bool IsBackupBetLog => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMBGApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}