using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMSportStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMSportStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMSport;

        public override string DWDailyProfitLossTableName => "DW_Daily_IMSportProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMSportMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMSportMoneyOutInfo";

        public override string MoneyInInfoTableName => "IMSportMoneyInInfo";

        public override string MoneyOutInfoTableName => "IMSportMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMSportTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMSportTransferRollback";

        protected override string TransferInSpName => "Pro_IMSportTransferIn";

        protected override string TransferOutSpName => "Pro_IMSportTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMSportProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "IMSportPlayInfo";

        public override string ProfitlossTableName => "IMSportProfitloss";
    }
}