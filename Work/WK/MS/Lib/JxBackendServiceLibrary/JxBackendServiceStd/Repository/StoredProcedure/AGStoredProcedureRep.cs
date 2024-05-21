using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class AGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public AGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.AG;

        public override string DWDailyProfitLossTableName => "DW_Daily_AGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchAGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchAGMoneyOutInfo";

        public override string MoneyInInfoTableName => "AGMoneyInInfo";

        public override string MoneyOutInfoTableName => "AGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_AGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_AGTransferRollback";

        protected override string TransferInSpName => "Pro_AGTransferIn";

        protected override string TransferOutSpName => "Pro_AGTransferOut";

        public override string PlayInfoTableName => "AGPlayInfo";

        public override string ProfitlossTableName => "AGProfitloss";

        private readonly int _padLength = 5;

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string actionCode = ConvertToActionCode(isDeposit);
            string yyMMdd = DateTime.Now.ToFormatLastYearMonthDateValue();
            string fullOrderSeq = moneyId.PadLeft(_padLength, '0');

            return $"{OrderIdPrefix}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{yyMMdd}{fullOrderSeq}";
        }
    }
}