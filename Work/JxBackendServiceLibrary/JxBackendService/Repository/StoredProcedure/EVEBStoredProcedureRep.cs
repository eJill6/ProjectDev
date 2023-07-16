using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class EVEBStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public EVEBStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.EVEB;

        public override string DWDailyProfitLossTableName => "DW_Daily_EVEBProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchEVEBMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchEVEBMoneyOutInfo";

        protected override string MoneyInInfoTableName => "EVEBMoneyInInfo";

        protected override string MoneyOutInfoTableName => "EVEBMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_EVEBTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_EVEBTransferRollback";

        protected override string TransferInSpName => "Pro_EVEBTransferIn";

        protected override string TransferOutSpName => "Pro_EVEBTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddEVEBProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "EVEBPlayInfo";

        public override string ProfitlossTableName => "EVEBProfitloss";
    }
}