using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMSGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMSGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override string VIPFlowProductTableName => "VIPFlowProductIMSGLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMSGLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMSG";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMSG";

        protected override string RptProfitLossCurrentTableName => "RPT_IMSGProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMSGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMSGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMSGMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMSGMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMSGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMSGTransferSuccessV2";

        protected override string TransferRollbackSpName => "Pro_IMSGTransferRollbackV2";

        protected override string TransferInSpName => "Pro_IMSGTransferInV2";

        protected override string TransferOutSpName => "Pro_IMSGTransferOutV2";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMSGProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "IMSGAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMSGFreezeScores";

        protected override string PlayInfoTableName => "IMSGPlayInfo";

        protected override string ProfitlossTableName => "IMSGProfitloss";
    }
}
