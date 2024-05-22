using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
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
                        Provider = "SGWIN_LOTTERY",
                        GameId = "imlotto30001",
                        GameName = "11X5JSC",
                        ChineseGameName = "极速11选5",
                        GameNo = DateTime.Now.ToFormatDateTimeStringWithoutSymbol(),
                        PlayerId = playerId,
                        ProviderPlayerId = $"im0ts_{playerId}",
                        Currency = "CNY",
                        Tray = "",
                        BetOn = "总和大小",
                        BetType = "总和大",
                        BetDetails = "",
                        Odds = "1.988",
                        BetAmount = 50,
                        ValidBet = 50,
                        PlayerWinLoss = 0,
                        LossPrize = 0,
                        Tips = 0,
                        CommissionRate = 0,
                        Status = "Settled",
                        WinLoss = -50,
                        BetId = betId.ToString(),
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        ResultDate = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                        DateCreated = DateTime.Now.ToFormatDateTimeString(),
                        Platform = "Mobile",
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

        public static DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string amountText = tpGameMoneyInfo.Amount.ToString("0.####");

            var request = new IMTransferRequestModel
            {
                MerchantCode = IMSGAppSettings.Instance.MerchantCode,
                ProductWallet = IMSGAppSettings.Instance.ProductWallet,
                PlayerId = tpGameAccount,
                Amount = amountText,
                TransactionId = tpGameMoneyInfo.OrderID,
            };

            var webRequestParam = new WebRequestParam
            {
                Url = $"http://test.transfer.{PlatformProduct.IMSG.Value}",
                Body = request.ToJsonString(),
            };

            string apiResult = new IMTransferResponseModel
            {
                Code = "510",
                Status = "Declined",
                Message = "Insufficient amount."
            }.ToJsonString();

            return new DetailRequestAndResponse(webRequestParam, apiResult);
        }
    }
}