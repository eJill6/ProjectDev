using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class TPGameIMSGApiMSLMockService : TPGameIMSGApiService
    {
        public TPGameIMSGApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return base.GetRemoteBetLogApiResult(lastSearchToken);
            //return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return base.GetRemoteTransferApiResult(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);
            //return MockDataUtil.GetRemoteTransferApiResult(isMoneyIn, createRemoteAccountParam.TPGameAccount, tpGameMoneyInfo);
        }
    }
}