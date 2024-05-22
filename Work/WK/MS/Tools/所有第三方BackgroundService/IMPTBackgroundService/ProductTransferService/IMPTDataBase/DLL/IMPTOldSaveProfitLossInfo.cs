using IMPTDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;

namespace IMPTDataBase.DLL
{
    public interface IIMPTOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class IMPTOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, IIMPTOldSaveProfitLossInfo
    {
        public IMPTOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMPT;

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

                ISingleBetInfo singleBetInfo = betLog.SingleBetInfo;

                if (!DateTime.TryParse(singleBetInfo.GameDate.ToString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                DateTime profitLossTime = betTime;
                decimal betMoney = singleBetInfo.Bet;
                decimal prizeMoney = singleBetInfo.Win + singleBetInfo.ProgressiveWin;
                decimal winMoney = prizeMoney - betMoney;
                string playId = singleBetInfo.GameCode.ToNonNullString();
                string sourceGameType = singleBetInfo.GameType.ToNonNullString();

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = betLog.Memo,
                    GameType = IMPTGameType.GetIMPTGameType(sourceGameType).OrderGameId.SubGameCode,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = betLog.KeyId,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, betMoney, betMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}