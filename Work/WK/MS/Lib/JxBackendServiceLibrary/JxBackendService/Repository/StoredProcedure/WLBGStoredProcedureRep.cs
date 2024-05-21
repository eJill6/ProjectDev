using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;

namespace JxBackendService.Repository.StoredProcedure
{
    public class WLBGStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public WLBGStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.WLBG;

        public override string DWDailyProfitLossTableName => "DW_Daily_WLBGProfitLoss";

        protected override string SearchUnprocessedMoneyInInfoSpName => "Pro_SearchWLBGMoneyInInfo";

        protected override string SearchUnprocessedMoneyOutInfoSpName => "Pro_SearchWLBGMoneyOutInfo";

        protected override string MoneyInInfoTableName => "WLBGMoneyInInfo";

        protected override string MoneyOutInfoTableName => "WLBGMoneyOutInfo";

        protected override string TransferSuccessSpName => "Pro_WLBGTransferSuccess";

        protected override string TransferRollbackSpName => "Pro_WLBGTransferRollback";

        protected override string TransferInSpName => "Pro_WLBGTransferIn";

        protected override string TransferOutSpName => "Pro_WLBGTransferOut";

        protected override string AddProfitlossAndPlayInfoSpName => "Pro_AddWLBGProfitLossAndPlayInfo";

        public override string PlayInfoTableName => "WLBGPlayInfo";

        public override string ProfitlossTableName => "WLBGProfitloss";

        protected override string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            //WLBG的orderId格式必须为${agentId}_${yyyyMMddHHmmssSSS}_${userId}
            string agentId = WLBGSharedAppSetting.AgentID;
            return $"{agentId}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_{tpGameAccount}";
        }
    }
}