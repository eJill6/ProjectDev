using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;

namespace JxBackendServiceN6.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseABTransferScheduleService : BaseTransferScheduleService<ABEBBetLog>
    {
        protected override BaseReturnDataModel<List<ABEBBetLog>> ConvertToBetLogs(string apiResult)
        {
            string[] responseContents = apiResult.Deserialize<string[]>();
            var betLogs = new List<ABEBBetLog>();

            foreach (string responseContent in responseContents)
            {
                var responseModel = responseContent.Deserialize<ABBetLogResponseModel>();

                if (responseModel.IsSuccess)
                {
                    if (responseModel.Data != null && responseModel.Data.List.Count > 0)
                    {
                        betLogs.AddRange(responseModel.Data.List.Where(w => w.Status == ABBetLogStatus.Settled).ToList());
                    }
                    else
                    {
                        return new BaseReturnDataModel<List<ABEBBetLog>>(responseModel.Message);
                    }
                }
            }

            return new BaseReturnDataModel<List<ABEBBetLog>>(ReturnCode.Success, betLogs);
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<ABEBBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();

            foreach (ABEBBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (betLog.Status == ABBetLogStatus.Settled && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.ValidAmount, betLog.BetAmount);
                    profitloss.WinMoney = betLog.WinOrLossAmount;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.BetNum.ToString();
                    profitloss.GameType = ABGameType.GetGameTypeName(betLog.GameType);
                    profitloss.BetTime = DateTime.Parse(betLog.BetTime);
                    profitloss.KeyId = betLog.KeyId;

                    BetResultType betResultType = betLog.WinOrLossAmount.ToBetResultType();
                    profitloss.BetResultType = betResultType.Value;

                    if (!betLog.GameRoundEndTime.IsNullOrEmpty())
                    {
                        profitloss.ProfitLossTime = DateTime.Parse(betLog.GameRoundEndTime);
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

        protected override LocalizationParam GetCustomizeMemo(ABEBBetLog betLog)
        {
            var localizationParam = new LocalizationParam
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            string resourceName = typeof(ThirdPartyGameElement).FullName;

            if (betLog.GameType > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoGameType),
                    Args = new List<string>() { ABGameType.GetGameTypeName(betLog.GameType) }
                });
            }

            if (betLog.GameRoundId > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoGameRoundId),
                    Args = new List<string>() { betLog.GameRoundId.ToString() }
                });
            }

            if (!betLog.TableName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoTableName),
                    Args = new List<string>() { betLog.TableName }
                });
            }

            if (betLog.BetType > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetType),
                    Args = new List<string>() { ABBetType.GetBetTypeName(betLog.BetType) }
                });
            }

            if (betLog.BetMethod > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetMethod),
                    Args = new List<string>() { ABBetMethod.GetBetMethod(betLog.BetMethod) }
                });
            }

            if (betLog.BetNum > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetNum),
                    Args = new List<string>() { betLog.BetNum.ToString() }
                });
            }

            return localizationParam;
        }

        protected override bool IsToLowerTPGameAccount => true;
    }
}