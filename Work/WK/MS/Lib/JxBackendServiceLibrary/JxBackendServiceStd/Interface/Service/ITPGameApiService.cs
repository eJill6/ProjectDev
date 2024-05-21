using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameApiReadService
    {
        bool IsWriteRemoteContentToOtherMerchant { get; }

        BaseReturnModel GetAllowCreateTransferOrderResult();

        BaseReturnModel GetQueryOrderReturnModel(string apiResult);

        List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo();

        Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts);

        /// <summary>計算有效投注額</summary>
        decimal ComputeAdmissionBetMoney(ComputeAdmissionBetMoneyParam computAdmissionBetAmountParam);

        BaseReturnDataModel<UserScore> GetRemoteUserScore(IInvocationUserParam invocationUserParam, bool isRetry);
    }

    public interface ITPGameApiService
    {
        PlatformProduct Product { get; }

        List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo();

        BaseReturnDataModel<UserScore> GetRemoteUserScore(IInvocationUserParam invocationUserParam, bool isRetry);

        BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken);

        void RecheckProcessingOrders(object state);

        BaseReturnModel CheckOrCreateAccount(int userId);

        BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlParam param);

        BaseReturnDataModel<string> GetLoginApiResult(ForwardGameUrlParam param);

        BaseReturnModel CreateTransferInInfo(TPGameTranfserParam param);

        BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out string moneyId);

        BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out decimal actuallyAmount, out string moneyId);

        BaseReturnDataModel<decimal> CreateAllAmountTransferOutInfo(TPGameTranfserOutParam param, out bool isAllAmount);

        void SaveMultipleProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam);

        void WriteRemoteContentToOtherMerchant(BaseReturnDataModel<RequestAndResponse> requestAndResponseResult);

        void KickUser(int userId);

        void StartDequeueUpdateTPGameUserScoreJob();
    }

    public interface ITPGameRemoteApiService
    {        
        DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo);

        string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam);
    }
}