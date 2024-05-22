using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using UnitTestProject;

namespace ProductTransferService
{
    public class TPGameIMVRApiMSLMockService : TPGameIMVRApiService
    {
        public TPGameIMVRApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //return base.GetRemoteBetLogApiResult(lastSearchToken);
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }
    }
}