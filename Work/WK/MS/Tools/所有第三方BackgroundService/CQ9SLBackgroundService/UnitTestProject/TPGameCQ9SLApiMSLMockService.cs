using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.CQ9SL;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGameCQ9SLApiMSLMockService : TPGameCQ9SLApiMSLService
    {
        public TPGameCQ9SLApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var response = new CQ9Response<CQ9BetLogViewModel>()
            {
                Status = new CQ9Status()
                {
                    Code = CQ9Code.Success.Value,
                },
                Data = new CQ9BetLogViewModel()
                {
                    TotalSize = 10,
                    Data = new List<CQ9BetLog>() { }
                }
            };

            TestUtil.DoPlayerJobs(
                recordCount: 10,
                (playerId, id) =>
                {
                    response.Data.Data.Add(new CQ9BetLog()
                    {
                        Account = playerId,
                        Balance = 10,
                        ValidBet = 10,
                        Bet = 10,
                        Win = -10,
                        GameType = CQ9GameType.Arcade.Value,
                        Round = id.ToString(),
                        BetTime = DateTimeOffset.UtcNow.AddMinutes(-2),
                        CreateTime = DateTimeOffset.UtcNow,
                    });
                });

            return new BaseReturnDataModel<RequestAndResponse>(
                ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = "1",
                    ResponseContent = response.ToJsonString()
                });
        }
    }
}