using System.Collections.Generic;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.HomeGameMenu
{
    public class GameMenuViewModel
    {
        public string MenuName { get; set; }

        public List<GameMenuEntranceItemViewModel> EntranceItems { get; set; }

        public List<GameMenuRecommendItemViewModel> RecommendItems { get; set; }
    }

    /// <summary>
    /// 遊戲入口項目的Model
    /// </summary>
    public class GameMenuEntranceItemViewModel
    {
        public string ProductCode { get; set; }

        public string SubGameCode { get; set; } = string.Empty;

        public string Title { get; set; }

        public string SubTitle { get; set; } = string.Empty;

        public string LogoImageUrl { get; set; }

        public string AdvertisingImageUrl { get; set; }

        public string LoadingImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool IsGameLobby
        {
            get
            {
                return GameLobbyType.IsGameLobby(ProductCode, SubGameCode);
            }

            set { }
        }

        public bool IsAppFullScreenGame { get; set; }
    }

    /// <summary>
    /// 推薦項目的Model，首頁目前只會有彩票使用到
    /// </summary>
    public class GameMenuRecommendItemViewModel
    {
        public string RecommendItemId { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}