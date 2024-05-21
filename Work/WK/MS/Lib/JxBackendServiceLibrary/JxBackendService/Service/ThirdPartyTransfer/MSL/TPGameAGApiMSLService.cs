using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.MSL
{
    public class TPGameAGApiMSLService : TPGameAGApiService
    {
        public TPGameAGApiMSLService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
            => throw new NotSupportedException("目前舊版Transfer走自己的FTP流程");
    }
}