using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTPGameRemoteApiService : BaseService, ITPGameRemoteApiService
    {
        public BaseTPGameRemoteApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>去遠端的第三方實際打轉帳API取得結果</summary>
        public abstract DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo);

        /// <summary>去遠端的第三方實際打查詢餘額API取得結果</summary>
        public abstract string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam);        
    }
}