namespace ControllerShareLib.Models.Game.GameLobby
{
    public class MobileApiGameLobbyInfoViewModel
    {
        /// <summary>上方banner圖片網址</summary>
        public string BannerFullImageUrl { get; set; }

        /// <summary>AES上方banner圖片網址</summary>
        public string AESBannerFullImageUrl { get; set; }

        /// <summary>獎金池數量,null為產品不提供</summary>
        public string? JackpotAmount { get; set; }
    }
}