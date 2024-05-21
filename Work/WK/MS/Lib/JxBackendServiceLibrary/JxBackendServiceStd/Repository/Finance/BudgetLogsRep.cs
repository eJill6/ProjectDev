﻿using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Finance
{
    public class BudgetLogsRep : BaseDbRepository<Budget_Logs>, IBudgetLogsRep
    {
        public BudgetLogsRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public string CreateBudgetID() => GetSequenceIdentity("SEQ_Budget_Logs_BudgetID");
    }
}