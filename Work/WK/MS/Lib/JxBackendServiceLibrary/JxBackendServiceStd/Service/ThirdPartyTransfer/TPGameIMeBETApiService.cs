using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMeBETApiService : TPGameIMOneApiService
    {
        public TPGameIMeBETApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMeBET;

        public override IIMOneAppSetting AppSetting => _gameAppSettingService.Value.GetIMeBETAppSetting();
    }

    [MockService]
    public class TPGameIMeBETApiMockService : ITPGameApiService, ITPGameApiReadService
    {
        public PlatformProduct Product => PlatformProduct.IMeBET;

        public bool IsWriteRemoteContentToOtherMerchant => throw new NotImplementedException();

        public void WriteRemoteContentToOtherMerchant(BaseReturnDataModel<RequestAndResponse> requestAndResponseResult)
        {
            throw new NotImplementedException();
        }

        public BaseReturnModel CheckOrCreateAccount(int userId)
        {
            return new BaseReturnModel(ReturnCode.TestingEnvironmentIsNoPermission);
        }

        public decimal ComputeAdmissionBetMoney(ComputeAdmissionBetMoneyParam computAdmissionBetAmountParam)
        {
            throw new NotImplementedException();
        }

        public BaseReturnDataModel<decimal> CreateAllAmountTransferOutInfo(TPGameTranfserOutParam param, out bool isAllAmount)
        {
            isAllAmount = false;

            return new BaseReturnDataModel<decimal>(ReturnCode.TestingEnvironmentIsNoPermission);
        }

        public BaseReturnModel CreateTransferInInfo(TPGameTranfserParam param)
        {
            return new BaseReturnModel(ReturnCode.TestingEnvironmentIsNoPermission);
        }

        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out decimal actuallyAmount, out string moneyId)
        {
            moneyId = string.Empty;
            actuallyAmount = 0;

            return new BaseReturnModel(ReturnCode.TestingEnvironmentIsNoPermission);
        }

        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out string moneyId)
        {
            moneyId = string.Empty;

            return new BaseReturnModel(ReturnCode.TestingEnvironmentIsNoPermission);
        }

        public BaseReturnModel GetAllowCreateTransferOrderResult()
        {
            throw new NotImplementedException();
        }

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlParam param)
        {
            return new BaseReturnDataModel<TPGameOpenParam>(
                ReturnCode.Success,
                new TPGameOpenParam()
                {
                    OpenGameModeValue = OpenGameMode.Redirect.Value,
                    Url = GetLoginApiResult(param).DataModel
                });
        }

        public BaseReturnDataModel<string> GetLoginApiResult(ForwardGameUrlParam param)
        {
            return new BaseReturnDataModel<string>(ReturnCode.Success, $"https://www.google.com/search?q={Product.Value}");
        }

        public BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            throw new NotImplementedException();
        }

        public BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken)
        {
            throw new NotImplementedException();
        }

        public BaseReturnDataModel<UserScore> GetRemoteUserScore(IInvocationUserParam invocationUserParam, bool isRetry)
        {
            return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = 99999m, FreezeScores = 0m });
        }

        public List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            throw new NotImplementedException();
        }

        public List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo()
        {
            throw new NotImplementedException();
        }

        public List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo()
        {
            throw new NotImplementedException();
        }

        public List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts)
        {
            throw new NotImplementedException();
        }

        public void KickUser(int userId)
        {
            throw new NotImplementedException();
        }

        public void SaveMultipleProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam)
        {
            throw new NotImplementedException();
        }

        public void RecheckProcessingOrders(object state)
        {
            throw new NotImplementedException();
        }

        public void SaveProfitlossToPlatform(List<InsertTPGameProfitlossParam> tpGameProfitlosses, Func<List<SaveLocalBetLogParam>, bool> updateSQLiteToSavedStatus)
        {
            throw new NotImplementedException();
        }

        public void SaveProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam)
        {
            throw new NotImplementedException();
        }

        public void SendTransferMessage(int userId, decimal amount, string summary, int delaySendSecond)
        {
            throw new NotImplementedException();
        }

        public void TransferIn(object state)
        {
            throw new NotImplementedException();
        }

        public void TransferOut(object state)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, UserScore> GetUserScoreMap(List<int> userIds)
        {
            throw new NotImplementedException();
        }

        public void StartDequeueUpdateTPGameUserScoreJob()
        {
            throw new NotImplementedException();
        }
    }
}