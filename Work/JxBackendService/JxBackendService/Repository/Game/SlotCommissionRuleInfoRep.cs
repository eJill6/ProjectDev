using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class SlotCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public SlotCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.Slot;

        protected override string RuleInfoTableName => "CommissionPTRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionSlotRule";
    }
}
