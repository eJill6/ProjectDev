using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.PM.PMBG
{
    public class PMBGResponseCode : BaseIntValueModel<PMBGResponseCode>
    {
        private PMBGResponseCode()
        { }

        /// <summary>成功</summary>
        public static readonly PMBGResponseCode Success = new PMBGResponseCode() { Value = 1000 };

        /// <summary>無數據</summary>
        public static readonly PMBGResponseCode NoDataFound = new PMBGResponseCode() { Value = 0 };
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public class PMBGTransferOrderStatus : BaseIntValueModel<PMBGTransferOrderStatus>
    {
        private PMBGTransferOrderStatus()
        { }

        public static PMBGTransferOrderStatus Success = new PMBGTransferOrderStatus() { Value = 0 };

        public static PMBGTransferOrderStatus Fail = new PMBGTransferOrderStatus() { Value = 1 };

        public static PMBGTransferOrderStatus Notfound = new PMBGTransferOrderStatus() { Value = 2 };
    }

    public class PMBGOpreationOrderStatus : BaseIntValueModel<PMBGOpreationOrderStatus>
    {
        private PMBGOpreationOrderStatus()
        { }

        public static PMBGOpreationOrderStatus Success = new PMBGOpreationOrderStatus() { Value = 1000 };

        public static PMBGOpreationOrderStatus Error = new PMBGOpreationOrderStatus() { Value = 1001 };

        public static PMBGOpreationOrderStatus ParseError = new PMBGOpreationOrderStatus() { Value = 1002 };

        public static PMBGOpreationOrderStatus Timeout = new PMBGOpreationOrderStatus() { Value = 1003 };
    }
}