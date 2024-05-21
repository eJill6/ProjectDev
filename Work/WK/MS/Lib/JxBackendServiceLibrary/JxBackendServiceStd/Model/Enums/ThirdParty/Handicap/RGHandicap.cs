namespace JxBackendService.Model.Enums.ThirdParty.Handicap
{
    public class RGHandicap : BaseStringValueModel<RGHandicap>
    {
        public PlatformHandicap PlatformHandicap { get; private set; }

        private RGHandicap(string value)
        {
            Value = value;
        }

        /// <summary>歐洲</summary>
        public static readonly RGHandicap Europe = new RGHandicap("Europe")
        {
            PlatformHandicap = PlatformHandicap.Europe
        };
    }
}
