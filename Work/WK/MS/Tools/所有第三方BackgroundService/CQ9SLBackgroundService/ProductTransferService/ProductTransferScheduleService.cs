using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.CQ9SL;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<CQ9BetLog>
    {
        private static readonly string s_memoResourceName = typeof(ThirdPartyGameElement).FullName;

        public ProductTransferScheduleService()
        {
        }

        protected override PlatformProduct Product => PlatformProduct.CQ9SL;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override bool IsToLowerTPGameAccount => false;

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, requestAndResponse);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<CQ9BetLog> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (CQ9BetLog betLog in betLogs)
            {
                if (!userMap.TryGetValue(betLog.TPGameAccount, out int userId))
                {
                    continue;
                }

                decimal validBetMoney = betLog.Bet;
                decimal allBetMoney = betLog.Bet;
                decimal winMoney = betLog.Win;

                if (betLog.GameType == CQ9GameType.Table)
                {
                    validBetMoney = betLog.ValidBet;
                }

                if (betLog.GameType == CQ9GameType.Slot ||
                    betLog.GameType == CQ9GameType.Arcade ||
                    betLog.GameType == CQ9GameType.Fish)
                {
                    winMoney = betLog.Win - betLog.Bet;
                }

                string subGameCode = ConvertToSubGameCode(betLog.GameType);

                BetResultType betResultType = winMoney.ToBetResultType();

                var insertTPGameProfitlossParam = new InsertTPGameProfitlossParam
                {
                    KeyId = betLog.KeyId,
                    UserID = userId,
                    WinMoney = winMoney,
                    Memo = betLog.Memo,
                    PlayID = betLog.KeyId,
                    GameType = subGameCode,
                    BetTime = betLog.BetTime.UtcDateTime.ToChinaDateTime(),
                    ProfitLossTime = betLog.CreateTime.UtcDateTime.ToChinaDateTime(),
                    BetResultType = betResultType.Value
                };

                insertTPGameProfitlossParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertTPGameProfitlossParam);
            }

            return insertTPGameProfitlossParams;
        }

        protected override LocalizationParam GetCustomizeMemo(CQ9BetLog betLog)
        {
            var localizationParam = new LocalizationParam
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            MiseOrderGameId miseOrderGameId = ConvertToOrderGameId(betLog.GameType);

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = s_memoResourceName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9ShareMemo),
                Args = new List<string>()
                {
                    miseOrderGameId.Name,
                    betLog.GameType,
                    betLog.GameCode,
                    betLog.Bet.ToString(),
                    betLog.EndRoundTime.UtcDateTime.ToChinaDateTime().ToFormatDateTimeString()
                }
            });

            if (miseOrderGameId == MiseOrderGameId.CQ9Slot)
            {
                localizationParam.LocalizationSentences.AddRange(GetCQ9SlotSentences(betLog));
            }
            else if (miseOrderGameId == MiseOrderGameId.CQ9Table)
            {
                localizationParam.LocalizationSentences.AddRange(GetCQ9TableSentences(betLog));
            }

            return localizationParam;
        }

        protected override BaseReturnDataModel<List<CQ9BetLog>> ConvertToBetLogs(string apiResult)
        {
            CQ9Response<CQ9BetLogViewModel> cq9Response = apiResult.Deserialize<CQ9Response<CQ9BetLogViewModel>>();

            if (cq9Response == null)
            {
                return new BaseReturnDataModel<List<CQ9BetLog>>("cq9Response is null");
            }

            if (!cq9Response.Status.IsSuccess)
            {
                if (cq9Response.Status.Code == CQ9Code.DataNotFound)
                {
                    return new BaseReturnDataModel<List<CQ9BetLog>>(ReturnCode.Success, dataModel: new List<CQ9BetLog>());
                }

                return new BaseReturnDataModel<List<CQ9BetLog>>(cq9Response.Status.ErrorMessage);
            }

            return new BaseReturnDataModel<List<CQ9BetLog>>(ReturnCode.Success, cq9Response.Data.Data);
        }

        private List<LocalizationSentence> GetCQ9SlotSentences(CQ9BetLog betLog)
        {
            var localizationSentences = new List<LocalizationSentence>();

            if (betLog.Detail.AnyAndNotNull())
            {
                CQ9BetDetail? freeGameDetail = betLog.Detail.Where(w => w.FreeGame > 0).FirstOrDefault();

                if (freeGameDetail != null)
                {
                    localizationSentences.Add(
                        new LocalizationSentence()
                        {
                            ResourceName = s_memoResourceName,
                            ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9DetailFreeGame),
                            Args = new List<string>()
                            {
                                freeGameDetail.FreeGame.ToString()
                            }
                        });
                }

                CQ9BetDetail? luckydrawDetail = betLog.Detail.Where(w => w.Luckydraw > 0).FirstOrDefault();

                if (luckydrawDetail != null)
                {
                    localizationSentences.Add(
                        new LocalizationSentence()
                        {
                            ResourceName = s_memoResourceName,
                            ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9DetailLuckyDraw),
                            Args = new List<string>()
                            {
                                luckydrawDetail.Luckydraw.ToString()
                            }
                        });
                }

                CQ9BetDetail? bonusDetail = betLog.Detail.Where(w => w.Bonus > 0).FirstOrDefault();

                if (bonusDetail != null)
                {
                    localizationSentences.Add(
                        new LocalizationSentence()
                        {
                            ResourceName = s_memoResourceName,
                            ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9DetailBonus),
                            Args = new List<string>()
                            {
                                bonusDetail.Bonus.ToString()
                            }
                        });
                }
            }

            localizationSentences.Add(
                new LocalizationSentence()
                {
                    ResourceName = s_memoResourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9SinglerowbetMemo),
                    Args = new List<string>()
                    {
                        betLog.SingleRowBet.GetYesNoText()
                    }
                });

            if (betLog.IsFreeGame)
            {
                localizationSentences.Add(
                    new LocalizationSentence()
                    {
                        ResourceName = s_memoResourceName,
                        ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9FreeGameMemo),
                        Args = new List<string>()
                        {
                            betLog.TicketId,
                            betLog.TicketBets.ToString()
                        }
                    });
            }

            return localizationSentences;
        }

        private List<LocalizationSentence> GetCQ9TableSentences(CQ9BetLog betLog)
        {
            var localizationSentences = new List<LocalizationSentence>
            {
                new LocalizationSentence()
                {
                    ResourceName = s_memoResourceName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.CQ9TableMemo),
                    Args = new List<string>()
                    {
                        betLog.GameRole,
                        betLog.BankerType,
                        betLog.Rake.ToString()
                    }
                }
            };

            return localizationSentences;
        }

        private string ConvertToSubGameCode(string cq9GameTypeValue)
        {
            MiseOrderGameId miseOrderGameId = ConvertToOrderGameId(cq9GameTypeValue);

            return miseOrderGameId.SubGameCode;
        }

        private MiseOrderGameId ConvertToOrderGameId(string cq9GameTypeValue)
        {
            CQ9GameType cq9GameType = CQ9GameType.GetSingle(cq9GameTypeValue);

            if (cq9GameType == null)
            {
                //default slot
                cq9GameType = CQ9GameType.Slot;
            }

            return cq9GameType.OrderGameId;
        }
    }
}