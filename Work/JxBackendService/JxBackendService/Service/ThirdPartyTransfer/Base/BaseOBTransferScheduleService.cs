using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseOBTransferScheduleService : BaseTransferScheduleService<OBFIBetLog>
    {
        protected override BaseReturnDataModel<List<OBFIBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<OBFIBetLog>();

            var responseModel = apiResult.Deserialize<OBFIBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.code == OBOpreationOrderStatus.Success)
                {
                    if (responseModel.data != null && responseModel.data.list != null)
                    {
                        betLogs = responseModel.data.list;
                    }
                    //else not found data
                }
                else
                {
                    errorMsg = responseModel.msg;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<OBFIBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<OBFIBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);

            //DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameOBFIApiService.MaxSearchRangeMinutes);

            //if (!lastSearchToken.IsNullOrEmpty())
            //{
            //    if (long.TryParse(lastSearchToken, out long timeStamp))
            //    {
            //        lastCrawlStartDate = timeStamp.ToDateTime();
            //    }
            //}

            //DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameOBFIApiService.MaxSearchRangeMinutes);

            ////跨日時 要特別處理 從00:01開始 因為後面會-1
            //if (lastCrawlStartDate.Day != nextStartSearchDate.Day)
            //{
            //    nextStartSearchDate = nextStartSearchDate.Date.AddMinutes(1);
            //}

            //if (nextStartSearchDate > DateTime.Now)
            //{
            //    nextStartSearchDate = DateTime.Now;
            //}

            //return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<OBFIBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (OBFIBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    //bc 有效投注,tb 總投注金額,單位 分
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.ProfitLossTime = DateTimeUtil.ToDateTime((long)betLog.et * 1000);
                    profitloss.ProfitLossType = ProfitLossTypeName.KY.Value;
                    profitloss.ProfitLossMoney = (decimal)betLog.tb / 100;
                    profitloss.WinMoney = (decimal)betLog.mw / 100;
                    profitloss.PrizeMoney = ((decimal)betLog.tb + (decimal)betLog.mw) / 100;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PalyID = betLog.bi.ToString();
                    profitloss.GameType = betLog.gn;
                    profitloss.BetTime = DateTimeUtil.ToDateTime((long)betLog.st * 1000);
                    profitloss.AllBetMoney = (decimal)betLog.tb / 100;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.HighestParentRebateMoney = 0; //直屬抽水
                    profitloss.GrandParentRebateMoney = ((decimal)betLog.tb / 100) * (decimal)0.001;   //上上級抽水
                    profitloss.ParentRebateMoney = ((decimal)betLog.tb / 100) * (decimal)0.002;        //上級抽水
                    profitloss.SelfRebateMoney = ((decimal)betLog.tb / 100) * (decimal)0.003;          //返水

                    if (profitloss.WinMoney >= 0)
                    {
                        profitloss.IsWin = 1;
                    }
                    else
                    {
                        profitloss.IsWin = 0;
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

        protected override string GetCustomizeMemo(OBFIBetLog betLog)
        {
            var strMemo = new StringBuilder();
            if (!string.IsNullOrEmpty(betLog.gn))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.OBMemoGameName, betLog.gn));
            }
            if (!string.IsNullOrEmpty(betLog.gr))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.OBMemoGameRoom, betLog.gr));
            }
            strMemo.Append(string.Format(ThirdPartyGameElement.OBMemoGameTable, betLog.gd));
            strMemo.Append(string.Format(ThirdPartyGameElement.OBMemoId, betLog.bi));
            if (!string.IsNullOrEmpty(betLog.cn))
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.OBMemoPart, betLog.cn));
            }
            return strMemo.ToString();
        }

    }
}
