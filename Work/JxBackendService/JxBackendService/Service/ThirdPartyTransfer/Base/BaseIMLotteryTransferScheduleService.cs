using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
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
                    profitloss.ProfitLossType = ProfitLossTypeName.KY.Value;
                    profitloss.ProfitLossMoney = betLog.BetAmount;
                    profitloss.WinMoney = betLog.WinLoss;//VR=PlayerWinLoss;SG=WinLoss
                    profitloss.PrizeMoney = betLog.BetAmount + betLog.WinLoss;//VR：BetAmount+PlayerWinLoss SG：BetAmount+WinLoss
                    profitloss.Memo = betLog.Memo;
                    profitloss.PalyID = betLog.BetId;
                    profitloss.GameType = betLog.ChineseGameName;
                    profitloss.BetTime = DateTime.Parse(betLog.BetDate);
                    profitloss.AllBetMoney = betLog.BetAmount;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.HighestParentRebateMoney = 0; //IM彩票系列不發直屬抽水
                    profitloss.GrandParentRebateMoney = 0;   //IM彩票系列不發上上級抽水
                    profitloss.ParentRebateMoney = 0;        //IM彩票系列不發上級抽水
                    profitloss.SelfRebateMoney = 0;          //IM彩票系列不發返水

                    if (betLog.WinLoss > 0)
                    {
                        profitloss.IsWin = 1;
                    }
                    else if (betLog.WinLoss < 0)
                    {
                        profitloss.IsWin = 0;
                    }
                    else
                    {
                        profitloss.IsWin = -1; //和局
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

        protected override string GetCustomizeMemo(IMLotteryBetLog betLog)
        {
            var strMemo = new StringBuilder();

            if (!string.IsNullOrEmpty(betLog.BetId))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.IMMemoBetId, betLog.BetId));
            }

            if (!string.IsNullOrEmpty(betLog.GameName))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.IMMemoGameName, betLog.GameName));
            }

            if (!string.IsNullOrEmpty(betLog.GameNo))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.IMMemoGameNo, betLog.GameNo));
            }

            if (!string.IsNullOrEmpty(betLog.BetOn))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.IMMemoBetOn, betLog.BetOn));
            }

            string subString = betLog.BetType.Replace(_imPositionStr, ThirdPartyGameElement.IMMemoPosition).Replace(_imNumberStr, ThirdPartyGameElement.IMMemoNumber).Replace(";", ", ");
            strMemo.Append(subString);
            string[] details = betLog.BetDetails.Split(';');

            if (details.Any(y => y.IndexOf(_imUnitStr) > 0))
            {
                strMemo.Append(details.First(y => y.IndexOf(_imUnitStr) > 0).Replace(_imUnitStr, ThirdPartyGameElement.IMMemoUnit));
            }

            if (details.Any(y => y.IndexOf(_imMultipleStr) > 0))
            {
                strMemo.Append(details.First(y => y.IndexOf(_imMultipleStr) > 0).Replace(_imMultipleStr, ThirdPartyGameElement.IMMemoMultiple));
            }

            if (details.Any(y => y.IndexOf(_imCountStr) > 0))
            {
                strMemo.Append(details.First(y => y.IndexOf(_imCountStr) > 0).Replace(_imCountStr, ThirdPartyGameElement.IMMemoCount));
            }

            strMemo.Append(string.Format(ThirdPartyGameElement.IMMemoOdds, betLog.Odds));

            return strMemo.ToString();
        }
    }
}
