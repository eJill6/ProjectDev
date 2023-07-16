using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class BTISStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public BTISStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.BTIS;

        public override string DWDailyProfitLossTableName => "DW_Daily_BTISProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchBTISMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchBTISMoneyOutInfo";

        public override string MoneyInInfoTableName => "BTISMoneyInInfo";

        public override string MoneyOutInfoTableName => "BTISMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_BTISTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_BTISTransferRollback";

        protected override string TransferInSpName => "Pro_BTISTransferIn";

        protected override string TransferOutSpName => "Pro_BTISTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddBTISProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "BTISPlayInfo";

        public override string ProfitlossTableName => "BTISProfitloss";
    }
}