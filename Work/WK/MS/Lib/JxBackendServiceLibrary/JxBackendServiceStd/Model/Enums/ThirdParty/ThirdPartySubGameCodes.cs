using JxBackendService.Model.Common;
using JxBackendService.Resource.Element;
using System.Collections.Generic;
using System.Linq;

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
            RemoteGameCode = "1",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AG),
        };

        public static readonly ThirdPartySubGameCodes AGFishing = new ThirdPartySubGameCodes()
        {
            Value = "AGFishing",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "6",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGFishing),
        };

        public static readonly ThirdPartySubGameCodes AGYoPlay = new ThirdPartySubGameCodes()
        {
            Value = "AGYoPlay",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "YP800",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGYoPlay),
        };

        public static readonly ThirdPartySubGameCodes AGXin = new ThirdPartySubGameCodes()
        {
            Value = "AGXin",
            PlatformProduct = PlatformProduct.AG,
            RemoteGameCode = "8",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGXin),
        };

        public static readonly ThirdPartySubGameCodes IMPTLive = new ThirdPartySubGameCodes()
        {
            Value = "IMPTLive",
            PlatformProduct = PlatformProduct.IMPT,
            RemoteGameCode = "7bal",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPTLIVE),
        };

        public static readonly ThirdPartySubGameCodes IMJDB = new ThirdPartySubGameCodes()
        {
            Value = "IMJDB",
            PlatformProduct = PlatformProduct.IMPP,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.JUMBO_SLOT)
        };

        public static readonly ThirdPartySubGameCodes IMSE = new ThirdPartySubGameCodes()
        {
            Value = "IMSE",
            PlatformProduct = PlatformProduct.IMPP,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SPRIBE_SLOT)
        };

        public static readonly ThirdPartySubGameCodes IMPP = new ThirdPartySubGameCodes()
        {
            Value = "IMPP",
            PlatformProduct = PlatformProduct.IMPP,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.PRAGMATIC_SLOT)
        };

        public static readonly ThirdPartySubGameCodes IMPT = new ThirdPartySubGameCodes()
        {
            Value = "IMPT",
            PlatformProduct = PlatformProduct.IMPT,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
        };

        public static readonly ThirdPartySubGameCodes WLFI = new ThirdPartySubGameCodes()
        {
            Value = "WLFI",
            PlatformProduct = PlatformProduct.WLBG,
            RemoteGameCode = WaliGameCode.WLFI.Value,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.WLGameID1),
        };

        /// <summary> 賽馬 </summary>
        public static readonly ThirdPartySubGameCodes AWCHB = new ThirdPartySubGameCodes()
        {
            Value = "AWCHB",
            PlatformProduct = PlatformProduct.AWCSP,
            RemoteGameCode = AWCSPPlatform.HORSEBOOK.Value,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.HORSEBOOK),
        };

        /// <summary> 鬥雞 </summary>
        public static readonly ThirdPartySubGameCodes AWCSV = new ThirdPartySubGameCodes()
        {
            Value = "AWCSV",
            PlatformProduct = PlatformProduct.AWCSP,
            RemoteGameCode = AWCSPPlatform.SV388.Value,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SV388),
        };

        /// <summary> CQ9电子 </summary>
        public static readonly ThirdPartySubGameCodes CQ9Slot = new ThirdPartySubGameCodes()
        {
            Value = "CQ9Slot",
            PlatformProduct = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Slot),
        };

        /// <summary> CQ9棋牌 </summary>
        public static readonly ThirdPartySubGameCodes CQ9Table = new ThirdPartySubGameCodes()
        {
            Value = "CQ9Table",
            PlatformProduct = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Table),
        };

        /// <summary> CQ9捕鱼 </summary>
        public static readonly ThirdPartySubGameCodes CQ9Fish = new ThirdPartySubGameCodes()
        {
            Value = "CQ9Fish",
            PlatformProduct = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Fish),
        };

        #region 自有彩票

        public static readonly ThirdPartySubGameCodes LotteryOMKS = new ThirdPartySubGameCodes()
        {
            Value = "LotteryOMKS",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "65",
            Sort = 65,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMKS),
        };

        public static readonly ThirdPartySubGameCodes LotteryOMPK10 = new ThirdPartySubGameCodes()
        {
            Value = "LotteryOMPK10",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "66",
            Sort = 66,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMPK10),
        };

        public static readonly ThirdPartySubGameCodes LotteryOMSSC = new ThirdPartySubGameCodes()
        {
            Value = "LotteryOMSSC",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "67",
            Sort = 67,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMSSC),
        };

        public static readonly ThirdPartySubGameCodes LotteryOMLHC = new ThirdPartySubGameCodes()
        {
            Value = "LotteryOMLHC",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "68",
            Sort = 68,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMLHC),
        };

        public static readonly ThirdPartySubGameCodes LotteryNUINUI = new ThirdPartySubGameCodes()
        {
            Value = "LotteryNUINUI",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "69",
            Sort = 69,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_NUINUI),
        };

        public static readonly ThirdPartySubGameCodes LotteryJSBaccarat = new ThirdPartySubGameCodes()
        {
            Value = "LotteryJSBaccarat",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "70",
            Sort = 70,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSBaccarat),
        };

        public static readonly ThirdPartySubGameCodes LotteryJSLP = new ThirdPartySubGameCodes()
        {
            Value = "LotteryJSLP",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "71",
            Sort = 71,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSLP),
        };

        public static readonly ThirdPartySubGameCodes LotteryJSYXX = new ThirdPartySubGameCodes()
        {
            Value = "LotteryJSYXX",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "72",
            Sort = 72,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSYXX),
        };

        public static readonly ThirdPartySubGameCodes LotteryJSSG = new ThirdPartySubGameCodes()
        {
            Value = "LotteryJSSG",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "73",
            Sort = 73,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSSG),
        };

        public static readonly ThirdPartySubGameCodes LotteryJSLH = new ThirdPartySubGameCodes()
        {
            Value = "LotteryJSLH",
            PlatformProduct = PlatformProduct.Lottery,
            RemoteGameCode = "74",
            Sort = 74,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSLH),
        };

        #endregion 自有彩票

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

        public static List<JxBackendSelectListItem> GetSelectListItems(PlatformProduct product, bool hasBlankOption, bool? isSubGameOptionOfHotGameVislble)
        {
            if (product == null || (isSubGameOptionOfHotGameVislble.HasValue && product.IsSubGameOptionOfHotGameVislble != isSubGameOptionOfHotGameVislble))
            {
                return new List<JxBackendSelectListItem>();
            }

            List<ThirdPartySubGameCodes> thirdPartySubGameCodes = GetAll().Where(w => w.PlatformProduct == product).ToList();

            return GetSelectListItems(thirdPartySubGameCodes, hasBlankOption);
        }
    }
}