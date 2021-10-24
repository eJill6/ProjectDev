using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class BTISStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public BTISStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.BTIS;

        public override string VIPFlowProductTableName => "VIPFlowProductBTISLog";

        public override string VIPPointsProductTableName => "VIPPointsProductBTISLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_BTIS";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_BTIS";

        protected override string RptProfitLossCurrentTableName => "RPT_BTISProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_BTISProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchBTISMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchBTISMoneyOutInfo";

        protected override string MoneyInInfoTableName => "BTISMoneyInInfo";

        protected override string MoneyOutInfoTableName => "BTISMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_BTISTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_BTISTransferRollback";

        protected override string TransferInSpName => "Pro_BTISTransferIn";

        protected override string TransferOutSpName => "Pro_BTISTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddBTISProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "BTISAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "BTISFreezeScores";

        protected override string PlayInfoTableName => "BTISPlayInfo";

        protected override string ProfitlossTableName => "BTISProfitloss";

    }
}
