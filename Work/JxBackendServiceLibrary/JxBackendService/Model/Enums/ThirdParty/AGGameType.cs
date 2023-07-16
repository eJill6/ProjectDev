using JxBackendService.Model.Enums.MiseOrder;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class AGGameType : BaseStringValueModel<AGGameType>
    {
        public MiseOrderGameId OrderGameId { get; private set; }

        private AGGameType()
        { }

        /// <summary> AG </summary>
        public static readonly AGGameType AGIN = new AGGameType()
        {
            Value = "AGIN",
            OrderGameId = MiseOrderGameId.AG,
        };

        /// <summary> 電子 </summary>
        public static readonly AGGameType XIN = new AGGameType()
        {
            Value = "XIN",
            OrderGameId = MiseOrderGameId.AGXin,
        };

        /// <summary> 捕魚 </summary>
        public static readonly AGGameType HUNTER = new AGGameType()
        {
            Value = "HUNTER",
            OrderGameId = MiseOrderGameId.AGFishing,
        };

        /// <summary> 街機 </summary>
        public static readonly AGGameType YOPLAY = new AGGameType()
        {
            Value = "YOPLAY",
            OrderGameId = MiseOrderGameId.AGYoPlay,
        };
    }
}