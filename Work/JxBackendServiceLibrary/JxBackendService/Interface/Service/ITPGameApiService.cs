using System;
using System.Collections.Generic;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameApiReadService
    {
        bool IsBackupBetLog { get; }

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

        BaseReturnModel CreateTransferOutInfo(TPGameTranfserParam param, bool isTransferOutAll, out string moneyId);

        BaseReturnModel CreateTransferOutInfo(TPGameTranfserParam param, bool isTransferOutAll, out decimal actuallyAmount, out string moneyId);

        BaseReturnDataModel<decimal> CreateAllAmountTransferOutInfo(TPGameTranfserParam param, out bool isAllAmount);

        void SaveProfitlossToPlatform(List<InsertTPGameProfitlossParam> tpGameProfitlosses, Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus);

        void SaveProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam);

        void BackupBetLog(BaseReturnDataModel<RequestAndResponse> requestAndResponseResult);
    }

    public interface IOldTPGameApiService
    {
        BaseReturnModel UpdateUserScoreFromRemote();
    }

    public interface IOldTPGameApiReadService
    {
    }
}