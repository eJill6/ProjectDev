using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMVRStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMVRStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMVR;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMVRProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMVRMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMVRMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMVRMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMVRMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMVRTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMVRTransferRollback";

        protected override string TransferInSpName => "Pro_IMVRTransferIn";

        protected override string TransferOutSpName => "Pro_IMVRTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMVRProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "IMVRPlayInfo";

        public override string ProfitlossTableName => "IMVRProfitloss";
    }
}