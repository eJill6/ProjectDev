using IMDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMBGDataBase.Merchant
{
    public class IMBetDetailMSLService : IMBetDetailMerchantService
    {
        protected override bool IsBackupBetLog => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}