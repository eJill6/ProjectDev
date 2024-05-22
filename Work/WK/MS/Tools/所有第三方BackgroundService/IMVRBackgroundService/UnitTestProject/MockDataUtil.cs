using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel.ThirdParty;
using UnitTestN6;

namespace UnitTestProject
{
    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var betlogs = new List<IMLotteryBetLog>();

            TestUtil.DoPlayerJobs(
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betlogs.Add(new IMLotteryBetLog
                    {
                        Provider = "VR_LOTTERY",
                        GameId = "imlotto20001",
                        GameName = "Venus 1.5 Lottery",
                        ChineseGameName = "VR 金星 1.5 分彩",
                        GameNo = DateTime.Now.ToFormatDateTimeStringWithoutSymbol(),
                        PlayerId = playerId,
                        ProviderPlayerId = "AihQixPcUgmH2xe7bNo2",
                        Currency = "CNY",
                        Tray = "1980",
                        BetOn = "五星总和大小单双",
                        BetType = "Position=万,千,百,十,个;Number=总和大;",
                        BetDetails = "WinningNumber=9,5,9,5,8;Award=五星总和大小单双;PrizeNumber=总和大;Unit=1.00000;Multiple=10;Count=1;",
                        Odds = "1.97",
                        BetAmount = 10,
                        ValidBet = 10,
                        PlayerWinLoss = 9.7m,
                        LossPrize = 0,
                        Tips = 0,
                        CommissionRate = 0,
                        Status = "Settled",
                        WinLoss = 9.7m,
                        BetId = betId.ToString(),
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        ResultDate = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                        DateCreated = DateTime.Now.ToFormatDateTimeString(),
                        Platform = "N/A",
                        LastUpdatedDate = DateTime.Now.ToFormatDateTimeString(),
                    });
                });

            var model = new IMLotteryBetLogResponseModel()
            {
                Code = "0",
                Result = betlogs
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = model.ToJsonString()
            });
        }
    }
}