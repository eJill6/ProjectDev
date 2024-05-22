using IMPPDataBase.Model;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendService.Common.Util;

namespace IMPPDataBase.DLL
{
    public interface IIMPPOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class IMPPOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, IIMPPOldSaveProfitLossInfo
    {
        public IMPPOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMPP;

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

                string gameType = IMOneProviderType.GetSingle(singleBetInfo.Provider.ToString()).OrderGameId.SubGameCode;
                string dateCreated = singleBetInfo.DateCreated.ToString();
                string gameDate = singleBetInfo.GameDate.ToString();

                DateTime betTime;

                if (!gameDate.IsNullOrEmpty())
                {
                    betTime = DateTime.Parse(gameDate);
                }
                else if (!dateCreated.IsNullOrEmpty())
                {
                    betTime = DateTime.Parse(dateCreated);
                }
                else
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(singleBetInfo.LastUpdatedDate.ToString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                decimal betMoney = Convert.ToDecimal(singleBetInfo.BetAmount);
                decimal winMoney = Convert.ToDecimal(singleBetInfo.WinLoss);
                string playId = singleBetInfo.RoundId.ToString();

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = betLog.Memo,
                    GameType = gameType,
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