using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.IMBGDataBase.BLL;
using ProductTransferService.IMBGDataBase.Common;
using ProductTransferService.IMBGDataBase.Model;

namespace ProductTransferService.IMBGDataBase.DLL
{
    public interface IIMBGOldSaveProfitLossInfo : IOldSaveProfitLossInfo<IMBGBetLogViewModel>
    { }

    public class IMBGOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<IMBGBetLogViewModel>, IIMBGOldSaveProfitLossInfo
    {
        public IMBGOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMBG;

        protected override List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(Dictionary<string, int> accountMap, List<IMBGBetLogViewModel> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (IMBGBetLogViewModel betLogViewModel in betLogs)
            {
                IMBGBetLog betLog = betLogViewModel.IMBGBetLog;

                if (!accountMap.TryGetValue(betLogViewModel.TPGameAccount, out int userId))
                {
                    continue;
                }

                int? gameId = null;

                if (int.TryParse(betLog.GameId.ToNonNullString(), out int value))
                {
                    gameId = value;
                }

                string gameName = IMBGGameNameUtil.GetGameName(gameId);

                DateTime gameStartTime = DateTime.Now;
                DateTime gameEndTime = DateTime.Now;

                if (!betLog.OpenTime.IsNullOrEmpty())
                {
                    gameStartTime = DateTimeUtility.Instance.ToLocalTime(betLog.OpenTime.ToNonNullString());
                }

                if (!betLog.EndTime.IsNullOrEmpty())
                {
                    gameEndTime = DateTimeUtility.Instance.ToLocalTime(betLog.EndTime.ToNonNullString());
                }

                decimal validBetMoney = Convert.ToDecimal(betLog.AllBills.ToNonNullString());
                decimal allBetMoney = Convert.ToDecimal(betLog.EffectBet.ToNonNullString());
                decimal winMoney = Convert.ToDecimal(betLog.WinLost.ToNonNullString());
                string playId = betLog.Id.ToString();

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = playId,
                    Memo = betLogViewModel.Memo,
                    GameType = gameName,
                    BetTime = gameStartTime,
                    ProfitLossTime = gameEndTime,
                    WinMoney = winMoney,
                    BetResultType = winMoney.ToBetResultType().Value,
                    KeyId = betLogViewModel.KeyId,
                };

                insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }
    }
}