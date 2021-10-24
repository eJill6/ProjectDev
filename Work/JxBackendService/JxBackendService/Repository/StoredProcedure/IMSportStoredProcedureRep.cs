using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMSportStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMSportStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMSport;

        public override string VIPFlowProductTableName => "VIPFlowProductIMSportLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMSportLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMSport";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMSport";

        protected override string RptProfitLossCurrentTableName => "RPT_IMSportProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMSportProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMSportMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMSportMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMSportMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMSportMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMSportTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMSportTransferRollback";

        protected override string TransferInSpName => "Pro_IMSportTransferIn";

        protected override string TransferOutSpName => "Pro_IMSportTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMSportAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMSportFreezeScores";

        protected override string PlayInfoTableName => "IMSportPlayInfo";

        protected override string ProfitlossTableName => "IMSportProfitloss";
    }
}
