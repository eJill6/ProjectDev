using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.MSL
{
    public class TPGamePMSLApiMSLService : TPGamePMSLApiService
    {
        public TPGamePMSLApiMSLService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        //todo 上線後打開註解，比照D3從FTP撈資料
        //protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken) => GetRemoteFileBetLogResult(lastSearchToken);
    }
}