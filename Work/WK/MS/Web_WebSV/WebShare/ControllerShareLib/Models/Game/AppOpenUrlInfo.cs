using ControllerShareLib.Interfaces.Model.Game;

namespace ControllerShareLib.Models.Game
{
    public class AppOpenUrlInfo
    {
        /// <summary>網址</summary>
        public string Url { get; set; }

        /// <summary>開啟全螢幕時是否隱藏上方標題列</summary>
        public bool IsHideHeaderWithFullScreen { get; set; }
    }

    public class CommonOpenUrlInfo : AppOpenUrlInfo
    {
        /// <summary>開啟方式</summary>
        public string OpenGameModeValue { get; set; }
    }

    public class EnterTPGameUrlInfo : CommonOpenUrlInfo, IEnterTPGameUrlInfo
    {
        /// <summary>子遊戲大廳代碼</summary>
        public string GameLobbyTypeValue { get; set; }

        /// <summary>標題</summary>
        public string Title { get; set; }
    }

    public class MobileApiEnterTPGameUrlInfo : AppOpenUrlInfo, IBaseEnterTPGameUrlInfo
    {
        public string GameLobbyTypeValue { get; set; }

        public string Title { get; set; }
    }
}