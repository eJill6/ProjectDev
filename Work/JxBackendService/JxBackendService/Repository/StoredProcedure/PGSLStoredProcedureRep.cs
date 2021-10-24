using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class PGSLStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public PGSLStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.PGSL;

        public override string VIPFlowProductTableName => "VIPFlowProductPGSLLog";

        public override string VIPPointsProductTableName => "VIPPointsProductPGSLLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_PGSL";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_PGSL";

        protected override string RptProfitLossCurrentTableName => "RPT_PGSLProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_PGSLProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchPGSLMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchPGSLMoneyOutInfo";

        protected override string MoneyInInfoTableName => "PGSLMoneyInInfo";

        protected override string MoneyOutInfoTableName => "PGSLMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_PGSLTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_PGSLTransferRollback";

        protected override string TransferInSpName => "Pro_PGSLTransferIn";

        protected override string TransferOutSpName => "Pro_PGSLTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddPGSLProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "PGSLAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "PGSLFreezeScores";

        protected override string PlayInfoTableName => "PGSLPlayInfo";

        protected override string ProfitlossTableName => "PGSLProfitloss";

    }
}
