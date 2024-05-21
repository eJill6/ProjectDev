using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;

namespace JxBackendService.Model.StoredProcedureParam.Finance
{
    public class ProProcessCMoneyInParam
    {
        public int UserID { get; set; }

        [NVarcharColumnInfo(50)]
        public string OrderID { get; set; }

        public int MoneyInDealType { get; private set; }

        [NVarcharColumnInfo(50)]
        public string Handler { get; set; }

        [NVarcharColumnInfo(1024)]
        public string Memo { get; set; }

        public int BudgetType { get; private set; }

        [VarcharColumnInfo(32)]
        public string BudgetID { get; set; }

        [NVarcharColumnInfo(50)]
        public string ProfitLossType { get; private set; }

        [VarcharColumnInfo(32)]
        public string ProfitLossID { get; set; }

        [VarcharColumnInfo(6)]
        public string RC_Success => ReturnCode.Success.Value;

        [VarcharColumnInfo(6)]
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;

        [VarcharColumnInfo(6)]
        public string RC_SystemError => ReturnCode.SystemError.Value;

        public void SetMoneyInDealType(MoneyInDealType moneyInDealType)
        {
            MoneyInDealType = moneyInDealType.Value;
        }

        public void SetProfitLossType(ProfitLossTypeName profitLossType)
        {
            ProfitLossType = profitLossType.Value;
        }

        public void SetBudgetType(BudgetType budgetType)
        {
            BudgetType = budgetType.Value;
        }
    }

    public class ProcessCMoneyInParam
    {
        public int UserID { get; set; }

        public BudgetType BudgetType { get; set; }

        public MoneyInDealType MoneyInDealType { get; set; }

        public ProfitLossTypeName ProfitLossType { get; set; }

        public string OrderID { get; set; }
    }
}