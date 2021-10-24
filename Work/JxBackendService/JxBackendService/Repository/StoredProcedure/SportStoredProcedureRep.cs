using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class SportStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public SportStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.Sport;

        public override string VIPFlowProductTableName => "VIPFlowProductSportLog";

        public override string VIPPointsProductTableName => "VIPPointsProductSportLog";

        protected override string ProfitLossCompareTimeColumnName => "BetTime";

        protected override TPGameProfitLossRowModelCompareTime CompareTimeProperty => TPGameProfitLossRowModelCompareTime.BetTime;

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_Sport";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_Sport";

        protected override string RptProfitLossCurrentTableName => "RPT_SportProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_SportProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchSportMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchSportMoneyOutInfo";

        protected override string MoneyInInfoTableName => "SportMoneyInInfo";

        protected override string MoneyOutInfoTableName => "SportMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_SportTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_SportTransferRollback";

        protected override string TransferInSpName => "Pro_SportTransferIn";

        protected override string TransferOutSpName => "Pro_SportTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "SPAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "SPFreezeScores";

        protected override string PlayInfoTableName => "SportPlayInfo";

        protected override string ProfitlossTableName => "SportProfitloss";
    }
}
