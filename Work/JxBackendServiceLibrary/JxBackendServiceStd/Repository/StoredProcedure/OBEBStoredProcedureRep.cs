using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class OBEBStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OBEBStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.OBEB;

        public override string DWDailyProfitLossTableName => "DW_Daily_OBEBProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchOBEBMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchOBEBMoneyOutInfo";

        public override string MoneyInInfoTableName => "OBEBMoneyInInfo";

        public override string MoneyOutInfoTableName => "OBEBMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_OBEBTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_OBEBTransferRollback";

        protected override string TransferInSpName => "Pro_OBEBTransferIn";

        protected override string TransferOutSpName => "Pro_OBEBTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddOBEBProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "OBEBPlayInfo";

        public override string ProfitlossTableName => "OBEBProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value.Substring(0, 1);
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }
    }
}