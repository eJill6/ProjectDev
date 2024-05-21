using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseIMLotteryTransferScheduleService : BaseTransferScheduleService<IMLotteryBetLog>
    {
        private static readonly string _imUnitStr = "Unit=";

        private static readonly string _imMultipleStr = "Multiple=";

        private static readonly string _imCountStr = "Count=";

        private static readonly string _imPositionStr = "Position=";

        private static readonly string _imNumberStr = "Number=";

        protected override BaseReturnDataModel<List<IMLotteryBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<IMLotteryBetLog>();

            var responseModel = apiResult.Deserialize<IMLotteryBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.Code == IMOneResponseCode.Success)
                {
                    betLogs = responseModel.Result.Where(w => w.Status == IMLotteryOrderStatus.Settled).ToList();
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
                    return new BaseReturnDataModel<List<IMLotteryBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<IMLotteryBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);

            //DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameIMOneApiService.MaxSearchRangeMinutes);

            //if (requestAndResponse != null && !requestAndResponse.RequestBody.IsNullOrEmpty())
            //{
            //    lastCrawlStartDate = GameIMUtil.ToBetRecordDateTime(requestAndResponse.RequestBody.Deserialize<IMGetBetLogRequestModel>().StartDate);
            //}
            //else if (!lastSearchToken.IsNullOrEmpty())
            //{
            //    if (long.TryParse(lastSearchToken, out long timeStamp))
            //    {
            //        lastCrawlStartDate = timeStamp.ToDateTime();
            //    }
            //}

            //DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameIMOneApiService.MaxSearchRangeMinutes);

            //if (nextStartSearchDate > DateTime.Now)
            //{
            //    nextStartSearchDate = DateTime.Now;
            //}

            //return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<IMLotteryBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (IMLotteryBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (betLog.Status == IMLotteryOrderStatus.Settled && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.ValidBet, betLog.BetAmount);

                    if (Product == PlatformProduct.IMVR)
                    {
                        profitloss.WinMoney = betLog.PlayerWinLoss;
                    }
                    else
                    {
                        profitloss.WinMoney = betLog.WinLoss;
                    }

                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.BetId;
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

                    if (!betLog.SettlementDate.IsNullOrEmpty())
                    {
                        profitloss.ProfitLossTime = DateTime.Parse(betLog.SettlementDate);
                    }
                    else
                    {
                        profitloss.ProfitLossTime = DateTime.Parse(betLog.ResultDate);
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

        protected override LocalizationParam GetCustomizeMemo(IMLotteryBetLog betLog)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            if (!string.IsNullOrEmpty(betLog.BetId))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetId),
                    Args = new List<string>() { betLog.BetId }
                });
            }

            if (!string.IsNullOrEmpty(betLog.GameName))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoGameName),
                    Args = new List<string>() { betLog.GameName }
                });
            }

            if (!string.IsNullOrEmpty(betLog.GameNo))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoGameNo),
                    Args = new List<string>() { betLog.GameNo }
                });
            }

            if (!string.IsNullOrEmpty(betLog.BetOn))
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoBetOn),
                    Args = new List<string>() { betLog.BetOn }
                });
            }

            //Position=万,个; Number=虎;
            string[] betTypes = betLog.BetType.Split(';');

            foreach (var betType in betTypes)
            {
                if (betType.IndexOf(_imPositionStr) >= 0)
                {
                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoPosition),
                        Args = new List<string>() { betType.Replace(_imPositionStr, string.Empty) }
                    });
                }
                else if (betType.IndexOf(_imNumberStr) >= 0)
                {
                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoNumber),
                        Args = new List<string>() { betType.Replace(_imNumberStr, string.Empty) }
                    });
                }
            }

            string[] details = betLog.BetDetails.Split(';');
            string detail = details.FirstOrDefault(y => y.IndexOf(_imUnitStr) > 0);

            if (!detail.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoUnit),
                    Args = new List<string>() { detail.Replace(_imUnitStr, string.Empty) }
                });
            }

            detail = details.FirstOrDefault(y => y.IndexOf(_imMultipleStr) > 0);

            if (!detail.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoMultiple),
                    Args = new List<string>() { detail.Replace(_imMultipleStr, string.Empty) }
                });
            }

            detail = details.FirstOrDefault(y => y.IndexOf(_imCountStr) > 0);

            if (!detail.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.IMMemoCount),
                    Args = new List<string>() { detail.Replace(_imCountStr, string.Empty) }
                });
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.SomeOdds),
                Args = new List<string>() { betLog.Odds }
            });

            return localizationParam;
        }
    }
}