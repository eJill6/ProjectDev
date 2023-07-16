using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.StoredProcedureParam.Finance
{
    public class ProCreateCMoneyOutInfoParam
    {
        [VarcharColumnInfo(32)]
        public string MoneyOutID { get; set; }

        public int UserID { get; set; }

        [NVarcharColumnInfo(50)]
        public string OrderID { get; set; }

        public decimal Amount { get; set; }

        public int MoneyOutDealType { get; private set; }

        [NVarcharColumnInfo(50)]
        public string Handler { get; set; }

        [NVarcharColumnInfo(1024)]
        public string Memo { get; set; }

        public int BudgetType { get; private set; }

        [VarcharColumnInfo(32)]
        public string BudgetID { get; set; }

        [VarcharColumnInfo(6)]
        public string RC_Success => ReturnCode.Success.Value;

        [VarcharColumnInfo(6)]
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;

        [VarcharColumnInfo(6)]
        public string RC_SystemError => ReturnCode.SystemError.Value;

        public void SetMoneyOutDealType(MoneyOutDealType moneyOutDealType)
        {
            MoneyOutDealType = moneyOutDealType.Value;
        }

        public void SetBudgetType(BudgetType budgetType)
        {
            BudgetType = budgetType.Value;
        }
    }
}