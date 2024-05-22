using JxBackendService.Model.ViewModel.ThirdParty.Old;
using LCDataBase.Model;

namespace LCDataBase.Merchant
{
    public class LCBetDetailMSLService : LCBetDetailMerchantService
    {
        protected override bool IsBackupBetLog => false;

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(LCApiParamModel apiParam)
        {
            return GetRemoteFileBetLogResult(apiParam);
        }
    }
}