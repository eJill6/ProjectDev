using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.OB.OBFI
{
    public class OBSPReturnCode : BaseStringValueModel<OBSPReturnCode>
    {
        private OBSPReturnCode()
        { }

        public static OBSPReturnCode Success = new OBSPReturnCode() { Value = "0000" };

        public static OBSPReturnCode UserIsNotExist = new OBSPReturnCode() { Value = "2002" };

        //API 有回覆msg 故這邊不再宣告其他代碼
    }

    public class OBSPOrderStatus : BaseIntValueModel<OBSPOrderStatus>
    {
        private OBSPOrderStatus()
        { }

        /// <summary>待處理</summary>
        public static OBSPOrderStatus Unprocessed = new OBSPOrderStatus()
        {
            Value = 0,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus0)
        };

        /// <summary>已結算</summary>
        public static OBSPOrderStatus Done = new OBSPOrderStatus()
        {
            Value = 1,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus1)
        };

        /// <summary>取消(人工)</summary>
        public static OBSPOrderStatus ManualCancel = new OBSPOrderStatus()
        {
            Value = 2,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus2)
        };

        /// <summary>待確認</summary>
        public static OBSPOrderStatus Confirming = new OBSPOrderStatus()
        {
            Value = 3,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus3)
        };

        /// <summary>風控拒單</summary>
        public static OBSPOrderStatus Risk = new OBSPOrderStatus()
        {
            Value = 4,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus4)
        };

        /// <summary>撤單(賽事取消)</summary>
        public static OBSPOrderStatus SystemCancel = new OBSPOrderStatus()
        {
            Value = 5,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.OBSPOrderStatus5)
        };
    }

    public class OBSPTerminalType : BaseStringValueModel<OBSPTerminalType>
    {
        private OBSPTerminalType()
        { }

        public static OBSPTerminalType PC = new OBSPTerminalType()
        {
            Value = "pc"
        };

        public static OBSPTerminalType Mobile = new OBSPTerminalType()
        {
            Value = "mobile"
        };
    }

    /// <summary>
    /// 訂單結算結果
    /// </summary>
    public class OBSPOutcome : BaseIntValueModel<OBSPOutcome>
    {
        public BetResultType BetResultType { get; private set; }

        private OBSPOutcome()
        { }

        public static readonly OBSPOutcome NoResult = new OBSPOutcome()
        {
            Value = 0,
            BetResultType = BetResultType.Draw
        };

        public static readonly OBSPOutcome RefundCapital = new OBSPOutcome()
        {
            Value = 2,
            BetResultType = BetResultType.Draw
        };

        public static readonly OBSPOutcome Lose = new OBSPOutcome()
        {
            Value = 3,
            BetResultType = BetResultType.Lose
        };

        public static readonly OBSPOutcome Win = new OBSPOutcome()
        {
            Value = 4,
            BetResultType = BetResultType.Win
        };

        public static readonly OBSPOutcome HalfWin = new OBSPOutcome()
        {
            Value = 5,
            BetResultType = BetResultType.HalfWin
        };

        public static readonly OBSPOutcome HalfLose = new OBSPOutcome()
        {
            Value = 6,
            BetResultType = BetResultType.HalfLose
        };
    }
}