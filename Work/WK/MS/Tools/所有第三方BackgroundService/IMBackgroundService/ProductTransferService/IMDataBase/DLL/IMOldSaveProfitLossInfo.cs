using IMDataBase.Model;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Data;

namespace IMDataBase.DLL
{
    public interface IIMOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class IMOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, IIMOldSaveProfitLossInfo
    {
        public IMOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IM;

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

                if (!DateTime.TryParse(singleBetInfo.WagerCreationDateTime, out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(singleBetInfo.SettlementDateTime, out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                string memo = betLog.Memo;
                decimal allBetMoney = Convert.ToDecimal(singleBetInfo.StakeAmount);
                decimal validBetMoney = allBetMoney;
                decimal winMoney = Convert.ToDecimal(singleBetInfo.WinLoss);
                string playId = singleBetInfo.BetId;
                string oddsType = singleBetInfo.OddsType;
                string gameTypeName;
                BetResultType betResultType = winMoney.ToBetResultType();

                string detailItems = singleBetInfo.DetailItems;
                List<DetailItem> detailItemList = detailItems.Deserialize<List<DetailItem>>();

                gameTypeName = string.Join("/", detailItemList
                    .Select(s => s.SportsName.IsNullOrEmpty() ? s.BetDescription : s.SportsName))
                    .ToShortString(IMProfitLossInfo.GameNameMaxLength);

                if (IsComputeAdmissionBetMoney)
                {
                    List<HandicapInfo> handicapInfos = new List<HandicapInfo>();

                    if (detailItemList.AnyAndNotNull())
                    {
                        handicapInfos.AddRange(
                            detailItemList.
                            Select(s => new HandicapInfo()
                            {
                                Handicap = oddsType,
                                Odds = s.Odds.ToDecimalNullable()
                            }).
                        ToList());
                    }

                    validBetMoney = TPGameApiReadService.ComputeAdmissionBetMoney(new ComputeAdmissionBetMoneyParam()
                    {
                        AllBetMoney = allBetMoney,
                        HandicapInfos = handicapInfos,
                        ProfitLossMoney = winMoney,
                        BetResultType = betResultType,
                        WagerType = WagerType.Single
                    });
                }

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = memo,
                    GameType = gameTypeName,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = betResultType.Value,
                    KeyId = betLog.KeyId
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}