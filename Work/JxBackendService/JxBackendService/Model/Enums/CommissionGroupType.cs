using System;
using JxBackendService.Repository.Game;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class CommissionGroupType : BaseStringValueModel<CommissionGroupType>
    {
        public Type CommissionRuleInfoRepType { get; private set; }

        /// <summary>
        /// 對應的產品類別
        /// </summary>
        public ProductTypes[] CalculateProductTypes { get; set; }

        private CommissionGroupType() { }

        /// <summary>
        /// AMD彩票
        /// </summary>
        public static readonly CommissionGroupType PlatformLottery = new CommissionGroupType()
        {
            Value = "Lottery",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_PlatformLottery),
            CommissionRuleInfoRepType = typeof(LotteryCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.Lottery },
            Sort = 1
        };

        /// <summary>
        /// 其它彩票
        /// </summary>
        public static readonly CommissionGroupType OtherLottery = new CommissionGroupType()
        {
            Value = "OtherLottery",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_OtherLottery),
            CommissionRuleInfoRepType = typeof(OtherLotteryCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.OtherLottery },
            Sort = 2
        };

        /// <summary>
        /// 真人
        /// </summary>
        public static readonly CommissionGroupType Live = new CommissionGroupType()
        {
            Value = "Live",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_Live),
            CommissionRuleInfoRepType = typeof(LiveCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.LiveCasino },
            Sort = 3
        };

        /// <summary>
        /// 體育電競
        /// </summary>
        public static readonly CommissionGroupType ESport = new CommissionGroupType()
        {
            Value = "ESport",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_ESport),
            CommissionRuleInfoRepType = typeof(ESportCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.Sports, ProductTypes.ESports },
            Sort = 4
        };

        /// <summary>
        /// 電子
        /// </summary>
        public static readonly CommissionGroupType Slot = new CommissionGroupType()
        {
            Value = "Slot",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_Slot),
            CommissionRuleInfoRepType = typeof(SlotCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.Slots },
            Sort = 5
        };

        /// <summary>
        /// 棋牌
        /// </summary>
        public static readonly CommissionGroupType BoardGame = new CommissionGroupType()
        {
            Value = "BoardGame",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_BoardGame),
            CommissionRuleInfoRepType = typeof(BoardGameCommissionRuleInfoRep),
            CalculateProductTypes = new ProductTypes[] { ProductTypes.BoardGame },
            Sort = 6
        };
    }
}
