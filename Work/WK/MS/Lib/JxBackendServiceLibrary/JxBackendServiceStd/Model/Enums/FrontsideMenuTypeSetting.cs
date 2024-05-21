using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class FrontsideMenuTypeSetting : BaseIntValueModel<FrontsideMenuTypeSetting>
    {
        /// <summary> 此类型打开的游戏是否为第三方内的游戏 </summary>
        public bool IsThirdPartySubGame { get; private set; }

        public int ColsInRow { get; private set; } = 2;

        public string CardOutCssClass { get; private set; } = "card_outer";

        public string MaintainanceCssClass { get; private set; } = "cover_maintain";

        public string IconFileName { get; private set; }

        private FrontsideMenuTypeSetting()
        {
            ResourceType = typeof(CommonElement);
        }

        public static readonly FrontsideMenuTypeSetting Lottery = new FrontsideMenuTypeSetting()
        {
            Value = 0,
            ResourcePropertyName = nameof(CommonElement.Lottery),
            IconFileName = "icon_lottery.png",
            Sort = 6,
        };

        public static readonly FrontsideMenuTypeSetting Live = new FrontsideMenuTypeSetting()
        {
            Value = 1,
            ResourcePropertyName = nameof(CommonElement.LiveAppDisplayName),
            Sort = 5,
            IconFileName = "icon_casino.png",
        };

        public static readonly FrontsideMenuTypeSetting Sport = new FrontsideMenuTypeSetting()
        {
            Value = 2,
            ResourcePropertyName = nameof(CommonElement.SportAppDisplayName),
            Sort = 7,
            IconFileName = "icon_sport.png",
        };

        public static readonly FrontsideMenuTypeSetting Fishing = new FrontsideMenuTypeSetting()
        {
            Value = 3,
            ResourcePropertyName = nameof(CommonElement.FishingAppDisplayName),
            Sort = 4,
            IconFileName = "icon_fishing.png",
        };

        public static readonly FrontsideMenuTypeSetting Slot = new FrontsideMenuTypeSetting()
        {
            Value = 4,
            ResourcePropertyName = nameof(CommonElement.Slot),
            Sort = 3,
            IconFileName = "icon_slot.png",
        };

        public static readonly FrontsideMenuTypeSetting BoardGame = new FrontsideMenuTypeSetting()
        {
            Value = 5,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.BoardGame),
            Sort = 2,
            IconFileName = "icon_boardgame.png",
        };

        public static readonly FrontsideMenuTypeSetting ESport = new FrontsideMenuTypeSetting()
        {
            Value = 6,
            ResourcePropertyName = nameof(CommonElement.ESport),
            Sort = 8,
            IconFileName = "icon_esports.png",
        };

        public static readonly FrontsideMenuTypeSetting Hot = new FrontsideMenuTypeSetting()
        {
            Value = 7,
            ResourcePropertyName = nameof(CommonElement.Hot),
            Sort = 1,
            IsThirdPartySubGame = true,
            ColsInRow = 3,
            IconFileName = "icon_hot.png",
            CardOutCssClass = "card_outer_hot",
            MaintainanceCssClass = "cover_hotmaintain",
        };
    }

    /// <summary>
    /// 沒有放在db的設定先用列舉紀錄
    /// </summary>
    public class FrontsideMenuSetting : BaseStringValueModel<FrontsideMenuSetting>
    {
        public PlatformProduct Product { get; private set; }

        public string CardCssClass { get; private set; }

        public string CardImageName { get; private set; }

        /// <summary>自製遊戲內的圖檔名稱</summary>
        public MenuInnerIcon MenuInnerIcon { get; private set; }

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
            CardImageName = "bg_boardgame_wlbg.jpg"
        };

        /// <summary>開元棋牌</summary>
        public static FrontsideMenuSetting IMKY = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMKY.Value,
            Product = PlatformProduct.IMKY,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMKY),
            CardCssClass = "bg_boardgame_imky",
            CardImageName = "bg_boardgame_imky.jpg",
        };

        /// <summary>龍城棋牌</summary>
        public static FrontsideMenuSetting LC = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.LC.Value,
            Product = PlatformProduct.LC,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.LC),
            CardCssClass = "bg_boardgame_lc",
            CardImageName = "bg_boardgame_lc.jpg"
        };

        /// <summary>IM棋牌</summary>
        public static FrontsideMenuSetting IMBG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMBG.Value,
            Product = PlatformProduct.IMBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
            CardCssClass = "bg_boardgame_imbg",
            CardImageName = "bg_boardgame_imbg.jpg",
        };

        /// <summary>PM棋牌</summary>
        public static FrontsideMenuSetting PMBG = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PMBG.Value,
            Product = PlatformProduct.PMBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMBG),
            CardCssClass = "bg_boardgame_pmbg",
            CardImageName = "bg_boardgame_pmbg.jpg",
        };

        /// <summary>CQ9棋牌</summary>
        public static FrontsideMenuSetting CQ9Table = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.CQ9SL, ThirdPartySubGameCodes.CQ9Table),
            Product = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Table),
            CardCssClass = "bg_boardgame_cq9",
            CardImageName = "bg_boardgame_cq9.jpg",
        };

        #endregion 棋牌

        #region 电子

        /// <summary>JDB电子</summary>
        public static FrontsideMenuSetting IMJDB = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.IMPP, ThirdPartySubGameCodes.IMJDB),
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMJDB),
            CardCssClass = "bg_slot_imjdb",
            CardImageName = "bg_slot_imjdb.jpg",
        };

        /// <summary>AG街機</summary>
        public static FrontsideMenuSetting AGYoPlay = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AG, ThirdPartySubGameCodes.AGYoPlay),
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGYoPlay),
            CardCssClass = "bg_slot_agyoplay",
            CardImageName = "bg_slot_agyoplay.jpg",
        };

        /// <summary>PG電子</summary>
        public static FrontsideMenuSetting PGSL = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PGSL.Value,
            Product = PlatformProduct.PGSL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
            CardCssClass = "bg_slot_pgsl",
            CardImageName = "bg_slot_pgsl.jpg",
        };

        /// <summary>SE電子</summary>
        public static FrontsideMenuSetting IMSE = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.IMPP, ThirdPartySubGameCodes.IMSE),
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSE),
            CardCssClass = "bg_slot_imse",
            CardImageName = "bg_slot_imse.jpg",
        };

        /// <summary>AG电子</summary>
        public static FrontsideMenuSetting AGXin = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AG, ThirdPartySubGameCodes.AGXin),
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGXin),
            CardCssClass = "bg_slot_agxin",
            CardImageName = "bg_slot_agxin.jpg",
        };

        /// <summary>PT电游</summary>
        public static FrontsideMenuSetting IMPT = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.IMPT, ThirdPartySubGameCodes.IMPT),
            Product = PlatformProduct.IMPT,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
            CardCssClass = "bg_slot_impt",
            CardImageName = "bg_slot_impt.jpg",
        };

        /// <summary>PM電子</summary>
        public static FrontsideMenuSetting PMSL = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.PMSL.Value,
            Product = PlatformProduct.PMSL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMSL),
            CardCssClass = "bg_slot_pmsl",
            CardImageName = "bg_slot_pmsl.jpg",
        };

        /// <summary>PP电子</summary>
        public static FrontsideMenuSetting IMPP = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.IMPP, ThirdPartySubGameCodes.IMPP),
            Product = PlatformProduct.IMPP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPP),
            CardCssClass = "bg_slot_pp",
            CardImageName = "bg_slot_pp.jpg",
        };

        /// <summary>CQ9Slot电子</summary>
        public static FrontsideMenuSetting CQ9Slot = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.CQ9SL, ThirdPartySubGameCodes.CQ9Slot),
            Product = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9SL),
            CardCssClass = "bg_slot_cq9",
            CardImageName = "bg_slot_cq9.jpg",
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
            CardImageName = "bg_fishing_jdbfi.jpg",
        };

        /// <summary>瓦力捕魚</summary>
        public static FrontsideMenuSetting WLFishing = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.WLBG, ThirdPartySubGameCodes.WLFI),
            Product = PlatformProduct.WLBG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.WLFishing),
            CardCssClass = "bg_fishing_wlfishing",
            CardImageName = "bg_fishing_wlfishing.jpg",
        };

        /// <summary>AG捕魚</summary>
        public static FrontsideMenuSetting AGFishing = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AG, ThirdPartySubGameCodes.AGFishing),
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGFishing),
            CardCssClass = "bg_fishing_agfishing",
            CardImageName = "bg_fishing_agfishing.jpg",
        };

        /// <summary>PM捕魚</summary>
        public static FrontsideMenuSetting OBFI = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.OBFI.Value,
            Product = PlatformProduct.OBFI,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
            CardCssClass = "bg_fishing_pmfishing",
            CardImageName = "bg_fishing_pmfishing.jpg",
        };

        /// <summary>CQ9捕鱼</summary>
        public static FrontsideMenuSetting CQ9Fish = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.CQ9SL, ThirdPartySubGameCodes.CQ9Fish),
            Product = PlatformProduct.CQ9SL,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9Fish),
            CardCssClass = "bg_fishing_cq9",
            CardImageName = "bg_fishing_cq9.jpg",
        };

        #endregion 捕魚

        #region 真人

        /// <summary>AG真人</summary>
        public static FrontsideMenuSetting AG = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AG, ThirdPartySubGameCodes.AG),
            Product = PlatformProduct.AG,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AG),
            CardCssClass = "bg_casino_ag",
            CardImageName = "bg_casino_ag.jpg",
        };

        /// <summary>OB(PM)真人</summary>
        public static FrontsideMenuSetting OBEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.OBEB.Value,
            Product = PlatformProduct.OBEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBEB),
            CardCssClass = "bg_casino_pmeb",
            CardImageName = "bg_casino_pmeb.jpg",
        };

        /// <summary>EVO真人</summary>
        public static FrontsideMenuSetting EVEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.EVEB.Value,
            Product = PlatformProduct.EVEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
            CardCssClass = "bg_casino_eveb",
            CardImageName = "bg_casino_eveb.jpg",
        };

        /// <summary>歐博真人</summary>
        public static FrontsideMenuSetting ABEB = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.ABEB.Value,
            Product = PlatformProduct.ABEB,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
            CardCssClass = "bg_casino_abeb",
            CardImageName = "bg_casino_abeb.jpg",
        };

        /// <summary>eBET真人</summary>
        public static FrontsideMenuSetting IMeBET = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMeBET.Value,
            Product = PlatformProduct.IMeBET,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
            CardCssClass = "bg_casino_imebet",
            CardImageName = "bg_casino_imebet.jpg",
        };

        /// <summary>PT真人</summary>
        public static FrontsideMenuSetting IMPTLive = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.IMPT, ThirdPartySubGameCodes.IMPTLive),
            Product = PlatformProduct.IMPT,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPTLIVE),
            CardCssClass = "bg_casino_imptlive",
            CardImageName = "bg_casino_imptlive.jpg",
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
            CardImageName = "bg_lottery_imsg.jpg",
        };

        /// <summary>IMVR彩票</summary>
        public static FrontsideMenuSetting IMVR = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMVR.Value,
            Product = PlatformProduct.IMVR,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
            CardCssClass = "bg_lottery_imvr",
            CardImageName = "bg_lottery_imvr.jpg",
        };

        /// <summary>一分快三</summary>
        public static FrontsideMenuSetting LotteryOMKS = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryOMKS),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMKS),
            CardCssClass = "bg_lottery_omks",
            CardImageName = "bg_lottery_omks.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_kuaisan_default.png",
                FocusImageName = "switch_kuaisan_focus.png"
            }
        };

        /// <summary>一分快车</summary>
        public static FrontsideMenuSetting LotteryOMPK10 = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryOMPK10),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMPK10),
            CardCssClass = "bg_lottery_ompk10",
            CardImageName = "bg_lottery_ompk10.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_pk10_default.png",
                FocusImageName = "switch_pk10_focus.png"
            }
        };

        /// <summary>一分時時彩</summary>
        public static FrontsideMenuSetting LotteryOMSSC = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryOMSSC),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMSSC),
            CardCssClass = "bg_lottery_omssc",
            CardImageName = "bg_lottery_omssc.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_omssc_default.png",
                FocusImageName = "switch_omssc_focus.png"
            }
        };

        /// <summary>一分六合彩</summary>
        public static FrontsideMenuSetting LotteryOMLHC = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryOMLHC),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_OMLHC),
            CardCssClass = "bg_lottery_omlhc",
            CardImageName = "bg_lottery_omlhc.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_omlhc_default.png",
                FocusImageName = "switch_omlhc_focus.png"
            }
        };

        /// <summary>百人牛牛</summary>
        public static FrontsideMenuSetting LotteryNUINUI = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryNUINUI),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_NUINUI),
            CardCssClass = "bg_lottery_nuinui",
            CardImageName = "bg_lottery_nuinui.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_nuinui_default.png",
                FocusImageName = "switch_nuinui_focus.png"
            }
        };

        /// <summary>極速百家樂</summary>
        public static FrontsideMenuSetting LotteryJSBaccarat = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryJSBaccarat),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSBaccarat),
            CardCssClass = "bg_lottery_jsbaccarat",
            CardImageName = "bg_lottery_jsbaccarat.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_jsbaccarat_default.png",
                FocusImageName = "switch_jsbaccarat_focus.png"
            }
        };

        /// <summary>極速輪盤</summary>
        public static FrontsideMenuSetting LotteryJSLP = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryJSLP),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSLP),
            CardCssClass = "bg_lottery_jslp",
            CardImageName = "bg_lottery_jslp.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_jslp_default.png",
                FocusImageName = "switch_jslp_focus.png"
            }
        };

        /// <summary>極速魚蝦螃</summary>
        public static FrontsideMenuSetting LotteryJSYXX = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryJSYXX),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSYXX),
            CardCssClass = "bg_lottery_jsyxx",
            CardImageName = "bg_lottery_jsyxx.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_jsyxx_default.png",
                FocusImageName = "switch_jsyxx_focus.png"
            }
        };

        /// <summary>极速三公</summary>
        public static FrontsideMenuSetting LotteryJSSG = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryJSSG),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSSG),
            CardCssClass = "bg_lottery_jssg",
            CardImageName = "bg_lottery_jssg.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_jssg_default.png",
                FocusImageName = "switch_jssg_focus.png"
            }
        };

        /// <summary>极速龙虎</summary>
        public static FrontsideMenuSetting LotteryJSLH = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.Lottery, ThirdPartySubGameCodes.LotteryJSLH),
            Product = PlatformProduct.Lottery,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery_JSLH),
            CardCssClass = "bg_lottery_jslh",
            CardImageName = "bg_lottery_jslh.jpg",
            MenuInnerIcon = new MenuInnerIcon()
            {
                DefaultImageName = "switch_jslh_default.png",
                FocusImageName = "switch_jslh_focus.png"
            }
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
            CardImageName = "bg_sport_pmsp.jpg",
        };

        /// <summary>IM體育</summary>
        public static FrontsideMenuSetting IMSport = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IMSport.Value,
            Product = PlatformProduct.IMSport,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
            CardCssClass = "bg_sport_imsport",
            CardImageName = "bg_sport_imsport.jpg",
        };

        /// <summary>沙巴體育</summary>
        public static FrontsideMenuSetting Sport = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.Sport.Value,
            Product = PlatformProduct.Sport,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
            CardCssClass = "bg_sport_sport",
            CardImageName = "bg_sport_sport.jpg",
        };

        /// <summary>BTI體育</summary>
        public static FrontsideMenuSetting BTIS = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.BTIS.Value,
            Product = PlatformProduct.BTIS,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
            CardCssClass = "bg_sport_btis",
            CardImageName = "bg_sport_btis.jpg",
        };

        /// <summary>賽馬</summary>
        public static FrontsideMenuSetting AWCHB = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AWCSP, ThirdPartySubGameCodes.AWCHB),
            Product = PlatformProduct.AWCSP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AWCHB),
            CardCssClass = "bg_sport_awchb",
            CardImageName = "bg_sport_awchb.jpg",
        };

        /// <summary>鬥雞</summary>
        public static FrontsideMenuSetting AWCSV = new FrontsideMenuSetting()
        {
            Value = GetCompositValue(PlatformProduct.AWCSP, ThirdPartySubGameCodes.AWCSV),
            Product = PlatformProduct.AWCSP,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AWCSV),
            CardCssClass = "bg_sport_awcsv",
            CardImageName = "bg_sport_awcsv.jpg",
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
            CardCssClass = "bg_esports_ks",
            CardImageName = "bg_esports_ks.jpg",
        };

        /// <summary>IM電競</summary>
        public static FrontsideMenuSetting IM = new FrontsideMenuSetting()
        {
            Value = PlatformProduct.IM.Value,
            Product = PlatformProduct.IM,
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IM),
            CardCssClass = "bg_esports_im",
            CardImageName = "bg_esports_im.jpg",
        };

        #endregion 電競

        private static string GetCompositValue(PlatformProduct platformProduct, ThirdPartySubGameCodes thirdPartySubGameCode)
            => $"{platformProduct}{thirdPartySubGameCode}";
    }

    public class MenuInnerIcon
    {
        /// <summary>未點選時的圖檔名稱</summary>
        public string DefaultImageName { get; set; }

        /// <summary>AES未點選時的圖檔名稱</summary>
        public string AESDefaultImageName => DefaultImageName.ConvertToAESExtension();

        /// <summary>點選時的圖檔名稱</summary>
        public string FocusImageName { get; set; }

        /// <summary>AES點選時的圖檔名稱</summary>
        public string AESFocusImageName => FocusImageName.ConvertToAESExtension();
    }
}