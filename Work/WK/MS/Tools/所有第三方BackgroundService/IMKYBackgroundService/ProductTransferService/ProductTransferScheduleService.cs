using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<IMKYBetLog>
    {
        protected override PlatformProduct Product => PlatformProduct.IMKY;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override BaseReturnDataModel<List<IMKYBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<IMKYBetLog>();

            var responseModel = apiResult.Deserialize<IMKYBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.Code == IMOneResponseCode.Success)
                {
                    betLogs.AddRange(responseModel.Result);
                }
                else if (responseModel.Code == IMOneResponseCode.NoDataFound)
                {
                    //do nothing
                }
                else
                {
                    errorMsg = responseModel.Message;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<IMKYBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<IMKYBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<IMKYBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();

            foreach (IMKYBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.BetAmount, betLog.BetAmount);
                    profitloss.ProfitLossTime = DateTime.Parse(betLog.SettlementDate);
                    profitloss.WinMoney = betLog.WinLoss;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.BetId.ToString();
                    profitloss.GameType = betLog.ChineseGameName;
                    profitloss.BetTime = DateTime.Parse(betLog.BetDate);
                    profitloss.KeyId = betLog.KeyId;

                    if (betLog.WinLoss > 0)
                    {
                        profitloss.BetResultType = 1;
                    }
                    else if (betLog.WinLoss < 0)
                    {
                        profitloss.BetResultType = 0;
                    }
                    else
                    {
                        profitloss.BetResultType = -1; //和局
                    }
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }

            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(IMKYBetLog betLog)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            if (!betLog.ChineseGameName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMKYGameType),
                    Args = new List<string>() { betLog.ChineseGameName }
                });
            }

            if (!betLog.BetId.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMKYBetId),
                    Args = new List<string>() { betLog.BetId }
                });
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.IMKYBetAmount),
                Args = new List<string>() { betLog.BetAmount.ToString() }
            });

            if (!betLog.BetDate.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMKYBetDate),
                    Args = new List<string>() { DateTime.Parse(betLog.BetDate).ToFormatDateTimeString() }
                });
            }

            if (!betLog.SettlementDate.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMKYSettlementDate),
                    Args = new List<string>() { DateTime.Parse(betLog.SettlementDate).ToFormatDateTimeString() }
                });
            }

            return localizationParam;
        }
    }
}