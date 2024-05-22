using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;

namespace UnitTestProject
{
    public class TPGameIMVRApiMSLMockService : TPGameIMVRApiMSLService
    {
        public TPGameIMVRApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }
    }
}