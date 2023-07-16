using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendServiceNF.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseEVTransferScheduleService : BaseTransferScheduleService<EVEBBetLog>
    {
        protected override BaseReturnDataModel<List<EVEBBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<EVEBBetLog>();

            var responseModel = apiResult.Deserialize<EVBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.ErrorCode == EVResponseCode.Success)
                {
                    if (responseModel.Data.Any())
                    {
                        //因回傳資料多一層RawData，故需要轉換
                        responseModel.Data.ForEach(x =>
                        {
                            EVEBBetLog betLog = new EVEBBetLog();

                            betLog = x.RawData.CastByJson<EVEBBetLog>();
                            betLog.BetId = x.BetId;
                            betLog.MemberAccount = x.MemberAccount;
                            betLog.WagersTime = x.WagersTime;
                            betLog.BetAmount = x.BetAmount;
                            betLog.Payoff = x.Payoff;
                            betLog.Commissionable = x.Commissionable;
                            betLog.UpdateTime = x.UpdateTime;

                            betLogs.Add(betLog);
                        });
                    }
                    //else not found data
                }
                else
                {
                    errorMsg = responseModel.ErrorMessage;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<EVEBBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<EVEBBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);

            //DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameEVEBApiService.MaxSearchRangeMinutes);

            //if (!lastSearchToken.IsNullOrEmpty())
            //{
            //    if (long.TryParse(lastSearchToken, out long timeStamp))
            //    {
            //        lastCrawlStartDate = timeStamp.ToDateTime();
            //    }
            //}

            //DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameEVEBApiService.MaxSearchRangeMinutes);

            //if (nextStartSearchDate > DateTime.Now)
            //{
            //    nextStartSearchDate = DateTime.Now;
            //}

            //return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<EVEBBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (EVEBBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.ProfitLossTime = betLog.GameSettledAt.AddHours(8);//時間轉換
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.Commissionable, betLog.BetAmount);
                    profitloss.WinMoney = betLog.Payoff;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.BetId.ToString();
                    profitloss.GameType = betLog.GameType;
                    profitloss.BetTime = betLog.WagersTime.AddHours(12);//美東時間轉換
                    profitloss.KeyId = betLog.KeyId;

                    if (profitloss.WinMoney >= 0)
                    {
                        profitloss.BetResultType = 1;
                    }
                    else
                    {
                        profitloss.BetResultType = 0;
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

        protected override LocalizationParam GetCustomizeMemo(EVEBBetLog betLog)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>() {
                    new LocalizationSentence() {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.EVMemoGameID),
                        Args = new List<string>() { betLog.GameId }
                    },
                    new LocalizationSentence()
                    {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.EVMemoGameType),
                        Args = new List<string>() { betLog.GameType }
                    },
                    new LocalizationSentence()
                    {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.EVMemoTableID),
                        Args = new List<string>() { betLog.TableId }
                    },
                    new LocalizationSentence()
                    {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.EVMemoTableName),
                        Args = new List<string>() { betLog.TableName }
                    },
                    new LocalizationSentence() {
                        ResourceName = resourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.EVMemoBetID),
                        Args = new List<string>() { betLog.BetId }
                    },
                }
            };

            return localizationParam;
        }
    }
}