using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMPPStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMPPStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMPP;

        public override string VIPFlowProductTableName => "VIPFlowProductIMPPLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMPPLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMPP";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMPP";

        protected override string RptProfitLossCurrentTableName => "RPT_IMPPProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMPPProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMPPMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMPPMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMPPMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMPPMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMPPTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMPPTransferRollback";

        protected override string TransferInSpName => "Pro_IMPPTransferIn";

        protected override string TransferOutSpName => "Pro_IMPPTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMPPAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMPPFreezeScores";

        protected override string PlayInfoTableName => "IMPPPlayInfo";

        protected override string ProfitlossTableName => "IMPPProfitloss";
    }
}
