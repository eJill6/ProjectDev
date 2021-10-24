using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class LiveCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public LiveCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.Live;

        protected override string RuleInfoTableName => "CommissionAGRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionLiveRule";
    }
}
