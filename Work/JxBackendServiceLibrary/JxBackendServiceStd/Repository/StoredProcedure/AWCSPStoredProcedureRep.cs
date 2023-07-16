using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class AWCSPStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public AWCSPStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.AWCSP;

        public override string DWDailyProfitLossTableName => "DW_Daily_AWCSPProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchAWCSPMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchAWCSPMoneyOutInfo";

        public override string MoneyInInfoTableName => "AWCSPMoneyInInfo";

        public override string MoneyOutInfoTableName => "AWCSPMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_AWCSPTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_AWCSPTransferRollback";

        protected override string TransferInSpName => "Pro_AWCSPTransferIn";

        protected override string TransferOutSpName => "Pro_AWCSPTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddAWCSPProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "AWCSPPlayInfo";

        public override string ProfitlossTableName => "AWCSPProfitloss";
    }
}