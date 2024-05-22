using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseTransferScheduleService<BTISBetLog>
    {
        public override PlatformProduct Product => PlatformProduct.BTIS;

        public override JxApplication Application => JxApplication.BTISTransferService;

        protected override BaseReturnDataModel<List<BTISBetLog>> ConvertToBetLogs(string apiResult)
        {
            var betHistoryResponse = apiResult.Deserialize<BTISBettingHistoryResponse>();

            if (betHistoryResponse == null)
            {
                return new BaseReturnDataModel<List<BTISBetLog>>("betHistoryResponse == null", null);
            }
            else if (!betHistoryResponse.IsSuccess)
            {
                BTISDataErrorCode dataErrorCode = BTISDataErrorCode.GetSingle(betHistoryResponse.ErrorCode);
                string errorMsg = betHistoryResponse.ErrorCode.ToString();

                if (dataErrorCode == null)
                {
                    errorMsg = dataErrorCode.Code;
                }

                return new BaseReturnDataModel<List<BTISBetLog>>(errorMsg, null);
            }

            return new BaseReturnDataModel<List<BTISBetLog>>(ReturnCode.Success, betHistoryResponse.Bets);
        }

        protected override List<InsertTPGameProfitlossParam> ConvertToTPGameProfitloss(List<BTISBetLog> betLogs)
        {
            //抓取sqllite資料轉換成標準的盈虧模型
            List<InsertTPGameProfitlossParam> result = new List<InsertTPGameProfitlossParam>();
            Dictionary<string, int> userMap = GetUserIdsFromTPGameAccounts(betLogs.Select(s => s.TPGameAccount).Distinct().ToList(), IsToLowerTPGameAccount);

            foreach (BTISBetLog betLog in betLogs)
            {
                InsertTPGameProfitlossParam profitloss = new InsertTPGameProfitlossParam()
                {
                    KeyId = betLog.KeyId
                };

                BTISBettingHistoryStatus betStatus = BTISBettingHistoryStatus.GetSingle(betLog.Status);

                if (betStatus != null &&
                    betStatus.BetResultType != null &&
                    userMap.ContainsKey(betLog.TPGameAccount))
                {
                    profitloss.UserID = userMap[betLog.TPGameAccount];
                    profitloss.WinMoney = betLog.PL;
                    profitloss.Memo = betLog.Memo;
                    profitloss.PlayID = betLog.PurchaseID;
                    profitloss.GameType = string.Join("/", betLog.Selections.Select(s => s.BranchName));
                    profitloss.BetTime = betLog.CreationDate.ToChinaDateTime();
                    profitloss.KeyId = betLog.KeyId;
                    profitloss.BetResultType = betStatus.BetResultType.Value;

                    if (betLog.BetSettledDate.HasValue)
                    {
                        profitloss.ProfitLossTime = betLog.BetSettledDate.Value.ToChinaDateTime();
                    }
                    else
                    {
                        profitloss.ProfitLossTime = DateTime.Now;
                    }

                    decimal allBetMoney = betLog.TotalStake;
                    decimal validBetMoney = allBetMoney;

                    if (IsComputeAdmissionBetMoney)
                    {
                        List<HandicapInfo> handicapInfos;

                        if (betLog.Selections.AnyAndNotNull())
                        {
                            handicapInfos = betLog.Selections
                                .Select(s => new HandicapInfo()
                                {
                                    Handicap = betLog.OddsStyleOfUser,
                                    Odds = s.OddsInUserStyle.ToDecimalNullable()
                                })
                                .ToList();
                        }
                        else
                        {
                            handicapInfos = new List<HandicapInfo>()
                            {
                                new HandicapInfo()
                                {
                                    Handicap = betLog.OddsStyleOfUser,
                                    Odds = betLog.OddsInUserStyle.ToDecimalNullable()
                                }
                            };
                        }

                        WagerType wagerType = ConvertToWagerType(betLog.BetTypeId);

                        validBetMoney = _tpGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                        {
                            AllBetMoney = allBetMoney,
                            HandicapInfos = handicapInfos,
                            ProfitLossMoney = profitloss.WinMoney,
                            BetResultType = betStatus.BetResultType,
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

        protected override LocalizationParam GetCustomizeMemo(BTISBetLog betLog)
        {
            WagerType wagerType = ConvertToWagerType(betLog.BetTypeId);

            PlatformHandicap platformHandicap = null;
            BTISHandicap btisHandicap = BTISHandicap.GetSingle(betLog.OddsStyleOfUser);

            if (btisHandicap != null)
            {
                platformHandicap = btisHandicap.PlatformHandicap;
            }

            StringBuilder allDetailContent = new StringBuilder();

            for (int i = 0; i < betLog.Selections.Count; i++)
            {
                Selection selection = betLog.Selections[i];

                if (i > 0)
                {
                    allDetailContent.Append("；");
                }

                var detailContents = new List<string>();

                if (!selection.BranchName.IsNullOrEmpty())
                {
                    detailContents.Add(selection.BranchName);
                }

                if (!selection.LeagueName.IsNullOrEmpty())
                {
                    detailContents.Add(selection.LeagueName);
                }

                if (!selection.HomeTeam.IsNullOrEmpty() && !selection.AwayTeam.IsNullOrEmpty())
                {
                    detailContents.Add($"{selection.HomeTeam} VS {selection.AwayTeam}");
                }

                if (!selection.EventTypeName.IsNullOrEmpty())
                {
                    detailContents.Add(selection.EventTypeName);
                }

                if (!selection.YourBet.IsNullOrEmpty())
                {
                    detailContents.Add(selection.YourBet);
                }

                string odds = selection.OddsInUserStyle;

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

            return LocalizationMemoUtil.CreateLocalizationParam(wagerType, platformHandicap, allDetailContent.ToString(), betLog.PurchaseID);
        }

        protected override string GetNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            ISqliteTokenService sqliteTokenService = DependencyUtil.ResolveKeyed<ISqliteTokenService>(Product, Merchant);
            return sqliteTokenService.GetSqliteNextSearchToken(lastSearchToken, dataModel);
        }

        private WagerType ConvertToWagerType(int betTypeId)
        {
            BTISWagerType btisWagerType = BTISWagerType.GetSingle(betTypeId);

            if (btisWagerType != null && btisWagerType.WagerType != null)
            {
                return btisWagerType.WagerType;
            }

            return WagerType.Single;
        }
    }
}