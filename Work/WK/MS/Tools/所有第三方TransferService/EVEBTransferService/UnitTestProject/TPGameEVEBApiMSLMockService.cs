using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameEVEBApiMSLMockService : TPGameEVEBApiMSLService
    {
        public TPGameEVEBApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            return base.GetRemoteCreateAccountApiResult(param);
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            return base.GetRemoteUserScoreApiResult(createRemoteAccountParam);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            return base.GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            return base.GetRemoteLoginApiResult(tpGameRemoteLoginParam);
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            return base.GetUserScoreReturnModel(apiResult);
        }
    }

    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.EVEBTransferService).AccountPrefixCode;
            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
            };

            var evBetLogModels = new List<EVBetLogModel>();

            foreach (string playerId in playerIds)
            {
                var evBetLogModel = new EVBetLogModel()
                {
                    MemberAccount = playerId,
                    BetAmount = 100,
                    Payoff = 100,
                    Commissionable = 1,
                    BetId = DateTime.Now.ToUnixOfTime().ToString(),
                    WagersTime = DateTime.Now.AddHours(-12),
                    UpdateTime = DateTime.Now,
                    RawData = new EVEBRowDataBetLog()
                    {
                        GameSettledAt = DateTime.UtcNow,
                        Memo = "test",
                        GameType = "gggg",
                        GameWager = 1,
                        GamePayout = 1,
                        BetCode = "1",
                        BetDescription = "1",
                        BetPayout = 100,
                        BetPlacedOn = DateTime.Now,
                        BetStake = 100,
                        BetTransactionId = "1",
                        DealerName = "1",
                        DealerUid = "1",
                        GameCurrency = "1",
                        GameId = "1",
                        GameResult = "1",
                        GameStartedAt = DateTime.Now,
                        GameStatus = "1",
                    }
                };

                evBetLogModels.Add(evBetLogModel);
            }

            var responseModel = new EVBetLogResponseModel()
            {
                ErrorCode = EVResponseCode.Success.Value,
                Data = evBetLogModels
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "0",
                ResponseContent = responseModel.ToJsonString()
            });
        }
    }
}