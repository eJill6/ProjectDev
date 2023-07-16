using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class JDBFIStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public JDBFIStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.JDBFI;

        public override string DWDailyProfitLossTableName => "DW_Daily_JDBFIProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchJDBFIMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchJDBFIMoneyOutInfo";

        protected override string MoneyInInfoTableName => "JDBFIMoneyInInfo";

        protected override string MoneyOutInfoTableName => "JDBFIMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_JDBFITransferSuccess";

        protected override string TransferRollbackSpName => "Pro_JDBFITransferRollback";

        protected override string TransferInSpName => "Pro_JDBFITransferIn";

        protected override string TransferOutSpName => "Pro_JDBFITransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddJDBFIProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "JDBFIPlayInfo";

        public override string ProfitlossTableName => "JDBFIProfitloss";
    }
}