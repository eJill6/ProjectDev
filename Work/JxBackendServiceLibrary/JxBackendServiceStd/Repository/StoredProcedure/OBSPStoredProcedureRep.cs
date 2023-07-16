using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class OBSPStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OBSPStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.OBSP;

        public override string DWDailyProfitLossTableName => "DW_Daily_OBSPProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchOBSPMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchOBSPMoneyOutInfo";

        public override string MoneyInInfoTableName => "OBSPMoneyInInfo";

        public override string MoneyOutInfoTableName => "OBSPMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_OBSPTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_OBSPTransferRollback";

        protected override string TransferInSpName => "Pro_OBSPTransferIn";

        protected override string TransferOutSpName => "Pro_OBSPTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddOBSPProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "OBSPPlayInfo";

        public override string ProfitlossTableName => "OBSPProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value.Substring(0, 1);
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }
    }
}