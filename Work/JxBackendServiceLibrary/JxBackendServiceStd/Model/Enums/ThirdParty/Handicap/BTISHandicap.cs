namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class BTISHandicap : BaseStringValueModel<BTISHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private BTISHandicap(string value)
        {
            Value = value;
        }

        /// <summary>香港</summary>
        public static readonly BTISHandicap Hongkong = new BTISHandicap("Hongkong")
        {
            PlatformHandicap = PlatformHandicap.HongKong
        };

        /// <summary>歐洲</summary>
        public static readonly BTISHandicap European = new BTISHandicap("European")
        {
            PlatformHandicap = PlatformHandicap.Europe
        };

        /// <summary>馬來</summary>
        public static readonly BTISHandicap Malaysia = new BTISHandicap("Malay")
        {
            PlatformHandicap = PlatformHandicap.Malaysia
        };
    }
}