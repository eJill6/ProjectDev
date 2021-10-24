using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMeBETStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMeBETStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMeBET;

        public override string VIPFlowProductTableName => "VIPFlowProductIMeBETLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMeBETLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMeBET";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMeBET";

        protected override string RptProfitLossCurrentTableName => "RPT_IMeBetProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMeBetProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMeBETMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMeBETMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMeBETMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMeBETMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMeBETTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMeBETTransferRollback";

        protected override string TransferInSpName => "Pro_IMeBETTransferIn";

        protected override string TransferOutSpName => "Pro_IMeBETTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMeBETAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMeBETFreezeScores";

        protected override string PlayInfoTableName => "IMeBETPlayInfo";

        protected override string ProfitlossTableName => "IMeBETProfitloss";
    }
}
