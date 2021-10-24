using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.OB.OBFI
{
    public class OBResponseCode : BaseIntValueModel<OBResponseCode>
    {
        private OBResponseCode() { }

        /// <summary>成功</summary>
        public static readonly OBResponseCode Success = new OBResponseCode() { Value = 1000 };

        /// <summary>無數據</summary>
        public static readonly OBResponseCode NoDataFound = new OBResponseCode() { Value = 0 };
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public class OBTransferOrderStatus : BaseIntValueModel<OBTransferOrderStatus>
    {
        private OBTransferOrderStatus() { }

        public static OBTransferOrderStatus Success = new OBTransferOrderStatus() { Value = 0 };
        public static OBTransferOrderStatus Fail = new OBTransferOrderStatus() { Value = 1 };
        public static OBTransferOrderStatus Notfound = new OBTransferOrderStatus() { Value = 2 };

    }

    public class OBOpreationOrderStatus : BaseIntValueModel<OBOpreationOrderStatus>
    {
        private OBOpreationOrderStatus() { }

        public static OBOpreationOrderStatus Success = new OBOpreationOrderStatus() { Value = 1000 };
        public static OBOpreationOrderStatus Error = new OBOpreationOrderStatus() { Value = 1001 };
        public static OBOpreationOrderStatus ParseError = new OBOpreationOrderStatus() { Value = 1002 };
        public static OBOpreationOrderStatus Timeout = new OBOpreationOrderStatus() { Value = 1003 };


    }

}
