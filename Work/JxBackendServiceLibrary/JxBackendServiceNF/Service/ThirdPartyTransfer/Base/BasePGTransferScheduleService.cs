using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendServiceNF.Service.ThirdPartyTransfer.Base
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
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.betAmount, betLog.betAmount);
                    profitloss.WinMoney = betLog.winAmount - betLog.betAmount;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.betId.ToString();
                    profitloss.GameType = GetGameTypeName(betLog.gameId);
                    profitloss.BetTime = DateTimeUtil.ToDateTime(betLog.betTime);
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

        protected override LocalizationParam GetCustomizeMemo(PGSLBetLog betLog)
        {
            string resourceName = typeof(ThirdPartyGameElement).FullName;

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

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

            if (!gameTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.PGMemoGameID),
                    Args = new List<string>() { gameTypeName }
                });
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.PGMemoBetId),
                Args = new List<string>() { betLog.betId.ToString() }
            });

            if (!transactionName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = resourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.PGMemoTransactionType),
                    Args = new List<string>() { transactionName }
                });
            }

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.PGMemoBetTime),
                Args = new List<string>() { DateTimeUtil.ToDateTime(betLog.betTime).ToFormatDateTimeString() }
            });

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = resourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.PGMemoBetEndTime),
                Args = new List<string>() { DateTimeUtil.ToDateTime(betLog.betEndTime).ToFormatDateTimeString() }
            });

            return localizationParam;
        }

        private string GetGameTypeName(int? gameId)
        {
            string gameTypeName = null;

            if (gameId.HasValue)
            {
                PGGameIDTypes pgGameIDType = PGGameIDTypes.GetSingle(gameId.Value);

                if (pgGameIDType != null)
                {
                    gameTypeName = pgGameIDType.Name;
                }
                else
                {
                    gameTypeName = string.Format(ThirdPartyGameElement.PGGameIDTypesByID, gameId);
                    LogUtilService.ForcedDebug($"未知的PG遊戲類型(gameId={gameId})");
                }
            }

            return gameTypeName;
        }
    }
}