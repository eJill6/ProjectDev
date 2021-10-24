using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class LotteryCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public LotteryCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.PlatformLottery;

        protected override string RuleInfoTableName => "CommissionAccRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionPlatformLotteryRule";
    }
}
