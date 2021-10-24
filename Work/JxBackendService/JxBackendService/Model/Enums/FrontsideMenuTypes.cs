using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class FrontsideMenuTypes : BaseIntValueModel<FrontsideMenuTypes>
    {
        public string EngName { get; set; }

        public string PicName { get; set; }

        public string MenuCssClass { get; set; }

        public string IndexCssClass { get; set; }

        public string IndexName { get; set; }

        public int AppSort { get; set; }

        public bool IsThirdPartyGame { get; set; }

        public string AppDisplayName { get; set; }

        public static FrontsideMenuTypes Lottery = new FrontsideMenuTypes()
        {
            Value = 0,
            EngName = "LOTTERY",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Lottery),
            Sort = 0,
            AppSort = 0,
            IsThirdPartyGame = false,
            AppDisplayName = CommonElement.Lottery
        };

        public static FrontsideMenuTypes Live = new FrontsideMenuTypes()
        {
            Value = 1,
            EngName = "LIVE CASINO",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Live),
            Sort = 1,
            AppSort = 1,
            PicName = "Live",
            MenuCssClass = "small",
            IndexCssClass = "games_container_live",
            IndexName = "真人娱乐",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.LiveAppDisplayName
        };

        public static FrontsideMenuTypes Sport = new FrontsideMenuTypes()
        {
            Value = 2,
            EngName = "SPORTS",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Sport),
            Sort = 2,
            AppSort = 3,
            PicName = "Sport",
            MenuCssClass = "medium",
            IndexCssClass = "games_container_sport",
            IndexName = "体育投注",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.SportAppDisplayName
        };

        public static FrontsideMenuTypes Fishing = new FrontsideMenuTypes()
        {
            Value = 3,
            EngName = "FISHING",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Fishing),
            Sort = 3,
            AppSort = 4,
            PicName = "Fishing",
            MenuCssClass = "large large_col_2_piece",
            IndexCssClass = "games_container_fishing",
            IndexName = "捕鱼王",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.FishingAppDisplayName
        };

        public static FrontsideMenuTypes Slot = new FrontsideMenuTypes()
        {
            Value = 4,
            EngName = "SLOTS",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Slot),
            Sort = 4,
            AppSort = 2,
            PicName = "Slot",
            MenuCssClass = "small",
            IndexCssClass = "games_container_slots",
            IndexName = "电子投注",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.Slot
        };

        public static FrontsideMenuTypes BoardGame = new FrontsideMenuTypes()
        {
            Value = 5,
            EngName = "BOARD GAME",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.BoardGame),
            Sort = 5,
            AppSort = 5,
            PicName = "BoardGame",
            MenuCssClass = "medium",
            IndexCssClass = "games_container_boardgame",
            IndexName = "棋牌游戏",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.BoardGame
        };

        public static FrontsideMenuTypes ESport = new FrontsideMenuTypes()
        {
            Value = 6,
            EngName = "E-SPORTS",
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.ESport),
            Sort = 6,
            AppSort = 6,
            PicName = "ESport",
            MenuCssClass = "large large_col_2_piece",
            IndexCssClass = "games_container_esport",
            IndexName = "电子竞技",
            IsThirdPartyGame = true,
            AppDisplayName = CommonElement.ESport
        };

        public static List<FrontsideMenuTypes> GetAllThirdPartyGameMenu()
        {
            return GetAll().Where(x => x.IsThirdPartyGame == true).ToList();
        }
    }
}
