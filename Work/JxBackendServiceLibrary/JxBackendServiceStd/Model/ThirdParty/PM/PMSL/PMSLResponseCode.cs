using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.PM.PMSL
{
    public class PMSLResponseCode : BaseIntValueModel<PMSLResponseCode>
    {
        private PMSLResponseCode()
        { }

        /// <summary>成功</summary>
        public static readonly PMSLResponseCode Success = new PMSLResponseCode() { Value = 1000 };

        /// <summary>無數據</summary>
        public static readonly PMSLResponseCode NoDataFound = new PMSLResponseCode() { Value = 0 };
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public class PMSLTransferOrderStatus : BaseIntValueModel<PMSLTransferOrderStatus>
    {
        private PMSLTransferOrderStatus()
        { }

        public static PMSLTransferOrderStatus Success = new PMSLTransferOrderStatus() { Value = 0 };
        public static PMSLTransferOrderStatus Fail = new PMSLTransferOrderStatus() { Value = 1 };
        public static PMSLTransferOrderStatus Notfound = new PMSLTransferOrderStatus() { Value = 2 };
    }

    public class PMSLOpreationOrderStatus : BaseIntValueModel<PMSLOpreationOrderStatus>
    {
        private PMSLOpreationOrderStatus()
        { }

        public static PMSLOpreationOrderStatus Success = new PMSLOpreationOrderStatus() { Value = 1000 };
        public static PMSLOpreationOrderStatus Error = new PMSLOpreationOrderStatus() { Value = 1001 };
        public static PMSLOpreationOrderStatus ParseError = new PMSLOpreationOrderStatus() { Value = 1002 };
        public static PMSLOpreationOrderStatus Timeout = new PMSLOpreationOrderStatus() { Value = 1003 };
    }
}