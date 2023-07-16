using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class CreateRemoteAccountParam
    {
        public string TPGameAccount { get; set; }

        public string TPGamePassword { get; set; }
    }

    public class ComputeBetBonusAmountParam
    {
        public ComputeBetBonusProfitLoss ComputeBetBonusProfitloss { get; set; }

        public int VIPCurrentLevel { get; set; }

        /// <summary>是否計算當日抽水上限</summary>
        public bool HasMaxRebateMoneyByDay { get; set; }
    }
}