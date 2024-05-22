using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.SportDataBase.BLL;
using ProductTransferService.SportDataBase.Model;
using JxBackendService.Common.Extensions;

namespace ProductTransferService.SportDataBase.DLL
{
    public interface ISportOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SabaBetDetailViewModel>
    { }

    public class SportOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SabaBetDetailViewModel>, ISportOldSaveProfitLossInfo
    {
        public SportOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.Sport;

        protected override List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(Dictionary<string, int> accountMap, List<SabaBetDetailViewModel> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (SabaBetDetailViewModel betLogViewModel in betLogs)
            {
                ISabaSportBetDetailInfo detailInfo = betLogViewModel.SabaSportBetDetailInfo;
                ISabaSportBetDetailName detailName = betLogViewModel.SabaSportBetDetailName;

                if (!accountMap.TryGetValue(betLogViewModel.TPGameAccount, out int userId))
                {
                    continue;
                }

                if (!DateTime.TryParse(detailInfo.Transaction_time.ToNonNullString().Replace("T", " "), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(detailInfo.Settlement_time.ToNonNullString().Replace("T", " "), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                //saba來的資料會出現輸贏時間<交易時間
                if (profitLossTime < betTime)
                {
                    profitLossTime = DateTime.Now;
                }

                decimal allBetMoney = Convert.ToDecimal(detailInfo.Stake);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(detailInfo.Winlost_amount);
                string oddsType = detailInfo.Odds_type.ToNonNullString();
                string odds = detailInfo.Odds.ToNonNullString();
                string ticketStatus = detailInfo.Ticket_status.ToNonNullString().ToLower();
                string ticketExtraStatus = detailInfo.Ticket_extra_status.ToNonNullString().ToLower();
                string betType = detailInfo.Bet_type.ToNonNullString();

                BetResultType? betResultType = null;

                if (ticketExtraStatus == SabaTicketExtraStatus.Cashout)
                {
                    betResultType = SabaTicketExtraStatus.Cashout.BetResultType;
                }
                else
                {
                    SabaTicketStatus sabaTicketStatus = SabaTicketStatus.GetSingle(ticketStatus);

                    if (sabaTicketStatus != null && sabaTicketStatus.BetResultType != null)
                    {
                        betResultType = sabaTicketStatus.BetResultType;
                    }
                }

                if (betResultType == null)
                {
                    betResultType = winMoney.ToBetResultType();
                }

                var gameTypeItems = new List<string>
                {
                    detailName.SportTypeName.ToNonNullString(),
                    detailName.LeagueName.ToNonNullString(),
                    detailName.HomeName.ToNonNullString(),
                    detailName.AwayName.ToNonNullString()
                };

                gameTypeItems.RemoveAll(r => r.IsNullOrEmpty());

                var insertParam = new InsertTPGameProfitlossParam
                {
                    KeyId = betLogViewModel.KeyId,
                    UserID = userId,
                    PlayID = detailInfo.Trans_id.ToNonNullString(),
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    Memo = detailName.Memo.ToNonNullString(),
                    GameType = string.Join(",", gameTypeItems).ToShortString(197)
                };

                if (IsComputeAdmissionBetMoney)
                {
                    WagerType wagerType = TransactionLogs.ConvertToWagerType(betType);
                    List<HandicapInfo> handicapInfos;

                    if (wagerType == WagerType.Combo && detailInfo.ParlayData.AnyAndNotNull())
                    {
                        handicapInfos = detailInfo.ParlayData.Select(s => new HandicapInfo()
                        {
                            Handicap = oddsType,
                            Odds = s.Odds.ToDecimalNullable()
                        }).ToList();
                    }
                    else
                    {
                        handicapInfos = new List<HandicapInfo>()
                        {
                            new HandicapInfo()
                            {
                                Handicap = oddsType,
                                Odds = odds.ToDecimalNullable()
                            }
                        };
                    }

                    validBetMoney = TPGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = wagerType
                    });
                }

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}