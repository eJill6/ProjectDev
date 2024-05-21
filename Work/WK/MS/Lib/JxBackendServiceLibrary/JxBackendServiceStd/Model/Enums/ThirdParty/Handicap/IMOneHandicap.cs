namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class IMOneHandicap : BaseStringValueModel<IMOneHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private IMOneHandicap(string value)
        {
            Value = value;
        }

        /// <summary>香港</summary>
        public static readonly IMOneHandicap HK = new IMOneHandicap("HK")
        {
            PlatformHandicap = PlatformHandicap.HongKong
        };

        /// <summary>歐洲</summary>
        public static readonly IMOneHandicap EURO = new IMOneHandicap("EURO")
        {
            PlatformHandicap = PlatformHandicap.Europe
        };

        /// <summary>馬來</summary>
        public static readonly IMOneHandicap MALAY = new IMOneHandicap("MALAY")
        {
            PlatformHandicap = PlatformHandicap.Malaysia
        };

        /// <summary>印尼</summary>
        public static readonly IMOneHandicap INDO = new IMOneHandicap("INDO")
        {
            PlatformHandicap = PlatformHandicap.Indonesia
        };
    }
}
