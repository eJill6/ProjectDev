using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class WaliGameCode : BaseStringValueModel<WaliGameCode>
    {
        //因目前瓦力的投注紀錄不分開查，所以MiseOrderGameId先暫不開放，
        //public MiseOrderGameId OrderGameId { get; private set; }
        public string GameCode { get; private set; }

        private WaliGameCode()
        { }

        public static readonly WaliGameCode WLBG = new WaliGameCode()
        {
            Value = "Default",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.WLBG),
            GameCode = PlatformProduct.WLBG.Value,
        };

        public static readonly WaliGameCode WLFI = new WaliGameCode()
        {
            Value = "1",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.WLGameID1),
            GameCode = "WLFI",//因投注紀錄目前不拆分，所以先用WLFI寫入DB，來區別，以利之後拆分使用
        };

        public static WaliGameCode GetSingleByTPGameCode(string value)
        {
            WaliGameCode providerType = GetSingle(value);

            if (providerType != null)
            {
                return providerType;
            }

            return WLBG;
        }
    }
}