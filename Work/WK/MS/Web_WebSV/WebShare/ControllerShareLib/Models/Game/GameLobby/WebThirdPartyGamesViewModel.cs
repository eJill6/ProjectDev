using JxBackendService.Model.ViewModel.ThirdParty;

namespace ControllerShareLib.Models.Game.GameLobby
{
    public class MobileApiThirdPartyGamesViewModel
    {
        /// <summary>是否為方正型的圖片</summary>
        public bool IsSquareGameImage { get; set; }

        /// <summary>子遊戲資料陣列</summary>
        public List<GameLobbyInfo> GameLobbyInfos { get; set; }
    }

    public class WebThirdPartyGamesViewModel : MobileApiThirdPartyGamesViewModel
    {
        public int GameTabType { get; set; }

        public string SearchGameName { get; set; }

        public bool IsSelfOpenPage { get; set; }
    }
}