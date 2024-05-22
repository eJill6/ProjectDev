using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.MSL;

namespace UnitTestProject
{
    [MockService]
    public class TPGameLCApiMSLMockService : TPGameLCApiMSLService
    {
        public TPGameLCApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            throw new ArgumentNullException("test");
        }
    }
}