using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.LCDataBase.Common;
using ProductTransferService.LCDataBase.Enums;
using ProductTransferService.LCDataBase.Model;

namespace ProductTransferService.LCDataBase.DLL
{
    public interface ILCOldSaveProfitLossInfo : IOldSaveProfitLossInfo<SingleBetInfoViewModel>
    { }

    public class LCOldSaveProfitLossInfo : BaseOldSaveProfitLossInfo<SingleBetInfoViewModel>, ILCOldSaveProfitLossInfo
    {
        public LCOldSaveProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.LC;

        protected override List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(Dictionary<string, int> accountMap, List<SingleBetInfoViewModel> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (SingleBetInfoViewModel betLogViewModel in betLogs)
            {
                var betLog = betLogViewModel.SingleBetInfo;

                if (!accountMap.TryGetValue(betLogViewModel.TPGameAccount, out int userId))
                {
                    continue;
                }

                if (!DateTime.TryParse(betLog.GameStartTime.ToNonNullString(), out DateTime betTime))
                {
                    betTime = DateTime.Now;
                }

                if (!DateTime.TryParse(betLog.GameEndTime.ToNonNullString(), out DateTime profitLossTime))
                {
                    profitLossTime = DateTime.Now;
                }

                string playGameName = EnumHelper<GameKind>
                    .GetEnumDescription(Enum.GetName(typeof(GameKind), betLog.KindID));

                string playRoomName = EnumHelper<RoomType>
                    .GetEnumDescription(Enum.GetName(typeof(RoomType), betLog.ServerID));

                string playGameAllName = playGameName + (string.IsNullOrEmpty(playRoomName) ? "" : "-" + playRoomName);

                decimal validBetMoney = Convert.ToDecimal(betLog.CellScore.ToNonNullString());
                decimal winMoney = Convert.ToDecimal(betLog.Profit.ToNonNullString());
                string palyId = betLog.GameID;
                string memo = betLogViewModel.Memo;
                decimal allBetMoney = Convert.ToDecimal(betLog.AllBet.ToNonNullString());

                var insertParam = new InsertTPGameProfitlossParam()
                {
                    UserID = userId,
                    PlayID = palyId,
                    Memo = memo,
                    GameType = playGameAllName,
                    BetTime = betTime,
                    ProfitLossTime = profitLossTime,
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