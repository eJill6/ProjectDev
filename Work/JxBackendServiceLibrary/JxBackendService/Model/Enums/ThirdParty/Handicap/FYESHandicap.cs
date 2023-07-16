namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class FYESHandicap : BaseStringValueModel<FYESHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private FYESHandicap(string value, PlatformHandicap platformHandicap)
        {
            Value = value;
            PlatformHandicap = platformHandicap;
        }

        /// <summary>歐洲</summary>
        public static readonly FYESHandicap EU = new FYESHandicap("EU", PlatformHandicap.Europe);

        /// <summary>香港</summary>
        public static readonly FYESHandicap HK = new FYESHandicap("HK", PlatformHandicap.HongKong);

        /// <summary>馬來</summary>
        public static readonly FYESHandicap MY = new FYESHandicap("MY", PlatformHandicap.Malaysia);

        /// <summary>印尼</summary>
        public static readonly FYESHandicap IN = new FYESHandicap("IN", PlatformHandicap.Indonesia);
    }
}