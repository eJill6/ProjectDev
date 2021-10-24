using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMPTStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMPTStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMPT;

        public override string VIPFlowProductTableName => "VIPFlowProductIMPTLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMPTLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMPT";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMPT";

        protected override string RptProfitLossCurrentTableName => "RPT_IMPTProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMPTProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMPTMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMPTMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMPTMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMPTMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMPTTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMPTTransferRollback";

        protected override string TransferInSpName => "Pro_IMPTTransferIn";

        protected override string TransferOutSpName => "Pro_IMPTTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMPTAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMPTFreezeScores";

        protected override string PlayInfoTableName => "IMPTPlayInfo";

        protected override string ProfitlossTableName => "IMPTProfitloss";
    }
}
