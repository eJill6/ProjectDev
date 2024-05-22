using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

namespace UnitTestProject
{
    [MockService]
    public class TPGameABEBApiMSLMockService : TPGameABEBApiMSLService
    {
        public TPGameABEBApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var responseContents = new List<string>();
            var betLogs = new List<ABEBBetLog>();

            TestUtil.DoPlayerJobs(                
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betLogs.Add(new ABEBBetLog()
                    {
                        Player = playerId,
                        Status = ABBetLogStatus.Settled.Value,
                        WinOrLossAmount = -10,
                        Memo = "test",
                        BetNum = betId,
                        GameType = ABGameType.Type101.Value,
                        BetTime = DateTime.Now.AddMinutes(-5).ToFormatDateString(),
                        ValidAmount = 10,
                        BetAmount = 20,
                        GameRoundEndTime = DateTime.Now.ToFormatDateString()
                    });
                });

            var responseModel = new ABBetLogResponseModel()
            {
                ResultCode = ABResponseCode.Success.Value,
                Data = new ABBetLogModel()
                {
                    StartDateTime = DateTime.Now.AddMinutes(-30).ToFormatDateString(),
                    EndDateTime = DateTime.Now.AddMinutes(-5).ToFormatDateString(),
                    Total = 1,
                    PageNum = 1,
                    PageSize = 1000,
                    List = betLogs,
                }
            };

            responseContents.Add(responseModel.ToJsonString());

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success)
            {
                DataModel = new RequestAndResponse()
                {
                    RequestBody = DateTime.Now.ToUnixOfTime().ToString(),
                    ResponseContent = responseContents.ToJsonString()
                }
            };
        }
    }
}