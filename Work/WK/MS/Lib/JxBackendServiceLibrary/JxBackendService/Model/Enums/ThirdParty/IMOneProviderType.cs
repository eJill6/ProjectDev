using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class IMOneProviderType : BaseStringValueModel<IMOneProviderType>
    {
        public MiseOrderGameId OrderGameId { get; private set; }

        private IMOneProviderType()
        { }

        public static readonly IMOneProviderType IMPP = new IMOneProviderType()
        {
            Value = "PRAGMATIC_SLOT",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.PRAGMATIC_SLOT),
            OrderGameId = MiseOrderGameId.IMPP,
        };

        public static readonly IMOneProviderType IMJDB = new IMOneProviderType()
        {
            Value = "JUMBO_SLOT",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.JUMBO_SLOT),
            OrderGameId = MiseOrderGameId.IMJDB,
        };

        public static readonly IMOneProviderType IMSE = new IMOneProviderType()
        {
            Value = "SPRIBE_SLOT",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SPRIBE_SLOT),
            OrderGameId = MiseOrderGameId.IMSE,
        };
    }
}