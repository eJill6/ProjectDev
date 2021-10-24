using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class ESportCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public ESportCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.ESport;

        protected override string RuleInfoTableName => "CommissionAddRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionESportRule";
    }
}
