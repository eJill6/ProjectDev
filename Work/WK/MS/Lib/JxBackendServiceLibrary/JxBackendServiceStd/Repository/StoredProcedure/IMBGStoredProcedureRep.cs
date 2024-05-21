using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMBGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMBGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMBG;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMBGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMBGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMBGMoneyOutInfo";

        public override string MoneyInInfoTableName => "IMBGMoneyInInfo";

        public override string MoneyOutInfoTableName => "IMBGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMBGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMBGTransferRollback";

        protected override string TransferInSpName => "Pro_IMBGTransferIn";

        protected override string TransferOutSpName => "Pro_IMBGTransferOut";

        public override string PlayInfoTableName => "IMBGPlayInfo";

        public override string ProfitlossTableName => "IMBGProfitloss";
    }
}