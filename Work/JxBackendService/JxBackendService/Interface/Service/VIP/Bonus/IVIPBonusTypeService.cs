using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.VIP.Bonus
{
    public interface IVIPBonusTypeService
    {
        /// <summary>領取禮金</summary>
        BaseReturnModel ReceiveBonus();

        ///// <summary>根據type決定回傳的金額資料</summary>
        //decimal GetBonusMoney(VIPLevelSetting vipLevelSetting);

        ///// <summary>是否允許領取</summary>
        //BaseReturnDataModel<VIPUserInfo> IsAllowReceiveBonus(int userId);
    }
}
