using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.PM.Base
{
    public class PMResponseCode : BaseIntValueModel<PMResponseCode>
    {
        private PMResponseCode()
        { }

        /// <summary>成功</summary>
        public static readonly PMResponseCode Success = new PMResponseCode() { Value = 1000 };

        /// <summary>無數據</summary>
        public static readonly PMResponseCode NoDataFound = new PMResponseCode() { Value = 0 };

        /// <summary>下分失败,玩家正在游戏中</summary>
        public static readonly PMResponseCode NoWithdrawalInGame = new PMResponseCode() { Value = 3015 };
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public class PMTransferOrderStatus : BaseIntValueModel<PMTransferOrderStatus>
    {
        private PMTransferOrderStatus()
        { }

        public static PMTransferOrderStatus Success = new PMTransferOrderStatus() { Value = 0 };

        public static PMTransferOrderStatus Fail = new PMTransferOrderStatus() { Value = 1 };

        public static PMTransferOrderStatus Notfound = new PMTransferOrderStatus() { Value = 2 };
    }

    public class PMOpreationOrderStatus : BaseIntValueModel<PMOpreationOrderStatus>
    {
        private PMOpreationOrderStatus()
        { }

        public static PMOpreationOrderStatus Success = new PMOpreationOrderStatus() { Value = 1000 };

        public static PMOpreationOrderStatus Error = new PMOpreationOrderStatus() { Value = 1001 };

        public static PMOpreationOrderStatus ParseError = new PMOpreationOrderStatus() { Value = 1002 };

        public static PMOpreationOrderStatus Timeout = new PMOpreationOrderStatus() { Value = 1003 };
    }
}