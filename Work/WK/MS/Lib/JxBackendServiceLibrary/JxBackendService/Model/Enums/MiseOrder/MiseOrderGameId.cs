using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Resource.Element;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums.MiseOrder
{
    /// <summary>
    /// 查詢注單api gameId的對應列舉(訂單第3層)
    /// </summary>
    public class MiseOrderGameId : BaseStringValueModel<MiseOrderGameId>
    {
        /// <summary>平台產品代碼</summary>
        public PlatformProduct Product { get; private set; }

        public MiseOrderSubType OrderSubType { get; private set; }

        /// <summary>
        /// 目前參考 FrontsideMenu內的GameCode
        /// </summary>
        public string SubGameCode
        {
            get
            {
                if (SubGameCodeType == null)
                {
                    return string.Empty;
                }

                return SubGameCodeType.Value;
            }
        }

        public ThirdPartySubGameCodes SubGameCodeType { get; private set; }

        /// <summary>ctor</summary>
        private MiseOrderGameId()
        {
            ResourceType = typeof(PlatformProductElement);
        }

        /// <summary>一分快三</summary>
        public static readonly MiseOrderGameId OMKS = new MiseOrderGameId()
        {
            Value = "65",
            OrderSubType = MiseOrderSubType.YFKS,
            Product = PlatformProduct.Lottery,
            ResourcePropertyName = nameof(PlatformProductElement.OMKS),
        };

        /// <summary>一分快车</summary>
        public static readonly MiseOrderGameId OMPK10 = new MiseOrderGameId()
        {
            Value = "66",
            OrderSubType = MiseOrderSubType.YFKC,
            Product = PlatformProduct.Lottery,
            ResourcePropertyName = nameof(PlatformProductElement.OMPK10),
        };

        /// <summary>一分时时彩</summary>
        public static readonly MiseOrderGameId OMSSC = new MiseOrderGameId()
        {
            Value = "67",
            OrderSubType = MiseOrderSubType.YFSSC,
            Product = PlatformProduct.Lottery,
            ResourcePropertyName = nameof(PlatformProductElement.OMSSC),
        };

        /// <summary>一分六合彩</summary>
        public static readonly MiseOrderGameId OMLHC = new MiseOrderGameId()
        {
            Value = "68",
            OrderSubType = MiseOrderSubType.YFLHC,
            Product = PlatformProduct.Lottery,
            ResourcePropertyName = nameof(PlatformProductElement.OMLHC),
        };

        /// <summary>开元棋牌</summary>
        public static readonly MiseOrderGameId IMKY = new MiseOrderGameId()
        {
            Value = "imky",
            OrderSubType = MiseOrderSubType.IMKY,
            Product = PlatformProduct.IMKY,
            ResourcePropertyName = nameof(PlatformProductElement.IMKY),
        };

        /// <summary>JDB捕鱼</summary>
        public static readonly MiseOrderGameId JDBFI = new MiseOrderGameId()
        {
            Value = "jdbfi",
            OrderSubType = MiseOrderSubType.JDBFI,
            Product = PlatformProduct.JDBFI,
            ResourcePropertyName = nameof(PlatformProductElement.JDBFI),
        };

        /// <summary>PM真人</summary>
        public static readonly MiseOrderGameId OBEB = new MiseOrderGameId()
        {
            Value = "obeb",
            OrderSubType = MiseOrderSubType.OBEB,
            Product = PlatformProduct.OBEB,
            ResourcePropertyName = nameof(PlatformProductElement.OBEB),
        };

        /// <summary>JDB电子</summary>
        public static readonly MiseOrderGameId IMJDB = new MiseOrderGameId()
        {
            Value = "imjdb",
            OrderSubType = MiseOrderSubType.IMJDB,
            Product = PlatformProduct.IMPP,
            SubGameCodeType = ThirdPartySubGameCodes.IMJDB,
            ResourcePropertyName = nameof(PlatformProductElement.IMJDB),
        };

        /// <summary>AG真人</summary>
        public static readonly MiseOrderGameId AG = new MiseOrderGameId()
        {
            Value = "ag",
            OrderSubType = MiseOrderSubType.AG,
            Product = PlatformProduct.AG,
            SubGameCodeType = ThirdPartySubGameCodes.AG,
            ResourcePropertyName = nameof(PlatformProductElement.AG),
        };

        /// <summary>AG捕鱼王</summary>
        public static readonly MiseOrderGameId AGFishing = new MiseOrderGameId()
        {
            Value = "agfishing",
            OrderSubType = MiseOrderSubType.AGFishing,
            Product = PlatformProduct.AG,
            SubGameCodeType = ThirdPartySubGameCodes.AGFishing,
            ResourcePropertyName = nameof(PlatformProductElement.AGFishing),
        };

        /// <summary>AG电子</summary>
        public static readonly MiseOrderGameId AGXin = new MiseOrderGameId()
        {
            Value = "agxin",
            OrderSubType = MiseOrderSubType.AGXin,
            Product = PlatformProduct.AG,
            SubGameCodeType = ThirdPartySubGameCodes.AGXin,
            ResourcePropertyName = nameof(PlatformProductElement.AGXin),
        };

        /// <summary>AG街机</summary>
        public static readonly MiseOrderGameId AGYoPlay = new MiseOrderGameId()
        {
            Value = "agyoplay",
            OrderSubType = MiseOrderSubType.AGYoPlay,
            Product = PlatformProduct.AG,
            SubGameCodeType = ThirdPartySubGameCodes.AGYoPlay,
            ResourcePropertyName = nameof(PlatformProductElement.AGYoPlay),
        };

        /// <summary>瓦力游戏</summary>
        public static readonly MiseOrderGameId WLBG = new MiseOrderGameId()
        {
            Value = "wlbg",
            OrderSubType = MiseOrderSubType.WLBG,
            Product = PlatformProduct.WLBG,
            ResourcePropertyName = nameof(PlatformProductElement.WLBG),
        };

        /// <summary>IM体育</summary>
        public static readonly MiseOrderGameId IMSport = new MiseOrderGameId()
        {
            Value = "imsport",
            OrderSubType = MiseOrderSubType.IMSport,
            Product = PlatformProduct.IMSport,
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
        };

        /// <summary>双赢彩票</summary>
        public static readonly MiseOrderGameId IMSG = new MiseOrderGameId()
        {
            Value = "imsg",
            OrderSubType = MiseOrderSubType.IMSG,
            Product = PlatformProduct.IMSG,
            ResourcePropertyName = nameof(PlatformProductElement.IMSG),
        };

        /// <summary>VR彩票</summary>
        public static readonly MiseOrderGameId IMVR = new MiseOrderGameId()
        {
            Value = "imvr",
            OrderSubType = MiseOrderSubType.IMVR,
            Product = PlatformProduct.IMVR,
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
        };

        /// <summary>LC棋牌</summary>
        public static readonly MiseOrderGameId LC = new MiseOrderGameId()
        {
            Value = "lc",
            OrderSubType = MiseOrderSubType.LC,
            Product = PlatformProduct.LC,
            ResourcePropertyName = nameof(PlatformProductElement.LC),
        };

        /// <summary>PM体育</summary>
        public static readonly MiseOrderGameId OBSP = new MiseOrderGameId()
        {
            Value = "obsp",
            OrderSubType = MiseOrderSubType.OBSP,
            Product = PlatformProduct.OBSP,
            ResourcePropertyName = nameof(PlatformProductElement.OBSP),
        };

        /// <summary>PG电子</summary>
        public static readonly MiseOrderGameId PGSL = new MiseOrderGameId()
        {
            Value = "pgsl",
            OrderSubType = MiseOrderSubType.PGSL,
            Product = PlatformProduct.PGSL,
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
        };

        /// <summary>EBET真人</summary>
        public static readonly MiseOrderGameId IMeBET = new MiseOrderGameId()
        {
            Value = "imebet",
            OrderSubType = MiseOrderSubType.IMeBET,
            Product = PlatformProduct.IMeBET,
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
        };

        /// <summary>SE电子</summary>
        public static readonly MiseOrderGameId IMSE = new MiseOrderGameId()
        {
            Value = "imse",
            OrderSubType = MiseOrderSubType.IMSE,
            Product = PlatformProduct.IMPP,
            SubGameCodeType = ThirdPartySubGameCodes.IMSE,
            ResourcePropertyName = nameof(PlatformProductElement.IMSE),
        };

        /// <summary>泛亞電競</summary>
        public static readonly MiseOrderGameId FYES = new MiseOrderGameId()
        {
            Value = "fyes",
            OrderSubType = MiseOrderSubType.FYES,
            Product = PlatformProduct.FYES,
            ResourcePropertyName = nameof(PlatformProductElement.FYES),
        };

        /// <summary>IM棋牌</summary>
        public static readonly MiseOrderGameId IMBG = new MiseOrderGameId()
        {
            Value = "imbg",
            OrderSubType = MiseOrderSubType.IMBG,
            Product = PlatformProduct.IMBG,
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
        };

        /// <summary>IM電競</summary>
        public static readonly MiseOrderGameId IM = new MiseOrderGameId()
        {
            Value = "im",
            OrderSubType = MiseOrderSubType.IM,
            Product = PlatformProduct.IM,
            ResourcePropertyName = nameof(PlatformProductElement.IM),
        };

        /// <summary>AB真人</summary>
        public static readonly MiseOrderGameId ABEB = new MiseOrderGameId()
        {
            Value = "abeb",
            OrderSubType = MiseOrderSubType.ABEB,
            Product = PlatformProduct.ABEB,
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
        };

        /// <summary>EVO真人</summary>
        public static readonly MiseOrderGameId EVEB = new MiseOrderGameId()
        {
            Value = "eveb",
            OrderSubType = MiseOrderSubType.EVEB,
            Product = PlatformProduct.EVEB,
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
        };

        /// <summary>PT電遊</summary>
        public static readonly MiseOrderGameId IMPT = new MiseOrderGameId()
        {
            Value = "impt",
            OrderSubType = MiseOrderSubType.IMPT,
            Product = PlatformProduct.IMPT,
            SubGameCodeType = ThirdPartySubGameCodes.IMPT,
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
        };

        /// <summary>PT真人</summary>
        public static readonly MiseOrderGameId IMPTLIVE = new MiseOrderGameId()
        {
            Value = "imptlive",
            OrderSubType = MiseOrderSubType.IMPTLIVE,
            Product = PlatformProduct.IMPT,
            SubGameCodeType = ThirdPartySubGameCodes.IMPTLive,
            ResourcePropertyName = nameof(PlatformProductElement.IMPTLIVE),
        };

        /// <summary>PP電子</summary>
        public static readonly MiseOrderGameId IMPP = new MiseOrderGameId()
        {
            Value = "impp",
            OrderSubType = MiseOrderSubType.IMPP,
            Product = PlatformProduct.IMPP,
            SubGameCodeType = ThirdPartySubGameCodes.IMPP,
            ResourcePropertyName = nameof(PlatformProductElement.IMPP),
        };

        /// <summary>PM捕魚</summary>
        public static readonly MiseOrderGameId OBFI = new MiseOrderGameId()
        {
            Value = "obfi",
            OrderSubType = MiseOrderSubType.OBFI,
            Product = PlatformProduct.OBFI,
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
        };

        /// <summary>沙巴體育</summary>
        public static readonly MiseOrderGameId Sport = new MiseOrderGameId()
        {
            Value = "sport",
            OrderSubType = MiseOrderSubType.Sport,
            Product = PlatformProduct.Sport,
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
        };

        /// <summary>BTI體育</summary>
        public static readonly MiseOrderGameId BTIS = new MiseOrderGameId()
        {
            Value = "btis",
            OrderSubType = MiseOrderSubType.BTIS,
            Product = PlatformProduct.BTIS,
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
        };

        /// <summary>賽馬</summary>
        public static readonly MiseOrderGameId AWCHB = new MiseOrderGameId()
        {
            Value = "awchb",
            OrderSubType = MiseOrderSubType.AWCHB,
            Product = PlatformProduct.AWCSP,
            SubGameCodeType = ThirdPartySubGameCodes.AWCHB,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.HORSEBOOK),
        };

        /// <summary>鬥雞</summary>
        public static readonly MiseOrderGameId AWCSV = new MiseOrderGameId()
        {
            Value = "awcsv",
            OrderSubType = MiseOrderSubType.AWCSV,
            Product = PlatformProduct.AWCSP,
            SubGameCodeType = ThirdPartySubGameCodes.AWCSV,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SV388),
        };

        /// <summary>PM棋牌</summary>
        public static readonly MiseOrderGameId PMBG = new MiseOrderGameId()
        {
            Value = "pmbg",
            OrderSubType = MiseOrderSubType.PMBG,
            Product = PlatformProduct.PMBG,
            ResourcePropertyName = nameof(PlatformProductElement.PMBG),
        };

        /// <summary>PM電子</summary>
        public static readonly MiseOrderGameId PMSL = new MiseOrderGameId()
        {
            Value = "pmsl",
            OrderSubType = MiseOrderSubType.PMSL,
            Product = PlatformProduct.PMSL,
            ResourcePropertyName = nameof(PlatformProductElement.PMSL),
        };

        private static ConcurrentDictionary<PlatformProduct, HashSet<MiseOrderGameId>> s_ProductGameIdsMap = null;

        public static ConcurrentDictionary<PlatformProduct, HashSet<MiseOrderGameId>> GetProductGameIdMap()
        {
            if (s_ProductGameIdsMap == null)
            {
                s_ProductGameIdsMap = new ConcurrentDictionary<PlatformProduct, HashSet<MiseOrderGameId>>();

                foreach (MiseOrderGameId miseOrderGameId in GetAll())
                {
                    PlatformProduct product = miseOrderGameId.Product;

                    if (!s_ProductGameIdsMap.TryGetValue(product, out HashSet<MiseOrderGameId> miseOrderGameIdSet))
                    {
                        s_ProductGameIdsMap[product] = new HashSet<MiseOrderGameId>();
                    }

                    s_ProductGameIdsMap[product].Add(miseOrderGameId);
                }
            }

            return s_ProductGameIdsMap;
        }

        #region 暫時沒用到

        ///// <summary>透過productCode + gameCode找回原本的gameId</summary>
        //public static QueryOrderGameId GetSingle(string productCode, string gameCode)
        //{
        //    if (GetQueryOrderGameIdMap().TryGetValue(productCode + gameCode, out QueryOrderGameId queryOrderGameId))
        //    {
        //        throw new ArgumentOutOfRangeException($"{productCode}{gameCode} not found");
        //    }

        //    return queryOrderGameId;
        //}

        #endregion 暫時沒用到
    }
}