using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class ProductTypeSetting : BaseIntValueModel<ProductTypeSetting>
    {
        /// <summary>是否計算有效投注額</summary>
        public bool IsComputeAdmissionBetMoneyByHandicap { get; private set; }

        public bool IsHideHeaderWithFullScreen { get; private set; }

        private ProductTypeSetting(int value)
        {
            Value = value;
            ResourceType = typeof(SelectItemElement);
        }

        ///<summary>自營平台彩票</summary>

        public static readonly ProductTypeSetting Lottery = new ProductTypeSetting(1)
        {
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_Lottery),
        };

        ///<summary>其他彩票</summary>
        public static readonly ProductTypeSetting OtherLottery = new ProductTypeSetting(2)
        {
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_OtherLottery),
        };

        ///<summary>真人</summary>
        public static readonly ProductTypeSetting LiveCasino = new ProductTypeSetting(3)
        {
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_LiveCasino),
            IsHideHeaderWithFullScreen = true
        };

        ///<summary>體育</summary>
        public static readonly ProductTypeSetting Sports = new ProductTypeSetting(4)
        {
            IsComputeAdmissionBetMoneyByHandicap = true,
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_Sports),
        };

        ///<summary>电子</summary>
        public static readonly ProductTypeSetting Slots = new ProductTypeSetting(5)
        {
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_Slots),
            IsHideHeaderWithFullScreen = true
        };

        ///<summary>棋牌</summary>
        public static readonly ProductTypeSetting BoardGame = new ProductTypeSetting(6)
        {
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_BoardGame),
            IsHideHeaderWithFullScreen = true
        };

        ///<summary>电竞</summary>
        public static readonly ProductTypeSetting ESports = new ProductTypeSetting(7)
        {
            IsComputeAdmissionBetMoneyByHandicap = true,
            ResourcePropertyName = nameof(SelectItemElement.ProductTypeSetting_ESports),
        };
    }
}