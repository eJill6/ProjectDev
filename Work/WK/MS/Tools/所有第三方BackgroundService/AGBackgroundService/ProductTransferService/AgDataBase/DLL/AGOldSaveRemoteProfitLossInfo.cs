using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.AgDataBase.Model;

namespace ProductTransferService.AgDataBase.DLL
{
    public class AGOldSaveRemoteProfitLossInfo : BaseOldSaveProfitLossInfo<BaseAGInfoModel>, IAGOldSaveProfitLossInfo
    {
        private readonly Lazy<IAGProfitlossSettingService> _agProfitlossSettingService;

        public AGOldSaveRemoteProfitLossInfo(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
            _agProfitlossSettingService = DependencyUtil.ResolveService<IAGProfitlossSettingService>();
        }

        protected override PlatformProduct Product => PlatformProduct.AG;

        protected override List<InsertTPGameProfitlossParam> ConvertFilterBetLogToTPGameProfitloss(Dictionary<string, int> accountMap, List<BaseAGInfoModel> betLogs)
        {
            var insertTPGameProfitlossParams = new List<InsertTPGameProfitlossParam>();

            foreach (BaseAGInfoModel baseAGInfoModel in betLogs)
            {
                InsertTPGameProfitlossParam? insertParam = null;

                if (baseAGInfoModel is AGInfo)
                {
                    insertParam = ConvertAGInfoToTPGameProfitlossParam(accountMap, baseAGInfoModel as AGInfo);
                }
                else if (baseAGInfoModel is AgFishInfo)
                {
                    insertParam = ConvertAGFishToTPGameProfitlossParam(accountMap, baseAGInfoModel as AgFishInfo);
                }
                else
                {
                    throw new NotSupportedException();
                }

                if (insertParam == null)
                {
                    continue;
                }

                insertTPGameProfitlossParams.Add(insertParam);
            }

            return insertTPGameProfitlossParams;
        }

        private InsertTPGameProfitlossParam? ConvertAGInfoToTPGameProfitlossParam(Dictionary<string, int> accountMap, AGInfo agInfo)
        {
            if (!accountMap.TryGetValue(agInfo.TPGameAccount, out int userId) || agInfo.IsIgnoreAddProfitLoss)
            {
                return null;
            }

            string dataType = agInfo.dataType;
            string memo = _agProfitlossSettingService.Value.CreateAGMemo(agInfo);
            decimal validBetMoney = agInfo.validBetAmount;
            decimal allBetMoney = agInfo.betAmount;
            decimal winMoney = agInfo.netAmount;
            BetResultType betResultType = winMoney.ToBetResultType();
            string playId = agInfo.billNo;
            string gameType = agInfo.MiseOrderGameId;
            bool isNoRebateAmount = false;

            if (_agProfitlossSettingService.Value.NoRebateKeywords.Any(keyword => memo.IndexOf(keyword) >= 0) ||
                (agInfo.dataType == "EBR" && agInfo.validBetAmount == 0))
            {
                isNoRebateAmount = true;
            }

            var insertParam = new InsertTPGameProfitlossParam()
            {
                UserID = userId,
                PlayID = playId,
                Memo = memo,
                GameType = gameType,
                BetTime = agInfo.betTime,
                ProfitLossTime = agInfo.recalcuTime.Value,
                WinMoney = winMoney,
                BetResultType = betResultType.Value,
                KeyId = agInfo.KeyId,
                FromSource = AGProfitLossInfo.AGProfitLossInfoTableName,
                IsNoRebateAmount = isNoRebateAmount
            };

            insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, validBetMoney, allBetMoney);

            return insertParam;
        }

        private InsertTPGameProfitlossParam? ConvertAGFishToTPGameProfitlossParam(Dictionary<string, int> accountMap, AgFishInfo agFishInfo)
        {
            if (!accountMap.TryGetValue(agFishInfo.TPGameAccount, out int userId) || agFishInfo.IsIgnoreAddProfitLoss)
            {
                return null;
            }

            string memo = _agProfitlossSettingService.Value.CreateFishMemo(agFishInfo);
            decimal betMoney = agFishInfo.Cost;
            decimal winMoney = agFishInfo.transferAmount;
            BetResultType betResultType = winMoney.ToBetResultType();
            string playId = agFishInfo.tradeNo;
            string gameType = agFishInfo.MiseOrderGameId;

            var insertParam = new InsertTPGameProfitlossParam()
            {
                UserID = userId,
                PlayID = playId,
                Memo = memo,
                GameType = gameType,
                BetTime = agFishInfo.creationTime,
                ProfitLossTime = agFishInfo.SceneEndTime,
                WinMoney = winMoney,
                BetResultType = betResultType.Value,
                KeyId = agFishInfo.KeyId,
                FromSource = AGProfitLossInfo.FishProfitLossInfoTableName,
                IsNoRebateAmount = true
            };

            insertParam.SetBetMoneys(IsComputeAdmissionBetMoney, betMoney, betMoney);

            return insertParam;
        }
    }
}