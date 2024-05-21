using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class PMBGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public PMBGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.PMBG;

        public override string DWDailyProfitLossTableName => "DW_Daily_PMBGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchPMBGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchPMBGMoneyOutInfo";

        public override string MoneyInInfoTableName => "PMBGMoneyInInfo";

        public override string MoneyOutInfoTableName => "PMBGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_PMBGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_PMBGTransferRollback";

        protected override string TransferInSpName => "Pro_PMBGTransferIn";

        protected override string TransferOutSpName => "Pro_PMBGTransferOut";

        public override string PlayInfoTableName => "PMBGPlayInfo";

        public override string ProfitlossTableName => "PMBGProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            long timestamp = DateTime.Now.ToUnixOfTime();
            return $"{tpGameAccount}:{timestamp}";
        }
    }
}