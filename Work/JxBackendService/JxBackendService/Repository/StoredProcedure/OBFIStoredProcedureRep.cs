using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class OBFIStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OBFIStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.OBFI;

        public override string VIPFlowProductTableName => "VIPFlowProductOBFILog";

        public override string VIPPointsProductTableName => "VIPPointsProductOBFILog";

        protected override string GetProductInloTotalProfitLossSpName => "Pro_GetProductInloTotalProfitLoss_OBFI";

        protected override string GetProductUserDailyTotalProfitLossSpName => "Pro_GetProductUserDailyTotalProfitLoss_OBFI";

        protected override string RptProfitLossCurrentTableName => "RPT_OBFIProfitLoss_Current";

        protected override string DWDailyProfitLossTableName => "DW_Daily_OBFIProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchOBFIMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchOBFIMoneyOutInfo";

        protected override string MoneyInInfoTableName => "OBFIMoneyInInfo";

        protected override string MoneyOutInfoTableName => "OBFIMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_OBFITransferSuccess";

        protected override string TransferRollbackSpName => "Pro_OBFITransferRollback";

        protected override string TransferInSpName => "Pro_OBFITransferIn";

        protected override string TransferOutSpName => "Pro_OBFITransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddOBFIProfitLossAndPlayInfo";

        protected override string DwDailyAvailableScoresColumn => "OBFIAvailableScores";

        protected override string DwDailyFreezeScoresColumn => "OBFIFreezeScores";

        protected override string PlayInfoTableName => "OBFIPlayInfo";

        protected override string ProfitlossTableName => "OBFIProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            long timestamp = DateTime.Now.ToUnixOfTime();
            return $"{tpGameAccount}:{timestamp}";
        }
    }
}
