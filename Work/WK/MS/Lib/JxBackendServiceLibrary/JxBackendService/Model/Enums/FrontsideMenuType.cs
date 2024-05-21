using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class FrontsideMenuTypeSetting : BaseIntValueModel<FrontsideMenuTypeSetting>
    {
        /// <summary> 此类型打开的游戏是否为第三方内的游戏 </summary>
        public bool IsThirdPartySubGame { get; private set; }

        public int ColsInRow { get; private set; } = 2; //熱門遊戲cols為3

        public string CardOutCssClass { get; private set; } = "card_outer"; //熱門遊戲為card_outer_hot

        private FrontsideMenuTypeSetting()
        {
            ResourceType = typeof(CommonElement);
        }

        public static readonly FrontsideMenuTypeSetting Lottery = new FrontsideMenuTypeSetting()
        {
            Value = 0,
            ResourcePropertyName = nameof(CommonElement.Lottery),
            Sort = 6,
        };

        public static readonly FrontsideMenuTypeSetting Live = new FrontsideMenuTypeSetting()
        {
            Value = 1,
            ResourcePropertyName = nameof(CommonElement.LiveAppDisplayName),
            Sort = 5,
        };

        public static readonly FrontsideMenuTypeSetting Sport = new FrontsideMenuTypeSetting()
        {
            Value = 2,
            ResourcePropertyName = nameof(CommonElement.SportAppDisplayName),
            Sort = 7,
        };

        public static readonly FrontsideMenuTypeSetting Fishing = new FrontsideMenuTypeSetting()
        {
            Value = 3,
            ResourcePropertyName = nameof(CommonElement.FishingAppDisplayName),
            Sort = 4,
        };

        public static readonly FrontsideMenuTypeSetting Slot = new FrontsideMenuTypeSetting()
        {
            Value = 4,
            ResourcePropertyName = nameof(CommonElement.Slot),
            Sort = 3,
        };

        public static readonly FrontsideMenuTypeSetting BoardGame = new FrontsideMenuTypeSetting()
        {
            Value = 5,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.BoardGame),
            Sort = 2,
        };

        public static readonly FrontsideMenuTypeSetting ESport = new FrontsideMenuTypeSetting()
        {
            Value = 6,
            ResourcePropertyName = nameof(CommonElement.ESport),
            Sort = 8,
        };

        public static readonly FrontsideMenuTypeSetting Hot = new FrontsideMenuTypeSetting()
        {
            Value = 7,
            ResourcePropertyName = nameof(CommonElement.Hot),
            Sort = 1,
            IsThirdPartySubGame = true,
            ColsInRow = 3,
            CardOutCssClass = "card_outer_hot",
        };
    }

    /// <summary>
    /// 沒有放在db的設定先用列舉紀錄
    /// </summary>
    public class FrontsideMenuSetting : BaseStringValueModel<FrontsideMenuSetting>
    {
        public PlatformProduct Product { get; private set; }

        public string CardCssClass { get; private set; }

        private FrontsideMenuSetting()
        { }

        public static FrontsideMenuSetting GetSingle(PlatformProduct product, ThirdPartySubGameCodes subGameCode)
        {
            string value = product.Value;

            if (subGameCode != null)
            {
                value += subGameCode.Value;
            }

            return GetSingle(value);
        }

        #region 棋牌

        /// <summary>瓦力遊戲</summary>
        public static FrontsideMenuSetting WLBG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.WLBG.Value,
            Product = PlatformProduct.WLBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.WLBG),
            CardCssClass = "bg_boardgame_wlbg",
        };

        /// <summary>開元棋牌</summary>
        public static FrontsideMenuSetting IMKY = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMKY.Value,
            Product = PlatformProduct.IMKY,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMKY),
            CardCssClass = "bg_boardgame_imky",
        };

        /// <summary>龍城棋牌</summary>
        public static FrontsideMenuSetting LC = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.LC.Value,
            Product = PlatformProduct.LC,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.LC),
            CardCssClass = "bg_boardgame_lc",
        };

        /// <summary>IM棋牌</summary>
        public static FrontsideMenuSetting IMBG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMBG.Value,
            Product = PlatformProduct.IMBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
            CardCssClass = "bg_boardgame_imbg",
        };

        /// <summary>PM棋牌</summary>
        public static FrontsideMenuSetting PMBG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PMBG.Value,
            Product = PlatformProduct.PMBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMBG),
            CardCssClass = "bg_boardgame_pmbg",
        };

        #endregion 棋牌

        #region 电子

        /// <summary>JDB电子</summary>
        public static FrontsideMenuSetting IMJDB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMPP.Value + ThirdPartySubGameCodes.IMJDB,
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMJDB),
            CardCssClass = "bg_slot_imjdb",
        };

        /// <summary>AG街機</summary>
        public static FrontsideMenuSetting AGYoPlay = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AG.Value + ThirdPartySubGameCodes.AGYoPlay,
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGYoPlay),
            CardCssClass = "bg_slot_agyoplay",
        };

        /// <summary>PG電子</summary>
        public static FrontsideMenuSetting PGSL = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PGSL.Value,
            Product = PlatformProduct.PGSL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
            CardCssClass = "bg_slot_pgsl",
        };

        /// <summary>SE電子</summary>
        public static FrontsideMenuSetting IMSE = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMPP.Value + ThirdPartySubGameCodes.IMSE,
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSE),
            CardCssClass = "bg_slot_imse",
        };

        /// <summary>AG电子</summary>
        public static FrontsideMenuSetting AGXin = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AG.Value + ThirdPartySubGameCodes.AGXin,
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGXin),
            CardCssClass = "bg_slot_agxin",
        };

        /// <summary>PT电游</summary>
        public static FrontsideMenuSetting IMPT = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMPT.Value,
            Product = PlatformProduct.IMPT,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
            CardCssClass = "bg_slot_impt",
        };

        /// <summary>PM電子</summary>
        public static FrontsideMenuSetting PMSL = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PMSL.Value,
            Product = PlatformProduct.PMSL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMSL),
            CardCssClass = "bg_slot_pmsl",
        };

        /// <summary>PP电子</summary>
        public static FrontsideMenuSetting IMPP = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMPP.Value + ThirdPartySubGameCodes.IMPP,
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPP),
            CardCssClass = "bg_slot_pp",
        };

        #endregion 电子

        #region 捕魚

        /// <summary>JDB捕魚</summary>
        public static FrontsideMenuSetting JDBFI = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.JDBFI.Value,
            Product = PlatformProduct.JDBFI,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.JDBFI),
            CardCssClass = "bg_fishing_jdbfi",
        };

        /// <summary>瓦力捕魚</summary>
        public static FrontsideMenuSetting WLFishing = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.WLBG.Value + ThirdPartySubGameCodes.WLFI,
            Product = PlatformProduct.WLBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.WLFishing),
            CardCssClass = "bg_fishing_wlfishing",
        };

        /// <summary>AG捕魚</summary>
        public static FrontsideMenuSetting AGFishing = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AG.Value + ThirdPartySubGameCodes.AGFishing,
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGFishing),
            CardCssClass = "bg_fishing_agfishing",
        };

        /// <summary>PM捕魚</summary>
        public static FrontsideMenuSetting OBFI = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.OBFI.Value,
            Product = PlatformProduct.OBFI,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
            CardCssClass = "bg_fishing_pmfishing",
        };

        #endregion 捕魚

        #region 真人

        /// <summary>AG真人</summary>
        public static FrontsideMenuSetting AG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AG.Value + ThirdPartySubGameCodes.AG,
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AG),
            CardCssClass = "bg_casino_ag",
        };

        /// <summary>OB(PM)真人</summary>
        public static FrontsideMenuSetting OBEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.OBEB.Value,
            Product = PlatformProduct.OBEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBEB),
            CardCssClass = "bg_casino_pmeb",
        };

        /// <summary>EVO真人</summary>
        public static FrontsideMenuSetting EVEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.EVEB.Value,
            Product = PlatformProduct.EVEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
            CardCssClass = "bg_casino_eveb",
        };

        /// <summary>歐博真人</summary>
        public static FrontsideMenuSetting ABEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.ABEB.Value,
            Product = PlatformProduct.ABEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
            CardCssClass = "bg_casino_abeb",
        };

        /// <summary>eBET真人</summary>
        public static FrontsideMenuSetting IMeBET = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMeBET.Value,
            Product = PlatformProduct.IMeBET,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
            CardCssClass = "bg_casino_imebet",
        };

        /// <summary>PT真人</summary>
        public static FrontsideMenuSetting IMPTLive = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMPT.Value + ThirdPartySubGameCodes.IMPTLive,
            Product = PlatformProduct.IMPT,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPTLIVE),
            CardCssClass = "bg_casino_imptlive",
        };

        #endregion 真人

        #region 彩票

        /// <summary>IM雙贏彩票</summary>
        public static FrontsideMenuSetting IMSG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMSG.Value,
            Product = PlatformProduct.IMSG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSG),
            CardCssClass = "bg_lottery_imsg",
        };

        /// <summary>IMVR彩票</summary>
        public static FrontsideMenuSetting IMVR = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMVR.Value,
            Product = PlatformProduct.IMVR,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
            CardCssClass = "bg_lottery_imvr",
        };

        #endregion 彩票

        #region 體育

        /// <summary>OB(PM)體育</summary>
        public static FrontsideMenuSetting OBSP = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.OBSP.Value,
            Product = PlatformProduct.OBSP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBSP),
            CardCssClass = "bg_sport_pmsp",
        };

        /// <summary>IM體育</summary>
        public static FrontsideMenuSetting IMSport = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMSport.Value,
            Product = PlatformProduct.IMSport,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
            CardCssClass = "bg_sport_imsport",
        };

        /// <summary>沙巴體育</summary>
        public static FrontsideMenuSetting Sport = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.Sport.Value,
            Product = PlatformProduct.Sport,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
            CardCssClass = "bg_sport_sport",
        };

        /// <summary>BTI體育</summary>
        public static FrontsideMenuSetting BTIS = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.BTIS.Value,
            Product = PlatformProduct.BTIS,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
            CardCssClass = "bg_sport_btis",
        };

        /// <summary>鬥雞</summary>
        public static FrontsideMenuSetting AWCSV = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AWCSP.Value + ThirdPartySubGameCodes.AWCSV,
            Product = PlatformProduct.AWCSP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AWCSV),
            CardCssClass = "bg_sport_awcsv"
        };

        /// <summary>賽馬</summary>
        public static FrontsideMenuSetting AWCHB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.AWCSP.Value + ThirdPartySubGameCodes.AWCHB,
            Product = PlatformProduct.AWCSP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AWCHB),
            CardCssClass = "bg_sport_awchb"
        };

        #endregion 體育

        #region 電競

        /// <summary>泛亞電競</summary>
        public static FrontsideMenuSetting FYES = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.FYES.Value,
            Product = PlatformProduct.FYES,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.FYES),
            CardCssClass = "bg_esports_fyes",
        };

        /// <summary>IM電競</summary>
        public static FrontsideMenuSetting IM = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IM.Value,
            Product = PlatformProduct.IM,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IM),
            CardCssClass = "bg_esports_im"
        };

        #endregion 電競
    }
}