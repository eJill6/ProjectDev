using System.Collections.Generic;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Entity.VIP.Rule;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPLevelSetting : BaseEntityModel
    {
        [ExplicitKey]
        public int VIPLevel { get; set; }

        public string LevelName { get; set; }

        public ChangeLevelRule ChangeLevelRuleJson { get; set; }

        public List<RebateRateRule> RebateRateJson { get; set; }

        public decimal RebateMaxAmountByDay { get; set; }

        public decimal LevelUpMoney { get; set; }

        public decimal MonthlyRedEnvelopeMoney { get; set; }

        public decimal BirthdayGiftMoney { get; set; }

        public MonthlyDepositRule MonthlyDepositRuleJson { get; set; }
    }
}