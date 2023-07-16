using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.MiseOrder
{
    /// <summary>
    /// 訂單第1層分類
    /// </summary>
    public class MiseOrderType : BaseStringValueModel<MiseOrderType>
    {
        private MiseOrderType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>祕色彩票</summary>
        public static readonly MiseOrderType MiseLottery = new MiseOrderType()
        {
            Value = "mscp",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_MiseLottery),
        };

        /// <summary>第三方彩票</summary>
        public static readonly MiseOrderType TPLottery = new MiseOrderType()
        {
            Value = "tpcp",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPLottery),
        };

        /// <summary>真人</summary>
        public static readonly MiseOrderType TPLive = new MiseOrderType()
        {
            Value = "tpeb",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPLive),
        };

        /// <summary>体育</summary>
        public static readonly MiseOrderType TPSport = new MiseOrderType()
        {
            Value = "tpsp",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPSport),
        };

        /// <summary>捕鱼</summary>
        public static readonly MiseOrderType TPFish = new MiseOrderType()
        {
            Value = "tpfi",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPFish),
        };

        /// <summary>电子</summary>
        public static readonly MiseOrderType TPSlots = new MiseOrderType()
        {
            Value = "tpsl",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPSlots),
        };

        /// <summary>棋牌</summary>
        public static readonly MiseOrderType TPBoardGame = new MiseOrderType()
        {
            Value = "tpbg",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPBoardGame),
        };

        /// <summary>電競</summary>
        public static readonly MiseOrderType TPESport = new MiseOrderType()
        {
            Value = "tpesp",
            ResourcePropertyName = nameof(SelectItemElement.MiseOrderType_TPESport),
        };
    }
}