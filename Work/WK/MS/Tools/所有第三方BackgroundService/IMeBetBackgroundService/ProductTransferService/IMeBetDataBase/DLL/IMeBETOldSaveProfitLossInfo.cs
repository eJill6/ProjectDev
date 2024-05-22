using IMeBetDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMeBetDataBase.DLL
{
    public interface IIMeBETOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class IMeBETOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, IIMeBETOldSaveProfitLossInfo
    {
        public IMeBETOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMeBET;

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

                string gameName = singleBetInfo.ChineseGameName.ToNonNullString();

                if (!DateTime.TryParse(singleBetInfo.BetDate, out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(singleBetInfo.ReportingDate, out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                decimal validBetMoney = Convert.ToDecimal(singleBetInfo.ValidBet);
                decimal allBetMoney = Convert.ToDecimal(singleBetInfo.BetAmount);
                decimal winMoney = Convert.ToDecimal(singleBetInfo.WinLoss);
                string palyId = singleBetInfo.BetId;

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = palyId,
                    Memo = betLog.Memo,
                    GameType = gameName,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = betLog.KeyId,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}