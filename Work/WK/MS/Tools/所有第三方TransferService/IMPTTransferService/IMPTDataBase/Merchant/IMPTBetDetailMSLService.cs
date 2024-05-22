using IMPTDataBase.Model;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMBGDataBase.Merchant
{
    public class IMPTBetDetailMSLService : IMPTBetDetailMerchantService
    {
        protected override bool IsBackupBetLog => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMPTApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}