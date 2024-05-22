using System.Data;
using IMSportDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using WagerTypeUtilService;

namespace IMSportDataBase.DLL
{
    public interface IIMSportOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class IMSportOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, IIMSportOldSaveProfitLossInfo
    {
        public IMSportOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMSport;

        protected override List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(
            Dictionary<string, int> accountMap,
            List<SingleBetInfoViewModel> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (SingleBetInfoViewModel betLog in betLogs)
            {
                if (!accountMap.TryGetValue(betLog.TPGameAccount, out int userId))
                {
                    continue;
                }

                SingleBetInfo singleBetInfo = betLog.SingleBetInfo;

                if (!DateTime.TryParse(singleBetInfo.WagerCreationDateTime.ToString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (DateTime.TryParse(singleBetInfo.LastUpdatedDate.ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                decimal allBetMoney = Convert.ToDecimal(singleBetInfo.StakeAmount);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(singleBetInfo.WinLoss);
                string playId = singleBetInfo.BetId.ToNonNullString();
                string oddsType = singleBetInfo.OddsType.ToNonNullString();
                string imsportWagerType = singleBetInfo.WagerType.ToNonNullString();
                string resultStatus = singleBetInfo.ResultStatus.ToNonNullString();
                bool isCashout = singleBetInfo.BetTradeStatus.ToNonNullString() == SingleBetInfo.s_cashOutTradeStatus;

                List<DetailItem> detailItemList = singleBetInfo.DetailItems;

                //兌現處理
                if (isCashout)
                {
                    winMoney = singleBetInfo.BetTradeBuybackAmount.ToNonNullString().ToDecimal() - allBetMoney;
                }

                //上線時alter add column會有舊資料問題
                BetResultType betResultType = winMoney.ToBetResultType();
                IMSportBetStatus imSportBetStatus = IMSportBetStatus.GetSingle(resultStatus);

                if (isCashout)
                {
                    betResultType = BetResultType.Cashout;
                }
                else
                {
                    if (imSportBetStatus != null && imSportBetStatus.BetResultType != null)
                    {
                        betResultType = imSportBetStatus.BetResultType;
                    }
                }

                if (detailItemList.Any() && IsComputeAdmissionBetMoney)
                {
                    List<HandicapInfo> handicapInfos = detailItemList
                        .Select(s => new HandicapInfo()
                        {
                            Handicap = oddsType,
                            Odds = s.Odds.ToDecimalNullable()
                        })
                        .ToList();

                    WagerType wagerType = WagerTypeUtil.ConvertToWagerType(imsportWagerType);

                    validBetMoney = TPGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = wagerType,
                    });
                }

                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
                string productWallet = configUtilService.Get("ProductWallet", string.Empty).Trim();

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = betLog.Memo,
                    GameType = productWallet,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    KeyId = betLog.KeyId,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}