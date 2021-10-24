using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class BoardGameCommissionRuleInfoRep : BaseGameCommissionRuleInfoRep
    {
        public BoardGameCommissionRuleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override CommissionGroupType CommissionGroupType => CommissionGroupType.BoardGame;

        protected override string RuleInfoTableName => "CommissionLCRuleInfo";

        protected override string SaveRuleInfoSpName => "Pro_AddUserCommissionBoardGameRule";
    }
}
