using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
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
                    profitloss.ProfitLossType = ProfitLossTypeName.KY.Value;
                    profitloss.ProfitLossMoney = betLog.betAmount;
                    profitloss.WinMoney = betLog.winOrLoss;
                    profitloss.PrizeMoney = betLog.betAmount + betLog.winOrLoss;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PalyID = betLog.betNum.ToString();
                    profitloss.GameType = ABGameType.GetSingle(betLog.gameType)?.Name;
                    profitloss.BetTime = DateTime.Parse(betLog.betTime);
                    profitloss.AllBetMoney = betLog.betAmount;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.HighestParentRebateMoney = 0; //直屬抽水
                    profitloss.GrandParentRebateMoney = betLog.betAmount * (decimal)0.001;   //上上級抽水
                    profitloss.ParentRebateMoney = betLog.betAmount * (decimal)0.002;        //上級抽水
                    profitloss.SelfRebateMoney = betLog.betAmount * (decimal)0.003;          //返水

                    if (betLog.winOrLoss > 0)
                    {
                        profitloss.IsWin = 1;
                    }
                    else if (betLog.winOrLoss < 0)
                    {
                        profitloss.IsWin = 0;
                    }
                    else
                    {
                        profitloss.IsWin = -1; //和局
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

        protected override string GetCustomizeMemo(ABEBBetLog betLog)
        {
            var strMemo = new StringBuilder();

            if (betLog.gameType > 0)
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoGameType, ABGameType.GetSingle(betLog.gameType)?.Name));
            }

            if (betLog.gameRoundId > 0)
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoGameRoundId, betLog.gameRoundId));
            }

            if (!string.IsNullOrEmpty(betLog.tableName))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoTableName, betLog.tableName));
            }

            if (betLog.betType > 0)
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoBetType, ABBetType.GetSingle(betLog.betType)?.Name));
            }

            if (betLog.betMethod > 0)
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoBetMethod, ABBetMethod.GetSingle(betLog.betMethod)?.Name));
            }

            if (betLog.betNum > 0)
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.ABMemoBetNum, betLog.betNum));
            }

            return strMemo.ToString();
        }

        protected override bool IsToLowerTPGameAccount => true;
    }
}
