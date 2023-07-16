using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMPTStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMPTStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMPT;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMPTProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMPTMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMPTMoneyOutInfo";

        public override string MoneyInInfoTableName => "IMPTMoneyInInfo";

        public override string MoneyOutInfoTableName => "IMPTMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMPTTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMPTTransferRollback";

        protected override string TransferInSpName => "Pro_IMPTTransferIn";

        protected override string TransferOutSpName => "Pro_IMPTTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMPTProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "IMPTPlayInfo";

        public override string ProfitlossTableName => "IMPTProfitloss";
    }
}