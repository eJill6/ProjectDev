using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;
using UnitTestN6;

namespace UnitTestProject
{
    public class TPGameIMKYApiMSLMockService : TPGameIMKYApiService
    {
        public TPGameIMKYApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //return base.GetRemoteBetLogApiResult(lastSearchToken);
            return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }
    }

    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var betlogs = new List<IMKYBetLog>();

            TestUtil.DoPlayerJobs(
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betlogs.Add(new IMKYBetLog
                    {
                        Provider = "KYG_BG",
                        GameId = "IMBG20004",
                        GameName = "Banker Bull Bull",
                        ChineseGameName = "抢庄牛牛",
                        BetId = betId.ToString(),
                        RoundId = betId.ToString(),
                        PlayerId = playerId,
                        ProviderPlayerId = $"IM0TS_{playerId}",
                        Currency = "CNY",
                        BetAmount = 4,
                        ValidBet = 4,
                        WinLoss = 4,
                        Commission = 0,
                        Bonus = 0,
                        ProviderBonus = 0,
                        Status = "Settled",
                        Platform = "N/A",
                        Remarks = string.Empty,
                        DateCreated = DateTime.Now.ToFormatDateTimeString(),
                        GameDate = DateTime.Now.ToFormatDateTimeString(),
                        GameEndDate = DateTime.Now.ToFormatDateTimeString(),
                        LastUpdatedDate = DateTime.Now.ToFormatDateTimeString(),
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                    });
                });

            var model = new IMKYBetLogResponseModel()
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
                MerchantCode = IMKYSharedAppSetting.Instance.MerchantCode,
                ProductWallet = IMKYSharedAppSetting.Instance.ProductWallet,
                PlayerId = tpGameAccount,
                Amount = amountText,
                TransactionId = tpGameMoneyInfo.OrderID,
            };

            var webRequestParam = new WebRequestParam
            {
                Url = $"http://test.transfer.{PlatformProduct.IMKY.Value}",
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