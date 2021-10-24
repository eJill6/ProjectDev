using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMVRStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMVRStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMVR;

        public override string VIPFlowProductTableName => "VIPFlowProductIMVRLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMVRLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IMVR";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IMVR";

        protected override string RptProfitLossCurrentTableName => "RPT_IMVRProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMVRProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMVRMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMVRMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMVRMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMVRMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMVRTransferSuccessV2";

        protected override string TransferRollbackSpName => "Pro_IMVRTransferRollbackV2";

        protected override string TransferInSpName => "Pro_IMVRTransferInV2";

        protected override string TransferOutSpName => "Pro_IMVRTransferOutV2";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddIMVRProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "IMVRAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMVRFreezeScores";

        protected override string PlayInfoTableName => "IMVRPlayInfo";

        protected override string ProfitlossTableName => "IMVRProfitloss";
    }
}
