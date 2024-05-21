using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IM;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMMoneyOutInfo";

        public override string MoneyInInfoTableName => "IMMoneyInInfo";

        public override string MoneyOutInfoTableName => "IMMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMTransferRollback";

        protected override string TransferInSpName => "Pro_IMTransferIn";

        protected override string TransferOutSpName => "Pro_IMTransferOut";

        public override string PlayInfoTableName => "IMPlayInfo";

        public override string ProfitlossTableName => "IMProfitloss";
    }
}