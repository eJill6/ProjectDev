using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BasePGTransferScheduleService : BaseTransferScheduleService<PGSLBetLog>
    {
        //private readonly int _maxSearchDaysBefore = 30;
        //private readonly string _initSearchToken = "1";

        protected override BaseReturnDataModel<List<PGSLBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<PGSLBetLog>();

            var responseModel = apiResult.Deserialize<PGBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.error == null)
                {
                    betLogs = responseModel.data.Where(x => x.betType == PGBetTypes.TrueGame.Value).ToList();
                }
                else
                {
                    errorMsg = responseModel.error.message;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<PGSLBetLog>>(ReturnCode.Success, betLogs);
                }
                else
                {
                    return new BaseReturnDataModel<List<PGSLBetLog>>(errorMsg, betLogs);
                }
            }

            throw new InvalidCastException();
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);

            ////第一次丟1
            //if (lastSearchToken.IsNullOrEmpty())
            //{
            //    return _initSearchToken;
            //}

            //var pgbetLogResponseModel = requestAndResponse.ResponseContent.Deserialize<PGBetLogResponseModel>();

            //if (pgbetLogResponseModel.data.Any())
            //{
            //    return pgbetLogResponseModel.data.Max(x => x.rowVersion).ToString();
            //}
            //else
            //{
            //    //TOKEN超過30天以上強迫更新為30天前(規格最多可查60天)
            //    lastSearchToken = ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
            //    {
            //        DateTime tokenDate = lastSearchToken.ToInt64().ToDateTime();
            //        DateTime lastSearchTokenDate = DateTime.UtcNow.AddDays(-_maxSearchDaysBefore);

            //        if (tokenDate < lastSearchTokenDate)
            //        {
            //            return lastSearchTokenDate.ToUnixOfTime().ToString();
            //        }

            //        return lastSearchToken;
            //    });

            //    return lastSearchToken;
            //}
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<PGSLBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (PGSLBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (betLog.betType == PGBetTypes.TrueGame.Value && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.ProfitLossTime = DateTimeUtil.ToDateTime(betLog.betEndTime);
                    profitloss.ProfitLossType = ProfitLossTypeName.KY.Value;
                    profitloss.ProfitLossMoney = betLog.betAmount;
                    profitloss.WinMoney = betLog.winAmount - betLog.betAmount;
                    profitloss.PrizeMoney = betLog.winAmount;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PalyID = betLog.betId.ToString();
                    profitloss.GameType = GetGameTypeName(betLog.gameId);
                    profitloss.BetTime = DateTimeUtil.ToDateTime(betLog.betTime);
                    profitloss.AllBetMoney = betLog.betAmount;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.HighestParentRebateMoney = 0; //直屬抽水
                    profitloss.GrandParentRebateMoney = betLog.betAmount * (decimal)0.001;   //上上級抽水
                    profitloss.ParentRebateMoney = betLog.betAmount * (decimal)0.002;        //上級抽水
                    profitloss.SelfRebateMoney = betLog.betAmount * (decimal)0.003;          //返水

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

        protected override string GetCustomizeMemo(PGSLBetLog betLog)
        {
            string gameTypeName = GetGameTypeName(betLog.gameId);
            string transactionName = null;

            if (betLog.transactionType.HasValue)
            {
                PGTransactionTypes pgTransactionType = PGTransactionTypes.GetSingle(betLog.transactionType.Value);
                transactionName = betLog.transactionType.ToString();

                if (pgTransactionType != null)
                {
                    transactionName = pgTransactionType.Name;
                }
                else
                {
                    ErrorMsgUtil.ErrorHandle(new ArgumentOutOfRangeException($"未知的PG交易類型(transactionType={betLog.transactionType})"), EnvUser);
                }
            }

            var strMemo = new StringBuilder();

            if (!gameTypeName.IsNullOrEmpty())
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.PGMemoGameID, gameTypeName));
            }

            strMemo.Append(string.Format(ThirdPartyGameElement.PGMemoBetId, betLog.betId));

            if (!transactionName.IsNullOrEmpty())
            {
                strMemo.Append(string.Format(ThirdPartyGameElement.PGMemoTransactionType, transactionName));
            }

            strMemo.Append(string.Format(ThirdPartyGameElement.PGMemoBetTime, DateTimeUtil.ToDateTime(betLog.betTime).ToFormatDateTimeString()));
            strMemo.Append(string.Format(ThirdPartyGameElement.PGMemoBetEndTime, DateTimeUtil.ToDateTime(betLog.betEndTime).ToFormatDateTimeString()));

            return strMemo.ToString();
        }

        private string GetGameTypeName(int? gameId)
        {
            string gameTypeName = null;

            if (gameId.HasValue)
            {
                PGGameIDTypes pgGameIDType = PGGameIDTypes.GetSingle(gameId.Value);
                gameTypeName = gameId.ToString();

                if (pgGameIDType != null)
                {
                    gameTypeName = pgGameIDType.Name;
                }
                else
                {
                    ErrorMsgUtil.ErrorHandle(new ArgumentOutOfRangeException($"未知的PG遊戲類型(gameId={gameId})"), EnvUser);
                }
            }

            return gameTypeName;
        }
    }
}
