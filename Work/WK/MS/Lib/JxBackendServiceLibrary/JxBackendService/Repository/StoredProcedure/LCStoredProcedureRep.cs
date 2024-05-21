using System;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.StoredProcedure
{
    public class LCStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public LCStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.LC;

        public override string DWDailyProfitLossTableName => "DW_Daily_LCProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchLCMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchLCMoneyOutInfo";

        protected override string MoneyInInfoTableName => "LCMoneyInInfo";

        protected override string MoneyOutInfoTableName => "LCMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_LCTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_LCTransferRollback";

        protected override string TransferInSpName => "Pro_LCTransferIn";

        protected override string TransferOutSpName => "Pro_LCTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddLCProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "LCPlayInfo";

        public override string ProfitlossTableName => "LCProfitloss";

        private readonly int _extractLength = 5;

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string actionCode = ConvertToActionCode(isDeposit);
            string yyMMdd = DateTime.Now.ToFormatLastYearMonthDateValue();
            string strMoneyId = moneyId.Substring(moneyId.Length - _extractLength, _extractLength); // 2021121700000215

            return $"{OrderIdPrefix}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{yyMMdd}{strMoneyId}";
        }
    }
}