using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMKYStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMKYStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMKY;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMKYProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMKYMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMKYMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMKYMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMKYMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMKYTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMKYTransferRollback";

        protected override string TransferInSpName => "Pro_IMKYTransferIn";

        protected override string TransferOutSpName => "Pro_IMKYTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMKYProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "IMKYPlayInfo";

        public override string ProfitlossTableName => "IMKYProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value.Substring(0, 1);
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }
    }
}