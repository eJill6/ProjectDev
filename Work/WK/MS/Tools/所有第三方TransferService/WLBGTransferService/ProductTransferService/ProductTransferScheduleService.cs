using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer.WLBG;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.WLBG;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<WLBGBetLog>
    {
        private readonly ITPGameWLBGApiService _wlbgApiService;

        public ProductTransferScheduleService()
        {
            _wlbgApiService = DependencyUtil.ResolveJxBackendService<ITPGameWLBGApiService>(
                EnvUser,
                DbConnectionTypes.Slave);
        }

        public override PlatformProduct Product => PlatformProduct.WLBG;

        public override JxApplication Application => JxApplication.WLBGTransferService;

        protected override bool IsToLowerTPGameAccount => false;

        protected override bool IsDoTransferCompensationJobEnabled => true;

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override BaseReturnDataModel<List<WLBGBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betLogs = new List<WLBGBetLog>();

            var responseModel = apiResult.Deserialize<WLBGBetLogResponseModel>();

            if (responseModel != null)
            {
                string errorMsg = null;

                if (responseModel.IsSuccess && responseModel.Data != null)
                {
                    for (int i = 0; i < responseModel.Data.Count; i++)
                    {
                        betLogs.Add(new WLBGBetLog()
                        {
                            Uid = responseModel.Data.List.Uid[i],
                            Game = responseModel.Data.List.Game[i],
                            Category = responseModel.Data.List.Category[i],
                            Profit = responseModel.Data.List.Profit[i].ToDecimal(),
                            Balance = responseModel.Data.List.Balance[i].ToDecimal(),
                            Bet = responseModel.Data.List.Bet[i].ToDecimal(),
                            ValidBet = responseModel.Data.List.ValidBet[i].ToDecimal(),
                            Tax = responseModel.Data.List.Tax[i].ToDecimal(),
                            GameStartTime = responseModel.Data.List.GameStartTime[i],
                            RecordTime = responseModel.Data.List.RecordTime[i],
                            GameId = responseModel.Data.List.GameId[i],
                            RecordId = responseModel.Data.List.RecordId[i],
                        });
                    }
                }
                else
                {
                    errorMsg = responseModel.Msg;
                }

                if (errorMsg.IsNullOrEmpty())
                {
                    return new BaseReturnDataModel<List<WLBGBetLog>>(ReturnCode.Success, betLogs);
                }

                return new BaseReturnDataModel<List<WLBGBetLog>>(errorMsg, betLogs);
            }

            throw new InvalidCastException();
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<WLBGBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (WLBGBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.GameId
                };

                if (userMap.ContainsKey(betLog.TPGameAccount))
                {
                    DateTime profitLossTime = Convert.ToDateTime(betLog.RecordTime);
                    DateTime betTime = Convert.ToDateTime(betLog.GameStartTime);
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, betLog.ValidBet, betLog.Bet);
                    profitloss.ProfitLossTime = profitLossTime;
                    profitloss.WinMoney = -betLog.Profit; //第三方平台回傳的Profit是第三方平台的輸贏，而非用戶的輸贏
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.GameId;
                    profitloss.GameType = GetGameCode(betLog.Game);
                    profitloss.BetTime = betTime;
                    profitloss.KeyId = betLog.GameId;
                    profitloss.BetResultType = profitloss.WinMoney.ToBetResultType().Value;
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }
            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(WLBGBetLog betLog)
        {
            DateTime profitLossTime = Convert.ToDateTime(betLog.RecordTime);
            DateTime betTime = Convert.ToDateTime(betLog.GameStartTime);

            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourceName = typeof(ThirdPartyGameElement).FullName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.WLBGMemo),
                        Args = new List<string>()
                        {
                            GetGameName(betLog.Game),
                            betLog.KeyId,
                            betLog.Bet.ToString(),
                            (-betLog.Profit).ToString(), //第三方平台回傳的Profit是第三方平台的輸贏，而非用戶的輸贏
                            betTime.ToFormatDateTimeString(),
                            profitLossTime.ToFormatDateTimeString()
                        }
                    }
                }
            };

            return localizationParam;
        }

        private string GetGameName(string gameValue)
        {
            Dictionary<string, string> gameDictionary = _wlbgApiService.GetApiGameListResult();

            if (gameDictionary.TryGetValue(gameValue, out string gameName))
            {
                return gameName;
            }

            return gameValue;
        }

        private string GetGameCode(string gameValue)
        {
            return WaliGameCode.GetSingleByTPGameCode(gameValue).GameCode;
        }
    }
}