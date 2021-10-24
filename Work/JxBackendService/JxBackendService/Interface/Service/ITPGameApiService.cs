using System;
using System.Collections.Generic;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameApiReadService
    {
        BaseReturnModel GetAllowCreateTransferOrderResult();

        BaseReturnModel GetQueryOrderReturnModel(string apiResult);

        List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo();

        Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts);
    }

    public interface ITPGameApiService
    {
        PlatformProduct Product { get; }

        List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo();

        List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo();

        BaseReturnDataModel<UserScore> GetRemoteUserScore(int userId, bool isRetry);

        BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken);

        void TransferIn(object state);

        void TransferOut(object state);

        void RecheckProcessingOrders(object state);

        BaseReturnModel CheckOrCreateAccount(int userId, string userName);

        BaseReturnDataModel<string> GetForwardGameUrl(int userId, string userName, string ip, bool isMobile);

        BaseReturnModel CreateTransferInInfo(int userID, string userName, decimal amount);

        BaseReturnModel CreateTransferOutInfo(int userID, string userName, decimal amount);

        void SaveProfitlossToPlatform(List<InsertTPGameProfitlossParam> tpGameProfitlosses, Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus);
    }


    public interface IOldTPGameApiService
    {
        BaseReturnModel UpdateUserScoreFromRemote();
    }

    public interface IOldTPGameApiReadService
    {
        BaseReturnModel GetAllowCreateTransferOrderResult();
    }
}