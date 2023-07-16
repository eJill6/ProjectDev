using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class OBFIStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OBFIStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.OBFI;

        public override string DWDailyProfitLossTableName => "DW_Daily_OBFIProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchOBFIMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchOBFIMoneyOutInfo";

        public override string MoneyInInfoTableName => "OBFIMoneyInInfo";

        public override string MoneyOutInfoTableName => "OBFIMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_OBFITransferSuccess";

        protected override string TransferRollbackSpName => "Pro_OBFITransferRollback";

        protected override string TransferInSpName => "Pro_OBFITransferIn";

        protected override string TransferOutSpName => "Pro_OBFITransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddOBFIProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "OBFIPlayInfo";

        public override string ProfitlossTableName => "OBFIProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            long timestamp = DateTime.Now.ToUnixOfTime();
            return $"{tpGameAccount}:{timestamp}";
        }
    }
}