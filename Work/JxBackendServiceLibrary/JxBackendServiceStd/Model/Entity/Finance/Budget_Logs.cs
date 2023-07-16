using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity.Finance
{
    public class Budget_Logs
    {
        [ExplicitKey]
        [VarcharColumnInfo(32)]
        public string BudgetID { get; set; }

        public int UserID { get; set; }

        public int BudgetType { get; set; }

        public decimal OldAvailableScores { get; set; }

        public decimal OldFreezeScores { get; set; }

        public decimal NewAvailableScores { get; set; }

        public decimal NewFreezeScores { get; set; }

        public DateTime ChangesTime { get; set; }

        public decimal ChangesAMoney { get; set; }

        public decimal ChangesFMoney { get; set; }

        [NVarcharColumnInfo(50)]
        public string Handler { get; set; }

        [NVarcharColumnInfo(1024)]
        public string Memo { get; set; }

        [VarcharColumnInfo(32)]
        public string RefID { get; set; }
    }
}