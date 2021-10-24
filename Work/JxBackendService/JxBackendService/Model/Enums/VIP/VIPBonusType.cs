using JxBackendService.Resource.Element;
using JxBackendService.Service.VIP.Bonus;
using System;

namespace JxBackendService.Model.Enums.VIP
{
    public class VIPBonusType : BaseIntValueModel<VIPBonusType>
    {
        public Type VIPBonusServiceType { get; private set; }

        private VIPBonusType() { }

        /// <summary>
        /// 晉級禮金
        /// </summary>
        public static readonly VIPBonusType LevelUpMoney = new VIPBonusType
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.VIPBonusType_LevelUpMoney),
            VIPBonusServiceType = null //暫時用不到       
        };

        /// <summary>
        /// 月紅包
        /// </summary>
        public static readonly VIPBonusType MonthlyRedEnvelope = new VIPBonusType
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.VIPBonusType_MonthlyRedEnvelope),
            VIPBonusServiceType = typeof(VIPBonusMonthlyRedEnvelopeService),
        };

        /// <summary>
        /// 生日禮金
        /// </summary>
        public static readonly VIPBonusType BirthdayGiftMoney = new VIPBonusType
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.VIPBonusType_BirthdayGiftMoney),
            VIPBonusServiceType = typeof(VIPBirthdayGiftMoneyService),
        };
    }
}