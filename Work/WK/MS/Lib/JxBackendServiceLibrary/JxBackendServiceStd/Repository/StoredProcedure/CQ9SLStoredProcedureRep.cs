using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class CQ9SLStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public CQ9SLStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.CQ9SL;

        public override string DWDailyProfitLossTableName => "DW_Daily_CQ9SLProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchCQ9SLMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchCQ9SLMoneyOutInfo";

        public override string MoneyInInfoTableName => "CQ9SLMoneyInInfo";

        public override string MoneyOutInfoTableName => "CQ9SLMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_CQ9SLTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_CQ9SLTransferRollback";

        protected override string TransferInSpName => "Pro_CQ9SLTransferIn";

        protected override string TransferOutSpName => "Pro_CQ9SLTransferOut";

        public override string PlayInfoTableName => "CQ9SLPlayInfo";

        public override string ProfitlossTableName => "CQ9SLProfitloss";
    }
}