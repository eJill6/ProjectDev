using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameIMeBETApiService : TPGameIMOneApiService
    {

        public TPGameIMeBETApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMeBET;

        public override IMLotterySharedAppSettings AppSettings => throw new NotImplementedException();
    }
}
