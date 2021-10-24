using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class LCStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public LCStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.LC;

        public override string VIPFlowProductTableName => "VIPFlowProductLCLog";

        public override string VIPPointsProductTableName => "VIPPointsProductLCLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_LC";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_LC";

        protected override string RptProfitLossCurrentTableName => "RPT_LCProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_LCProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchLCMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchLCMoneyOutInfo";

        protected override string MoneyInInfoTableName => "LCMoneyInInfo";

        protected override string MoneyOutInfoTableName => "LCMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_LCTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_LCTransferRollback";

        protected override string TransferInSpName => "Pro_LCTransferIn";

        protected override string TransferOutSpName => "Pro_LCTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "LCAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "LCFreezeScores";

        protected override string PlayInfoTableName => "LCPlayInfo";

        protected override string ProfitlossTableName => "LCProfitloss";
    }
}
