using System.Text;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<OBSPBetLog>
    {
        protected override PlatformProduct Product => PlatformProduct.OBSP;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);

        protected override BaseReturnDataModel<List<OBSPBetLog>> ConvertToBetLogs(string apiResult)
        {
            string[] responseContents = apiResult.Deserialize<string[]>();
            var betLogs = new List<OBSPBetLog>();

            foreach (string responseContent in responseContents)
            {
                var queryBetListResponse = responseContent.Deserialize<OBSPApiResponse<OBSPQueryBetListData>>();

                if (queryBetListResponse.Data.List.AnyAndNotNull())
                {
                    queryBetListResponse.Data.List.RemoveAll(r => r.OrderStatus != OBSPOrderStatus.Done);
                    betLogs.AddRange(queryBetListResponse.Data.List);
                }
            }

            return new BaseReturnDataModel<List<OBSPBetLog>>(ReturnCode.Success, betLogs);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(Dictionary<string, int> userMap, List<OBSPBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            var result = new List<InsertTPGameProfitlossParam>();

            foreach (OBSPBetLog betLog in betLogs)
            {
                var profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                if (betLog.OrderStatus == OBSPOrderStatus.Done && userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.WinMoney = betLog.ProfitAmount.GetValueOrDefault();
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.OrderNo;
                    profitloss.GameType = string.Join("/", betLog.DetailList.Select(s => s.SportName).Distinct());
                    profitloss.BetTime = betLog.CreateTime.ToDateTime();
                    profitloss.KeyId = betLog.KeyId;

                    BetResultType betResultType = ConvertToBetResultType(profitloss.WinMoney, betLog.PreBetAmount, betLog.Outcome);
                    profitloss.BetResultType = betResultType.Value;

                    if (betLog.SettleTime.HasValue)
                    {
                        profitloss.ProfitLossTime = betLog.SettleTime.Value.ToDateTime();
                    }
                    else
                    {
                        profitloss.ProfitLossTime = DateTime.Now;
                    }

                    decimal validBetMoney = betLog.OrderAmount;

                    if (IsComputeAdmissionBetMoney)
                    {
                        List<HandicapInfo> handicapInfos = betLog.DetailList
                        .Select(s => new HandicapInfo()
                        {
                            Handicap = s.MarketType,
                            Odds = s.OddsValue.ToDecimalNullable()
                        })
                        .ToList();

                        WagerType wagerType = ConvertToWagerType(betLog.SeriesType);

                        validBetMoney = TPGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                        {
                            AllBetMoney = betLog.OrderAmount,
                            HandicapInfos = handicapInfos,
                            ProfitLossMoney = profitloss.WinMoney,
                            BetResultType = betResultType,
                            WagerType = wagerType
                        });
                    }

                    profitloss.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, betLog.OrderAmount);
                }
                else
                {
                    profitloss.IsIgnore = true;
                }

                result.Add(profitloss);
            }

            return result;
        }

        protected override LocalizationParam GetCustomizeMemo(OBSPBetLog betLog)
        {
            WagerType wagerType = ConvertToWagerType(betLog.SeriesType);
            StringBuilder allDetailContent = new StringBuilder();

            PlatformHandicap platformHandicap = null;

            OBSPHandicap obspHandicap = OBSPHandicap.GetSingle(betLog.DetailList.First().MarketType);

            if (obspHandicap != null)
            {
                platformHandicap = obspHandicap.PlatformHandicap;
            }

            for (int i = 0; i < betLog.DetailList.Count; i++)
            {
                OBSPBetLogDetail betLogDetail = betLog.DetailList[i];

                if (i > 0)
                {
                    allDetailContent.Append("；");
                }

                var detailContents = new List<string>();

                if (!betLogDetail.SportName.IsNullOrEmpty())
                {
                    detailContents.Add(betLogDetail.SportName);
                }

                if (!betLogDetail.MatchName.IsNullOrEmpty())
                {
                    detailContents.Add(betLogDetail.MatchName);
                }

                if (!betLogDetail.MatchInfo.IsNullOrEmpty())
                {
                    detailContents.Add(betLogDetail.MatchInfo);
                }

                if (!betLogDetail.PlayName.IsNullOrEmpty() || betLogDetail.PlayOptionName.IsNullOrEmpty())
                {
                    detailContents.Add(string.Format(ThirdPartyGameElement.OBSPBetContent,
                        string.Join("/",
                            new string[]
                            {
                                betLogDetail.PlayName,
                                betLogDetail.PlayOptionName
                            }.
                            Where(w => !w.IsNullOrEmpty())
                        )));
                }

                string odds = betLogDetail.OddsValue;

                if (i >= MaxDetailMemoContentCount - 1)
                {
                    odds += "...";
                }

                detailContents.Add(string.Format(ThirdPartyGameElement.SomeOdds, odds));
                allDetailContent.Append(string.Join(",", detailContents));

                if (i >= MaxDetailMemoContentCount - 1)
                {
                    break;
                }
            }

            return LocalizationMemoUtil.CreateLocalizationParam(wagerType, platformHandicap, allDetailContent.ToString(), betLog.OrderNo);
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant).Value;

            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, dataModel);
        }

        private BetResultType ConvertToBetResultType(decimal winMoney, decimal? preBetAmount, int? outcome)
        {
            if (preBetAmount.HasValue)
            {
                return BetResultType.Cashout;
            }

            if (outcome.HasValue)
            {
                return OBSPOutcome.GetSingle(outcome.Value).BetResultType;
            }

            return winMoney.ToBetResultType();
        }

        private WagerType ConvertToWagerType(int seriesType)
        {
            if (seriesType == 1)
            {
                return WagerType.Single;
            }

            return WagerType.Combo;
        }
    }
}