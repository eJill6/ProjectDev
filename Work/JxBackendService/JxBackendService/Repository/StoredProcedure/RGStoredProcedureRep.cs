using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class RGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public RGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.RG;

        public override string VIPFlowProductTableName => "VIPFlowProductRGLog";

        public override string VIPPointsProductTableName => "VIPPointsProductRGLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_RG";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_RG";

        protected override string RptProfitLossCurrentTableName => "RPT_RGProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_RGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchRGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchRGMoneyOutInfo";

        protected override string MoneyInInfoTableName => "RGMoneyInInfo";

        protected override string MoneyOutInfoTableName => "RGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_RGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_RGTransferRollback";

        protected override string TransferInSpName => "Pro_RGTransferIn";

        protected override string TransferOutSpName => "Pro_RGTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "RGAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "RGFreezeScores";

        protected override string PlayInfoTableName => "RGPlayInfo";

        protected override string ProfitlossTableName => "RGProfitloss";
    }
}
