namespace JxBackendService.Model.Enums.ThirdParty
{
    //存放第三方登入時有子遊戲入口的GameCode
    public class ThirdPartySubGameCodes : BaseStringValueModel<ThirdPartySubGameCodes>
    {
        private ThirdPartySubGameCodes()
        { }

        public PlatformProduct PlatformProduct { get; private set; }

        /// <summary>確認多個相同的PlatformProduct集合中，哪一個是主要的</summary>
        public bool IsPrimary => (Value == PlatformProduct.Value);

        //要傳給第三方的真實GameCode，此值是於 20210129 跟 App Team確認的
        public string RemoteGameCode { get; private set; }

        public static readonly ThirdPartySubGameCodes AG = new ThirdPartySubGameCodes()
        {
            Value = "AG",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "1"
        };

        public static readonly ThirdPartySubGameCodes AGFishing = new ThirdPartySubGameCodes()
        {
            Value = "AGFishing",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "6"
        };

        public static readonly ThirdPartySubGameCodes AGYoPlay = new ThirdPartySubGameCodes()
        {
            Value = "AGYoPlay",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "YP800"
        };

        public static readonly ThirdPartySubGameCodes AGXin = new ThirdPartySubGameCodes()
        {
            Value = "AGXin",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "8"
        };

        public static readonly ThirdPartySubGameCodes IMPTLive = new ThirdPartySubGameCodes()
        {
            Value = "IMPTLive",
            PlatformProduct = PlatformProduct.IMPT,
            RemoteGameCode = "7bal"
        };

        public static readonly ThirdPartySubGameCodes IMJDB = new ThirdPartySubGameCodes()
        {
            Value = "IMJDB",
            PlatformProduct = PlatformProduct.IMPP,
        };

        public static readonly ThirdPartySubGameCodes IMSE = new ThirdPartySubGameCodes()
        {
            Value = "IMSE",
            PlatformProduct = PlatformProduct.IMPP,
        };

        public static readonly ThirdPartySubGameCodes IMPP = new ThirdPartySubGameCodes()
        {
            Value = "IMPP",
            PlatformProduct = PlatformProduct.IMPP,
            RemoteGameCode = "IMPP"
        };

        public static readonly ThirdPartySubGameCodes IMPT = new ThirdPartySubGameCodes()
        {
            Value = "IMPT",
            PlatformProduct = PlatformProduct.IMPT,
            RemoteGameCode = "IMPT"
        };

        public static readonly ThirdPartySubGameCodes WLFI = new ThirdPartySubGameCodes()
        {
            Value = "WLFI",
            PlatformProduct = PlatformProduct.WLBG,
            RemoteGameCode = WaliGameCode.WLFI.Value
        };

        /// <summary> 賽馬 </summary>
        public static readonly ThirdPartySubGameCodes AWCHB = new ThirdPartySubGameCodes()
        {
            Value = "AWCHB",
            PlatformProduct = PlatformProduct.AWCSP,
            RemoteGameCode = AWCSPPlatform.HORSEBOOK.Value
        };

        /// <summary> 鬥雞 </summary>
        public static readonly ThirdPartySubGameCodes AWCSV = new ThirdPartySubGameCodes()
        {
            Value = "AWCSV",
            PlatformProduct = PlatformProduct.AWCSP,
            RemoteGameCode = AWCSPPlatform.SV388.Value
        };

        public static readonly ThirdPartySubGameCodes PMSL = new ThirdPartySubGameCodes() { Value = "PMSL", PlatformProduct = PlatformProduct.PMSL, RemoteGameCode = "PMSL" };

        public static ThirdPartySubGameCodes GetSingle(string productCode, string gameCode)
        {
            ThirdPartySubGameCodes subGameCode = GetSingle(gameCode);

            if (subGameCode == null)
            {
                return null;
            }

            if (subGameCode.PlatformProduct.Value != productCode)
            {
                return null;
            }

            return subGameCode;
        }
    }
}