﻿using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class IMStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public IMStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IM;

        public override string VIPFlowProductTableName => "VIPFlowProductIMLog";

        public override string VIPPointsProductTableName => "VIPPointsProductIMLog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_IM";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_IM";

        protected override string RptProfitLossCurrentTableName => "RPT_IMProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_IMProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchIMMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchIMMoneyOutInfo";

        protected override string MoneyInInfoTableName => "IMMoneyInInfo";

        protected override string MoneyOutInfoTableName => "IMMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_IMTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_IMTransferRollback";

        protected override string TransferInSpName => "Pro_IMTransferIn";

        protected override string TransferOutSpName => "Pro_IMTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => throw new System.NotImplementedException();

        protected override string DwDailyAvailableScoresColumn => "IMAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "IMFreezeScores";

        protected override string PlayInfoTableName => "IMPlayInfo";

        protected override string ProfitlossTableName => "IMProfitloss";
    }
}
