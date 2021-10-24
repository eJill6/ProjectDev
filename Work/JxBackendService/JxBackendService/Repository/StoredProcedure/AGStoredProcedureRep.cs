using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class AGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public AGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }
        
        public override PlatformProduct Product => PlatformProduct.AG;

        public override string VIPFlowProductTableName => "VIPFlowProductAGLog";

        public override string VIPPointsProductTableName => "VIPPointsProductAGLog";

        protected override string ProfitLossCompareTimeColumnName => "BetTime";

        protected override TPGameProfitLossRowModelCompareTime CompareTimeProperty => TPGameProfitLossRowModelCompareTime.BetTime;

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_AG";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_AG";

        protected override string RptProfitLossCurrentTableName => "RPT_AGProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_AGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchAgMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchAgMoneyOutInfo";

        protected override string MoneyInInfoTableName => "GameMoneyInInfo";

        protected override string MoneyOutInfoTableName => "GameMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_AgTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_AgTransferRollback";

        protected override string TransferInSpName => "Pro_AgTransferIn";

        protected override string TransferOutSpName => "Pro_AgTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "AGAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "AGFreezeScores";

        protected override string PlayInfoTableName => "AGPlayInfo";

        protected override string ProfitlossTableName => "AGProfitloss";

        protected override bool IsMoneyIdNumeric => true;
    }
}
