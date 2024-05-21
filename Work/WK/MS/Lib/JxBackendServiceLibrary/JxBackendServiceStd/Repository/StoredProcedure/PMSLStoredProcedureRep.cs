using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class PMSLStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public PMSLStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.PMSL;

        public override string DWDailyProfitLossTableName => "DW_Daily_PMSLProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchPMSLMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchPMSLMoneyOutInfo";

        public override string MoneyInInfoTableName => "PMSLMoneyInInfo";

        public override string MoneyOutInfoTableName => "PMSLMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_PMSLTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_PMSLTransferRollback";

        protected override string TransferInSpName => "Pro_PMSLTransferIn";

        protected override string TransferOutSpName => "Pro_PMSLTransferOut";

        public override string PlayInfoTableName => "PMSLPlayInfo";

        public override string ProfitlossTableName => "PMSLProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            long timestamp = DateTime.Now.ToUnixOfTime();
            return $"{tpGameAccount}:{timestamp}";
        }
    }
}