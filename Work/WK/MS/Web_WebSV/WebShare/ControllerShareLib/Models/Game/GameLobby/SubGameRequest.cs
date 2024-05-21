namespace ControllerShareLib.Models.Game.GameLobby
{
    public class SubGameRequest
    {
        /// <summary cref="GameTabTypeValue">1:全部; 2:熱門; 3:最愛</summary>
        public int GameTabType { get; set; }

        /// <summary>上次獲取資料最後一筆的No</summary>
        public int? LastNo { get; set; }

        /// <summary>前端最愛的No</summary>
        public List<int>? FilterNos { get; set; }

        /// <summary>搜尋遊戲名稱</summary>
        public string? SearchGameName { get; set; }
    }

    public class GameLobbySubGameRequest : SubGameRequest
    {
        /// <summary>大廳代碼</summary>
        public string GameLobbyTypeValue { get; set; }
    }
}