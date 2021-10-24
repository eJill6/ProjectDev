using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class OBSPStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OBSPStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.OBSP;

        public override string VIPFlowProductTableName => "VIPFlowProductOBSPLog";

        public override string VIPPointsProductTableName => "VIPPointsProductOBSPLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_OBSP";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_OBSP";

        protected override string RptProfitLossCurrentTableName => "RPT_OBSPProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_OBSPProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchOBSPMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchOBSPMoneyOutInfo";

        protected override string MoneyInInfoTableName => "OBSPMoneyInInfo";

        protected override string MoneyOutInfoTableName => "OBSPMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_OBSPTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_OBSPTransferRollback";

        protected override string TransferInSpName => "Pro_OBSPTransferIn";

        protected override string TransferOutSpName => "Pro_OBSPTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddOBSPProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "OBSPAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "OBSPFreezeScores";

        protected override string PlayInfoTableName => "OBSPPlayInfo";

        protected override string ProfitlossTableName => "OBSPProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value.Substring(0, 1);
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix.Substring(0, 1)}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }
    }
}
