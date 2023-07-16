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
            var betLogs = new List<ABEBBetLog>();

            var responseModel = apiResult.Deserialize<ABBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.error_code == ABResponseCode.Success)
                {
                    betLogs = responseModel.histories.Where(w => w.status == ABOrderStatus.Settled && w.state == 0).ToList();
                }
                else
                {
                    errorMsg = responseModel.message;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<ABEBBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<ABEBBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);

            //DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameABEBApiService.MaxSearchRangeMinutes);

            //if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            //{
            //    lastCrawlStartDate = DateTime.Parse(requestAndResponse.ResponseContent.Deserialize<ABBetLogRequestModel>().startTime);
            //}
            //else if (!lastSearchToken.IsNullOrEmpty())
            //{
            //    if (long.TryParse(lastSearchToken, out long timeStamp))
            //    {
            //        lastCrawlStartDate = timeStamp.ToDateTime();
            //    }
            //}

            //DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameABEBApiService.MaxSearchRangeMinutes);

            //if (nextStartSearchDate > DateTime.Now)
            //{
            //    nextStartSearchDate = DateTime.Now;
            //}

            //return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<ABEBBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (ABEBBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (betLog.status == ABOrderStatus.Settled && betLog.state == 0 && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.validAmount, betLog.betAmount);
                    profitloss.WinMoney = betLog.winOrLoss;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.betNum.ToString();
                    profitloss.GameType = ABGameType.GetSingle(betLog.gameType)?.Name;
                    profitloss.BetTime = DateTime.Parse(betLog.betTime);
                    profitloss.KeyId = betLog.KeyId;

                    if (betLog.winOrLoss > 0)
                    {
                        profitloss.BetResultType = 1;
                    }
                    else if (betLog.winOrLoss < 0)
                    {
                        profitloss.BetResultType = 0;
                    }
                    else
                    {
                        profitloss.BetResultType = -1; //和局
                    }

                    if (!betLog.gameRoundEndTime.IsNullOrEmpty())
                    {
                        profitloss.ProfitLossTime = DateTime.Parse(betLog.gameRoundEndTime);
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

            if (betLog.gameType > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoGameType),
                    Args = new List<string>() { ABGameType.GetSingle(betLog.gameType)?.Name }
                });
            }

            if (betLog.gameRoundId > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoGameRoundId),
                    Args = new List<string>() { betLog.gameRoundId.ToString() }
                });
            }

            if (!betLog.tableName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoTableName),
                    Args = new List<string>() { betLog.tableName }
                });
            }

            if (betLog.betType > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetType),
                    Args = new List<string>() { ABBetType.GetSingle(betLog.betType)?.Name }
                });
            }

            if (betLog.betMethod > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetMethod),
                    Args = new List<string>() { ABBetMethod.GetSingle(betLog.betMethod)?.Name }
                });
            }

            if (betLog.betNum > 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.ABMemoBetNum),
                    Args = new List<string>() { betLog.betNum.ToString() }
                });
            }

            return localizationParam;
        }

        protected override bool IsToLowerTPGameAccount => true;
    }
}