using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;

namespace UnitTestProject
{
    public class TPGameIMSGApiMSLMockService : TPGameIMSGApiMSLService
    {
        public TPGameIMSGApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //return base.GetRemoteBetLogApiResult(lastSearchToken);
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            //return base.GetRemoteTransferApiResult(isMoneyIn, tpGameAccount, tpGameMoneyInfo);
            return MockDataUtil.GetRemoteTransferApiResult(isMoneyIn, createRemoteAccountParam.TPGameAccount, tpGameMoneyInfo);
        }
    }
}