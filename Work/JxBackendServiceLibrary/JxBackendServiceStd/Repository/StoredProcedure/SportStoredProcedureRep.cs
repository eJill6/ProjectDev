using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class SportStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public SportStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.Sport;

        public override string DWDailyProfitLossTableName => "DW_Daily_SportProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchSportMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchSportMoneyOutInfo";

        public override string MoneyInInfoTableName => "SportMoneyInInfo";

        public override string MoneyOutInfoTableName => "SportMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_SportTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_SportTransferRollback";

        protected override string TransferInSpName => "Pro_SportTransferIn";

        protected override string TransferOutSpName => "Pro_SportTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddSportProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "SportPlayInfo";

        public override string ProfitlossTableName => "SportProfitloss";

        private readonly int _extractLength = 4;

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string actionCode = ConvertToActionCode(isDeposit);
            string yyMMdd = DateTime.Now.ToFormatLastYearMonthDateValue();
            string fullOrderSeq = moneyId.PadLeft(_extractLength, '0');

            return $"{OrderIdPrefix}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{yyMMdd}{fullOrderSeq}";
        }
    }
}