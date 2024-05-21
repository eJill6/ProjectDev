using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMeBETStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMeBETStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMeBET;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMeBetProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMeBETMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMeBETMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMeBETMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMeBETMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMeBETTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMeBETTransferRollback";

        protected override string TransferInSpName => "Pro_IMeBETTransferIn";

        protected override string TransferOutSpName => "Pro_IMeBETTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMeBETProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "IMeBETPlayInfo";

        public override string ProfitlossTableName => "IMeBETProfitloss";
    }
}