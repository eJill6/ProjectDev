using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.CQ9SL
{
    public class CQ9Code : BaseStringValueModel<CQ9Code>
    {
        private CQ9Code()
        { }

        public static readonly CQ9Code Success = new CQ9Code() { Value = "0" };

        public static readonly CQ9Code DataNotFound = new CQ9Code() { Value = "8" };
    }

    public class CQ9TransactionAction : BaseStringValueModel<CQ9TransactionAction>
    {
        private CQ9TransactionAction()
        { }
    }

    public class CQ9TransactionStatus : BaseStringValueModel<CQ9TransactionStatus>
    {
        private CQ9TransactionStatus()
        { }

        public static readonly CQ9TransactionStatus Success = new CQ9TransactionStatus() { Value = "success" };

        public static readonly CQ9TransactionStatus Failed = new CQ9TransactionStatus() { Value = "failed" };

        public static readonly CQ9TransactionStatus Pending = new CQ9TransactionStatus() { Value = "pending" };
    }

    public class CQ9GameType : BaseStringValueModel<CQ9GameType>
    {
        public MiseOrderGameId OrderGameId { get; private set; }

        private CQ9GameType()
        {
            ResourceType = typeof(PlatformProductElement);
        }

        public static readonly CQ9GameType Slot = new CQ9GameType()
        {
            Value = "slot",
            OrderGameId = MiseOrderGameId.CQ9Slot,
        };

        public static readonly CQ9GameType Arcade = new CQ9GameType()
        {
            Value = "arcade",
            OrderGameId = MiseOrderGameId.CQ9Slot,
        };

        public static readonly CQ9GameType Table = new CQ9GameType()
        {
            Value = "table",
            OrderGameId = MiseOrderGameId.CQ9Table,
        };

        public static readonly CQ9GameType Fish = new CQ9GameType()
        {
            Value = "fish",
            OrderGameId = MiseOrderGameId.CQ9Fish,
        };
    }
}