using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.MiseOrder
{
    /// <summary>
    /// 訂單第2層分類
    /// </summary>
    public class MiseOrderSubType : BaseStringValueModel<MiseOrderSubType>
    {
        public MiseOrderType OrderType { get; private set; }

        private MiseOrderSubType()
        {
            ResourceType = typeof(PlatformProductElement);
        }

        /// <summary>一分快三</summary>
        public static readonly MiseOrderSubType YFKS = new MiseOrderSubType()
        {
            Value = "yfks",
            OrderType = MiseOrderType.MiseLottery
        };

        /// <summary>一分快车</summary>
        public static readonly MiseOrderSubType YFKC = new MiseOrderSubType()
        {
            Value = "yfkc",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>一分时时彩</summary>
        public static readonly MiseOrderSubType YFSSC = new MiseOrderSubType()
        {
            Value = "yfssc",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>一分六合彩</summary>
        public static readonly MiseOrderSubType YFLHC = new MiseOrderSubType()
        {
            Value = "yflhc",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>百人牛牛</summary>
        public static readonly MiseOrderSubType NUINUI = new MiseOrderSubType()
        {
            Value = "nuinui",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>极速百家乐</summary>
        public static readonly MiseOrderSubType JSBaccarat = new MiseOrderSubType()
        {
            Value = "jsbaccarat",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>极速轮盘</summary>
        public static readonly MiseOrderSubType JSLP = new MiseOrderSubType()
        {
            Value = "jslp",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>极速鱼虾蟹</summary>
        public static readonly MiseOrderSubType JSYXX = new MiseOrderSubType()
        {
            Value = "jsyxx",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>极速三公</summary>
        public static readonly MiseOrderSubType JSSG = new MiseOrderSubType()
        {
            Value = "jssg",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>极速龙虎</summary>
        public static readonly MiseOrderSubType JSLH = new MiseOrderSubType()
        {
            Value = "jslh",
            OrderType = MiseOrderType.MiseLottery,
        };

        /// <summary>开元棋牌</summary>
        public static readonly MiseOrderSubType IMKY = new MiseOrderSubType()
        {
            Value = "imky",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.IMKY),
        };

        /// <summary>JDB捕鱼</summary>
        public static readonly MiseOrderSubType JDBFI = new MiseOrderSubType()
        {
            Value = "jdbfi",
            OrderType = MiseOrderType.TPFish,
            ResourcePropertyName = nameof(PlatformProductElement.JDBFI),
        };

        /// <summary>PM真人</summary>
        public static readonly MiseOrderSubType OBEB = new MiseOrderSubType()
        {
            Value = "obeb",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.OBEB),
        };

        /// <summary>JDB电子</summary>
        public static readonly MiseOrderSubType IMJDB = new MiseOrderSubType()
        {
            Value = "imjdb",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.IMJDB),
        };

        /// <summary>AG真人</summary>
        public static readonly MiseOrderSubType AG = new MiseOrderSubType()
        {
            Value = "ag",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.AG),
        };

        /// <summary>AG捕鱼王</summary>
        public static readonly MiseOrderSubType AGFishing = new MiseOrderSubType()
        {
            Value = "agfishing",
            OrderType = MiseOrderType.TPFish,
            ResourcePropertyName = nameof(PlatformProductElement.AGFishing),
        };

        /// <summary>AG电子</summary>
        public static readonly MiseOrderSubType AGXin = new MiseOrderSubType()
        {
            Value = "agxin",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.AGXin),
        };

        /// <summary>AG街机</summary>
        public static readonly MiseOrderSubType AGYoPlay = new MiseOrderSubType()
        {
            Value = "agyoplay",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.AGYoPlay),
        };

        /// <summary>瓦力游戏</summary>
        public static readonly MiseOrderSubType WLBG = new MiseOrderSubType()
        {
            Value = "wlbg",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.WLBG),
        };

        /// <summary>IM体育</summary>
        public static readonly MiseOrderSubType IMSport = new MiseOrderSubType()
        {
            Value = "imsport",
            OrderType = MiseOrderType.TPSport,
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
        };

        /// <summary>双赢彩票</summary>
        public static readonly MiseOrderSubType IMSG = new MiseOrderSubType()
        {
            Value = "imsg",
            OrderType = MiseOrderType.TPLottery,
            ResourcePropertyName = nameof(PlatformProductElement.IMSG),
        };

        /// <summary>VR彩票</summary>
        public static readonly MiseOrderSubType IMVR = new MiseOrderSubType()
        {
            Value = "imvr",
            OrderType = MiseOrderType.TPLottery,
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
        };

        /// <summary>LC棋牌</summary>
        public static readonly MiseOrderSubType LC = new MiseOrderSubType()
        {
            Value = "lc",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.LC),
        };

        /// <summary>PM体育</summary>
        public static readonly MiseOrderSubType OBSP = new MiseOrderSubType()
        {
            Value = "obsp",
            OrderType = MiseOrderType.TPSport,
            ResourcePropertyName = nameof(PlatformProductElement.OBSP),
        };

        /// <summary>PG电子</summary>
        public static readonly MiseOrderSubType PGSL = new MiseOrderSubType()
        {
            Value = "pgsl",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
        };

        /// <summary>EBET真人</summary>
        public static readonly MiseOrderSubType IMeBET = new MiseOrderSubType()
        {
            Value = "imebet",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
        };

        /// <summary>SE电子</summary>
        public static readonly MiseOrderSubType IMSE = new MiseOrderSubType()
        {
            Value = "imse",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.IMSE),
        };

        /// <summary>泛亞電競</summary>
        public static readonly MiseOrderSubType FYES = new MiseOrderSubType()
        {
            Value = "fyes",
            OrderType = MiseOrderType.TPESport,
            ResourcePropertyName = nameof(PlatformProductElement.FYES),
        };

        /// <summary>IM棋牌</summary>
        public static readonly MiseOrderSubType IMBG = new MiseOrderSubType()
        {
            Value = "imbg",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
        };

        /// <summary>IM電競</summary>
        public static readonly MiseOrderSubType IM = new MiseOrderSubType()
        {
            Value = "im",
            OrderType = MiseOrderType.TPESport,
            ResourcePropertyName = nameof(PlatformProductElement.IM),
        };

        /// <summary>AB真人</summary>
        public static readonly MiseOrderSubType ABEB = new MiseOrderSubType()
        {
            Value = "abeb",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
        };

        /// <summary>EVO真人</summary>
        public static readonly MiseOrderSubType EVEB = new MiseOrderSubType()
        {
            Value = "eveb",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
        };

        /// <summary>PT電遊</summary>
        public static readonly MiseOrderSubType IMPT = new MiseOrderSubType()
        {
            Value = "impt",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
        };

        /// <summary>PT真人</summary>
        public static readonly MiseOrderSubType IMPTLIVE = new MiseOrderSubType()
        {
            Value = "imptlive",
            OrderType = MiseOrderType.TPLive,
            ResourcePropertyName = nameof(PlatformProductElement.IMPTLIVE),
        };

        /// <summary>PP電子</summary>
        public static readonly MiseOrderSubType IMPP = new MiseOrderSubType()
        {
            Value = "impp",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.IMPP),
        };

        /// <summary>PM捕魚</summary>
        public static readonly MiseOrderSubType OBFI = new MiseOrderSubType()
        {
            Value = "obfi",
            OrderType = MiseOrderType.TPFish,
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
        };

        /// <summary>沙巴體育</summary>
        public static readonly MiseOrderSubType Sport = new MiseOrderSubType()
        {
            Value = "sport",
            OrderType = MiseOrderType.TPSport,
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
        };

        /// <summary>BTI體育</summary>
        public static readonly MiseOrderSubType BTIS = new MiseOrderSubType()
        {
            Value = "btis",
            OrderType = MiseOrderType.TPSport,
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
        };

        /// <summary>賽馬</summary>
        public static readonly MiseOrderSubType AWCHB = new MiseOrderSubType()
        {
            Value = "awchb",
            OrderType = MiseOrderType.TPSport,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.HORSEBOOK),
        };

        /// <summary>鬥雞</summary>
        public static readonly MiseOrderSubType AWCSV = new MiseOrderSubType()
        {
            Value = "awcsv",
            OrderType = MiseOrderType.TPSport,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.SV388),
        };

        /// <summary>PM棋牌</summary>
        public static readonly MiseOrderSubType PMBG = new MiseOrderSubType()
        {
            Value = "pmbg",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.PMBG),
        };

        /// <summary>PM電子</summary>
        public static readonly MiseOrderSubType PMSL = new MiseOrderSubType()
        {
            Value = "pmsl",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.PMSL),
        };

        /// <summary>CQ9電子</summary>
        public static readonly MiseOrderSubType CQ9Slot = new MiseOrderSubType()
        {
            Value = "cq9slot",
            OrderType = MiseOrderType.TPSlots,
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Slot),
        };

        /// <summary> CQ9棋牌 </summary>
        public static readonly MiseOrderSubType CQ9Table = new MiseOrderSubType()
        {
            Value = "cq9table",
            OrderType = MiseOrderType.TPBoardGame,
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Table),
        };

        /// <summary> CQ9捕鱼 </summary>
        public static readonly MiseOrderSubType CQ9Fish = new MiseOrderSubType()
        {
            Value = "cq9fish",
            OrderType = MiseOrderType.TPFish,            
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Fish),
        };
    }
}