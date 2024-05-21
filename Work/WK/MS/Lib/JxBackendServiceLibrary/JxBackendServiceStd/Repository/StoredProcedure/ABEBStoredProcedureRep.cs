using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class ABEBStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public ABEBStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.ABEB;

        public override string DWDailyProfitLossTableName => "DW_Daily_ABEBProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchABEBMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchABEBMoneyOutInfo";

        public override string MoneyInInfoTableName => "ABEBMoneyInInfo";

        public override string MoneyOutInfoTableName => "ABEBMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_ABEBTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_ABEBTransferRollback";

        protected override string TransferInSpName => "Pro_ABEBTransferIn";

        protected override string TransferOutSpName => "Pro_ABEBTransferOut";

        public override string PlayInfoTableName => "ABEBPlayInfo";

        public override string ProfitlossTableName => "ABEBProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string propertyId = ABEBSharedAppSetting.OperatorId;
            string actionCode = ConvertToActionCode(isDeposit);
            string seq = moneyId.Substring(8);
            return $"{propertyId}{OrderIdPrefix}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{seq}";
        }
    }
}