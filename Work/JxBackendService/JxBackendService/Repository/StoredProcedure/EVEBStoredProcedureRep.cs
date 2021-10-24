using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class EVEBStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public EVEBStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.EVEB;

        public override string VIPFlowProductTableName => "VIPFlowProductEVEBLog";

        public override string VIPPointsProductTableName => "VIPPointsProductEVEBLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_EVEB";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_EVEB";

        protected override string RptProfitLossCurrentTableName => "RPT_EVEBProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_EVEBProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchEVEBMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchEVEBMoneyOutInfo";

        protected override string MoneyInInfoTableName => "EVEBMoneyInInfo";

        protected override string MoneyOutInfoTableName => "EVEBMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_EVEBTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_EVEBTransferRollback";

        protected override string TransferInSpName => "Pro_EVEBTransferIn";

        protected override string TransferOutSpName => "Pro_EVEBTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddEVEBProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "EVEBAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "EVEBFreezeScores";

        protected override string PlayInfoTableName => "EVEBPlayInfo";

        protected override string ProfitlossTableName => "EVEBProfitloss";

    }
}
