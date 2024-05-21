using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class FYESStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public FYESStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.FYES;

        public override string DWDailyProfitLossTableName => "DW_Daily_FYESProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchFYESMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchFYESMoneyOutInfo";

        public override string MoneyInInfoTableName => "FYESMoneyInInfo";

        public override string MoneyOutInfoTableName => "FYESMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_FYESTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_FYESTransferRollback";

        protected override string TransferInSpName => "Pro_FYESTransferIn";

        protected override string TransferOutSpName => "Pro_FYESTransferOut";

        public override string PlayInfoTableName => "FYESPlayInfo";

        public override string ProfitlossTableName => "FYESProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value.Substring(0, 1);
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }
    }
}