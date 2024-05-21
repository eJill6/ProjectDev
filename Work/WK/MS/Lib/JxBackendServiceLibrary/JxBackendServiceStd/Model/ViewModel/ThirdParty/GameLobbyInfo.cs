using JxBackendService.Interface.Model.ViewModel.ThirdParty;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class GameLobbyInfo : IGameLobbyInfo
    {
        /// <summary></summary>
        public int No { get; set; }

        /// <summary></summary>
        public string ChineseName { get; set; }

        /// <summary>行動裝置GameCode</summary>
        public string MobileGameCode { get; set; }

        /// <summary>是否為熱門遊戲</summary>
        public bool IsHot { get; set; }

        /// <summary>排序</summary>
        public int Sort { get; set; }

        //容舊API
        public string ImageUrl { get; set; }

        /// <summary>完整圖片網址</summary>
        public string FullImageUrl { get; set; }

        /// <summary>AES完整圖片網址</summary>
        public string AESFullImageUrl { get; set; }
    }
}