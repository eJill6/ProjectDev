using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

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
            return string.Empty;
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
            return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                new UserScore
                {
                    AvailableScores = 100,
                    FreezeScores = 0
                });
        }
    }

    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var evBetLogModels = new List<EVBetLogModel>();

            TestUtil.DoPlayerJobs(                
                recordCount: 10000,
                job: (playerId, betId) => 
                {
                    var evBetLogModel = new EVBetLogModel()
                    {
                        MemberAccount = playerId,
                        BetAmount = 100,
                        Payoff = 100,
                        Commissionable = 1,
                        BetId = betId.ToString(),
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
                });

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