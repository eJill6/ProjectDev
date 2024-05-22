using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGamePGSLApiMSLMockService : TPGamePGSLApiMSLService
    {
        public TPGamePGSLApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var betLogs = new List<PGSLBetLog>();

            TestUtil.DoPlayerJobs(
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betLogs.Add(new PGSLBetLog()
                    {
                        playerName = playerId,
                        betType = PGBetTypes.TrueGame.Value,
                        betTime = DateTime.Now.AddMinutes(-5).ToUnixOfTime(),
                        betEndTime = DateTime.Now.ToUnixOfTime(),
                        betAmount = 20,
                        winAmount = 0,
                        Memo = "test",
                        betId = betId,
                        gameId = PGGameIDTypes.PGGameIDTypes000.Value,
                    });
                });

            var response = new PGBetLogResponseModel()
            {
                data = betLogs
            };

            return new BaseReturnDataModel<RequestAndResponse>()
            {
                DataModel = new RequestAndResponse()
                {
                    RequestBody = DateTime.Now.ToUnixOfTime().ToString(),
                    ResponseContent = response.ToJsonString()
                }
            };
        }
    }
}