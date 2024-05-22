using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AWCSP;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<AWCSPBetLog>
    {
        public ProductTransferScheduleService()
        {
        }

        protected override PlatformProduct Product => PlatformProduct.AWCSP;

        protected override JxApplication Application => JxApplication.AWCSPTransferService;

        protected override bool IsToLowerTPGameAccount => true;

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override BaseReturnDataModel<List<AWCSPBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<AWCSPBetLog>();

            var responseModel = apiResult.Deserialize<AWCSPBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.IsSuccess && responseModel.Transactions.Any())
                {
                    betLogs.AddRange(responseModel.Transactions);
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<AWCSPBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<AWCSPBetLog>>(errorMsg);
                }
            }

            throw new InvalidCastException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<AWCSPBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();
            IEnumerable<ThirdPartySubGameCodes> thirdPartySubGameCodes = ThirdPartySubGameCodes.GetAll().Where(x => x.PlatformProduct == PlatformProduct.AWCSP);

            foreach (AWCSPBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    DateTime profitLossTime = DateTime.Parse(betLog.UpdateTime);
                    DateTime betTime = DateTime.Parse(betLog.BetTime);
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.Turnover, betLog.BetAmount);
                    profitloss.ProfitLossTime = profitLossTime;
                    profitloss.WinMoney = betLog.RealWinAmount - betLog.RealBetAmount;
                    profitloss.PlayID = betLog.KeyId;
                    profitloss.GameType = thirdPartySubGameCodes.Single(x => x.RemoteGameCode == betLog.Platform).Value;
                    profitloss.BetTime = betTime;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.Memo = betLog.Memo;
                    profitloss.BetResultType = profitloss.WinMoney.ToBetResultType().Value;
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }

            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(AWCSPBetLog betLog)
        {
            DateTime profitLossTime = DateTime.Parse(betLog.UpdateTime);
            DateTime betTime = DateTime.Parse(betLog.BetTime);

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourcePropertyName = nameof(DBContentElement.AWCSPMemo),
                        Args = new List<string>()
                        {
                            AWCSPPlatform.GetName(betLog.Platform),
                            betLog.Platform,
                            betLog.GameName,
                            betLog.PlatformTxId,
                            betLog.BetType,
                            betLog.BetAmount.ToString(),
                            betTime.ToFormatDateTimeString(),
                            profitLossTime.ToFormatDateTimeString()
                        }
                    }
                }
            };

            return localizationParam;
        }
    }
}