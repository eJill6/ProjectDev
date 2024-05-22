using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.FYES;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<FYESBetLog>
    {
        protected override PlatformProduct Product => PlatformProduct.FYES;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override BaseReturnDataModel<List<FYESBetLog>> ConvertToBetLogs(string apiResult)
        {
            string[] responseContents = apiResult.Deserialize<string[]>();
            var betLogs = new List<FYESBetLog>();

            foreach (string responseContent in responseContents)
            {
                var betLogModel = responseContent.Deserialize<FYESGetBetLogResponseModel>();

                if (betLogModel.IsSuccess)
                {
                    if (betLogModel.info != null && betLogModel.info.list != null)
                    {
                        betLogs.AddRange(betLogModel.info.list.Where(l =>
                        {
                            FYESBetLogStatus status = FYESBetLogStatus.GetSingle(l.Status);

                            if (status != null)
                            {
                                return status.IsSettled;
                            }

                            return false;
                        }));
                    }
                }
                else
                {
                    string errorMsg = betLogModel.msg;

                    return new BaseReturnDataModel<List<FYESBetLog>>(errorMsg, betLogs);
                }
            }

            return new BaseReturnDataModel<List<FYESBetLog>>(ReturnCode.Success, betLogs);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<FYESBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();             

            foreach (FYESBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                FYESBetLogStatus betLogStatus = FYESBetLogStatus.GetSingle(betLog.Status);

                if (betLogStatus != null && betLogStatus.IsSettled && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.WinMoney = betLog.Money;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.OrderID;
                    profitloss.GameType = betLog.Type;
                    profitloss.BetTime = betLog.CreateAt;
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.ProfitLossTime = betLog.RewardAt.GetValueOrDefault();

                    BetResultType betResultType = betLog.Money.ToBetResultType();
                    profitloss.BetResultType = betResultType.Value;

                    decimal allBetMoney = betLog.BetAmount;
                    decimal validBetMoney = allBetMoney;

                    if (IsComputeAdmissionBetMoney)
                    {
                        WagerType wagerType = WagerType.Single;
                        List<HandicapInfo> handicapInfos = new List<HandicapInfo>();

                        if (FYESBetType.GetSingle(betLog.Type) == FYESBetType.Smart)
                        {
                            handicapInfos.Add(new HandicapInfo()
                            {
                                Handicap = FYESHandicap.EU.Value, // 此Type固定为欧洲盘
                                Odds = betLog.Odds
                            });
                        }
                        else if (FYESBetType.GetSingle(betLog.Type) == FYESBetType.Combo && betLog.Details.AnyAndNotNull())
                        {
                            handicapInfos.AddRange(
                                betLog.Details.Select(s => new HandicapInfo()
                                {
                                    Handicap = s.OddsType,
                                    Odds = s.Odds
                                }).ToList());

                            wagerType = WagerType.Combo;
                        }
                        else
                        {
                            handicapInfos.Add(new HandicapInfo()
                            {
                                Handicap = betLog.OddsType,
                                Odds = betLog.Odds
                            });
                        }

                        validBetMoney = TPGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                        {
                            AllBetMoney = allBetMoney,
                            HandicapInfos = handicapInfos,
                            ProfitLossMoney = betLog.Money,
                            BetResultType = betResultType,
                            WagerType = wagerType
                        });
                    }

                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }

            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(FYESBetLog betLog)
        {
            var localizationParam = new LocalizationParam()
            {
                SplitOperator = ",",
                LocalizationSentences = new List<LocalizationSentence>()
            };

            FYESBetType fYESBetType = FYESBetType.GetSingle(betLog.Type) ?? FYESBetType.Single;

            localizationParam.LocalizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = typeof(DBContentElement).FullName,
                ResourcePropertyName = nameof(DBContentElement.FYESOrderInfo),
                Args = new List<string>()
                {
                    fYESBetType.Name,
                    betLog.OrderID
                }
            });

            if (fYESBetType == FYESBetType.Combo)
            {
                FYESHandicap comboHandicap = FYESHandicap.GetSingle(betLog.OddsType) ?? FYESHandicap.EU;

                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = typeof(DBContentElement).FullName,
                    ResourcePropertyName = nameof(DBContentElement.FYESComboInfo),
                    Args = new List<string>()
                    {
                        comboHandicap.PlatformHandicap.Name,
                        betLog.Odds.ToString(),
                        betLog.BetAmount.ToString(),
                        betLog.Status,
                        betLog.CreateAt.ToFormatDateTimeString(),
                        betLog.RewardAt.ToFormatDateTimeString(),
                    }
                });

                for (int i = 0; i < betLog.Details.Count; i++)
                {
                    if (i >= MaxDetailMemoContentCount - 1)
                    {
                        localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                        {
                            ResourceName = typeof(CommonElement).FullName,
                            ResourcePropertyName = nameof(CommonElement.InfoSimplified),
                        });

                        break;
                    }

                    FYESBetLogDetail detail = betLog.Details[i];

                    FYESHandicap detailHandicap = FYESHandicap.GetSingle(detail.OddsType) ?? FYESHandicap.EU;

                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = typeof(DBContentElement).FullName,
                        ResourcePropertyName = nameof(DBContentElement.FYESComboDetail),
                        Args = new List<string>()
                        {
                            detail.DetailID,
                            detail.Category,
                            detailHandicap.PlatformHandicap.Name,
                            detail.Odds.ToString(),
                            detail.Status,
                            betLog.CreateAt.ToFormatDateTimeString(),
                            detail.ResultAt.ToFormatDateTimeString(),
                        }
                    });
                }
            }
            else
            {
                if (!betLog.Category.IsNullOrEmpty())
                {
                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = typeof(DBContentElement).FullName,
                        ResourcePropertyName = nameof(DBContentElement.FYESCategory),
                        Args = new List<string>() { betLog.Category }
                    });
                }
                else if (!betLog.Code.IsNullOrEmpty())
                {
                    localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                    {
                        ResourceName = typeof(DBContentElement).FullName,
                        ResourcePropertyName = nameof(DBContentElement.FYESCode),
                        Args = new List<string>() { betLog.Code }
                    });
                }

                FYESHandicap handicap = FYESHandicap.GetSingle(betLog.OddsType) ?? FYESHandicap.EU;

                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = typeof(DBContentElement).FullName,
                    ResourcePropertyName = nameof(DBContentElement.FYESGameInfo),
                    Args = new List<string>()
                    {
                        handicap.PlatformHandicap.Name,
                        betLog.Odds.ToString(),
                        betLog.BetAmount.ToString(),
                        betLog.CreateAt.ToFormatDateTimeString(),
                        betLog.RewardAt.ToFormatDateTimeString(),
                    }
                });
            }

            return localizationParam;
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, dataModel);
        }

        protected override bool IsToLowerTPGameAccount => true;
    }
}