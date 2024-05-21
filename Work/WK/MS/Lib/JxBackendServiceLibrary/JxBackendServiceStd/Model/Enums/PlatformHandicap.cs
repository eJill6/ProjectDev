using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class PlatformHandicap : BaseStringValueModel<PlatformHandicap>
    {
        /// <summary>賠率標準值</summary>
        public decimal StandardOdds { get; private set; }

        /// <summary>賠率上限值</summary>
        public decimal MaxOdds { get; private set; }

        private PlatformHandicap(string value, decimal standardOdds, decimal maxOdds)
        {
            Value = value;
            StandardOdds = standardOdds;
            MaxOdds = maxOdds;
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>特殊盘口,不属于任何一种市场盘口</summary>
        public static readonly PlatformHandicap NotBelongAnyMarket = new PlatformHandicap("NotBelongAnyMarket", 0m, 0m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_NotBelongAnyMarket)
        };

        /// <summary>香港</summary>
        public static readonly PlatformHandicap HongKong = new PlatformHandicap("HongKong", 0.6m, 0.75m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_HongKong)
        };

        /// <summary>歐洲</summary>
        public static readonly PlatformHandicap Europe = new PlatformHandicap("Europe", 1.6m, 1.75m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_Europe)
        };

        /// <summary>馬來西亞</summary>
        public static readonly PlatformHandicap Malaysia = new PlatformHandicap("Malaysia", 0.6m, 0.75m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_Malaysia)
        };

        /// <summary>印尼</summary>
        public static readonly PlatformHandicap Indonesia = new PlatformHandicap("Indonesia", -1.67m, -1.33m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_Indonesia)
        };

        /// <summary>美國</summary>
        public static readonly PlatformHandicap America = new PlatformHandicap("America", -167m, -133m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_America)
        };

        /// <summary>中國</summary>
        public static readonly PlatformHandicap China = new PlatformHandicap("China", 0.6m, 0.75m)
        {
            ResourcePropertyName = nameof(SelectItemElement.PlatformHandicap_China)
        };
    }
}