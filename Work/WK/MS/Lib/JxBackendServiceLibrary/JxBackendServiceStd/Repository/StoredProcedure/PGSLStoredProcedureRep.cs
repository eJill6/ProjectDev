using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class PGSLStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public PGSLStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.PGSL;

        public override string DWDailyProfitLossTableName => "DW_Daily_PGSLProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchPGSLMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchPGSLMoneyOutInfo";

        public override string MoneyInInfoTableName => "PGSLMoneyInInfo";

        public override string MoneyOutInfoTableName => "PGSLMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_PGSLTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_PGSLTransferRollback";

        protected override string TransferInSpName => "Pro_PGSLTransferIn";

        protected override string TransferOutSpName => "Pro_PGSLTransferOut";

        public override string PlayInfoTableName => "PGSLPlayInfo";

        public override string ProfitlossTableName => "PGSLProfitloss";
    }
}