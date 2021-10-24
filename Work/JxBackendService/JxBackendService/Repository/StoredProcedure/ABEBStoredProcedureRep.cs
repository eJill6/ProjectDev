using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class ABEBStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public ABEBStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.ABEB;

        public override string VIPFlowProductTableName => "VIPFlowProductABEBLog";

        public override string VIPPointsProductTableName => "VIPPointsProductABEBLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_ABEB";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_ABEB";

        protected override string RptProfitLossCurrentTableName => "RPT_ABEBProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_ABEBProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchABEBMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchABEBMoneyOutInfo";

        protected override string MoneyInInfoTableName => "ABEBMoneyInInfo";

        protected override string MoneyOutInfoTableName => "ABEBMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_ABEBTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_ABEBTransferRollback";

        protected override string TransferInSpName => "Pro_ABEBTransferIn";

        protected override string TransferOutSpName => "Pro_ABEBTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddABEBProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "ABEBAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "ABEBFreezeScores";

        protected override string PlayInfoTableName => "ABEBPlayInfo";

        protected override string ProfitlossTableName => "ABEBProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string propertyId = ABEBSharedAppSettings.Instance.PropertyId;
            string actionCode = ConvertToActionCode(isDeposit);
            string seq = moneyId.Substring(8);
            return $"{propertyId}{OrderIdPrefix}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{seq}";
        }
    }
}
