﻿using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.MSL
{
    public class TPGameOBFIApiMSLService : TPGameOBFIApiService
    {
        public TPGameOBFIApiMSLService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken) => GetRemoteFileBetLogResult(lastSearchToken);
    }
}