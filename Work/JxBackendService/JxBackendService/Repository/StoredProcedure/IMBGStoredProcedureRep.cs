using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMBGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMBGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMBG;

        public override string VIPFlowProductTableName => "VIPFlowProductIMBGLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMBGLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMBG";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMBG";

        protected override string RptProfitLossCurrentTableName => "RPT_IMBGProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMBGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMBGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMBGMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMBGMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMBGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMBGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMBGTransferRollback";

        protected override string TransferInSpName => "Pro_IMBGTransferIn";

        protected override string TransferOutSpName => "Pro_IMBGTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMBGAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMBGFreezeScores";

        protected override string PlayInfoTableName => "IMBGPlayInfo";

        protected override string ProfitlossTableName => "IMBGProfitloss";
    }
}
