using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class AWCSPPlatform : BaseStringValueModel<AWCSPPlatform>
    {
        public string GameCode { get; private set; }

        private AWCSPPlatform()
        { }

        public static readonly AWCSPPlatform HORSEBOOK = new AWCSPPlatform()
        {
            Value = "HORSEBOOK",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.HORSEBOOK),
            GameCode = "HRB-LIVE-001"
        };

        public static readonly AWCSPPlatform SV388 = new AWCSPPlatform()
        {
            Value = "SV388",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SV388),
            GameCode = "SV-LIVE-001"
        };
    }
}