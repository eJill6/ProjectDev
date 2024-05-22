using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGameBTISApiMSLMockService : TPGameBTISApiService
    {
        public TPGameBTISApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var btisBetLogs = new List<BTISBetLog>();

            TestUtil.DoPlayerJobs(                
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    var btisBetLog = new BTISBetLog()
                    {
                        MerchantCustomerID = playerId,
                        PurchaseID = betId.ToString(),
                        PL = -10,
                        Memo = "test",
                        Selections = new List<Selection>(),
                        CreationDate = DateTime.Now.AddHours(-1),
                        Status = BTISBettingHistoryStatus.Lost.Value,
                        BetSettledDate = DateTime.Now,
                        TotalStake = 100,
                        OddsStyleOfUser = BTISHandicap.European.Value,
                        OddsInUserStyle = "1",
                        BetTypeId = BTISWagerType.QABet.Value,
                    };

                    btisBetLogs.Add(btisBetLog);
                });

            var response = new BTISBettingHistoryResponse()
            {
                ErrorCode = BTISDataErrorCode.Success.Value,
                CurrentPage = 1,
                TotalPages = 1,
                LastUpdateDate = DateTime.Now,
                Bets = btisBetLogs
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = response.ToJsonString()
            });
        }
    }
}