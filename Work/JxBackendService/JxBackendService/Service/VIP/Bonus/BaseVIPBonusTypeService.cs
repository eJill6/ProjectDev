using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.VIP.Bonus;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.VIP.Bonus
{
    public abstract class BaseVIPBonusTypeService : BaseService, IVIPBonusTypeService
    {
        private readonly IVIPUserBonusRep _vipUserBonusRep;
        private readonly IVIPLevelSettingRep _userLevelSettingRep;

        protected IVIPUserInfoRep VIPUserInfoRep { get; private set; }

        protected abstract ReturnCode ReceivedCode { get; }

        protected abstract int BonusTypeToken { get; }

        protected abstract VIPBonusType VIPBonusType { get; }

        /// <summary>領取日期是否為合法</summary>
        protected abstract bool IsReceiveDateValid(VIPUserInfo vipUserInfo);

        protected abstract BaseReturnModel SaveReceiveBonus(VIPBonusType vipBonusType, string currentLevelName, decimal bonusMoney);

        protected abstract decimal GetBonusMoney(VIPLevelSetting vipLevelSetting);

        public BaseVIPBonusTypeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _vipUserBonusRep = ResolveJxBackendService<IVIPUserBonusRep>();
            _userLevelSettingRep = ResolveJxBackendService<IVIPLevelSettingRep>();
            VIPUserInfoRep = ResolveJxBackendService<IVIPUserInfoRep>();
        }

        public BaseReturnModel ReceiveBonus()
        {
            BaseReturnDataModel<VIPUserInfo> returnDataModel = IsAllowReceiveBonus(EnvLoginUser.LoginUser.UserId);

            if (!returnDataModel.IsSuccess)
            {
                return returnDataModel.CastByJson<BaseReturnModel>();
            }

            VIPLevelSetting vipLevelSetting = _userLevelSettingRep.GetSingleByKey(InlodbType.Inlodb,
                new VIPLevelSetting { VIPLevel = returnDataModel.DataModel.CurrentLevel });

            if (vipLevelSetting == null)
            {
                return new BaseReturnModel(ReturnCode.NotExistVIPLevel);
            }

            return SaveReceiveBonus(VIPBonusType, vipLevelSetting.LevelName, GetBonusMoney(vipLevelSetting));
        }

        private BaseReturnDataModel<VIPUserInfo> IsAllowReceiveBonus(int userId)
        {
            VIPUserInfo vipUserInfo = VIPUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new VIPUserInfo { UserID = userId });

            if (vipUserInfo == null)
            {
                return new BaseReturnDataModel<VIPUserInfo>(ReturnCode.NotVIPUser, vipUserInfo);
            }

            if (_vipUserBonusRep.IsReceived(userId, BonusTypeToken, VIPBonusType.Value))
            {
                return new BaseReturnDataModel<VIPUserInfo>(ReceivedCode, vipUserInfo);
            }

            if (!IsReceiveDateValid(vipUserInfo))
            {
                return new BaseReturnDataModel<VIPUserInfo>(ReturnCode.GiftMoneyReceiveExpired, vipUserInfo);
            }

            return new BaseReturnDataModel<VIPUserInfo>(ReturnCode.Success, vipUserInfo);
        }
    }
}
