using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class OtherLotteryCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public OtherLotteryCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.OtherLottery;

        protected override string RuleInfoTableName => "CommissionOtherLotteryRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionOtherLotteryRule";
    }
}
