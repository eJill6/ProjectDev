using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMSGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMSGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMSGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMSGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMSGMoneyOutInfo";

        public override string MoneyInInfoTableName => "IMSGMoneyInInfo";

        public override string MoneyOutInfoTableName => "IMSGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMSGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMSGTransferRollback";

        protected override string TransferInSpName => "Pro_IMSGTransferIn";

        protected override string TransferOutSpName => "Pro_IMSGTransferOut";

        public override string PlayInfoTableName => "IMSGPlayInfo";

        public override string ProfitlossTableName => "IMSGProfitloss";
    }
}