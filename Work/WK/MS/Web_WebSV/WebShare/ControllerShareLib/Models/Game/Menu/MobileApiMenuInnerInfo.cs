using JxBackendService.Attributes.Extensions;

namespace ControllerShareLib.Models.Game.Menu
{
    public class MobileApiMenuInnerInfo
    {
        /// <summary>產品代碼</summary>
        public string ProductCode { get; set; }

        /// <summary>本地遊戲代碼</summary>
        public string GameCode { get; set; }

        /// <summary>遠端遊戲代碼</summary>
        public string RemoteCode { get; set; }

        public string Title { get; set; }

        /// <summary>是否維護中</summary>
        public bool IsMaintaining { get; set; }

        /// <summary>未點選時的圖檔名稱</summary>
        public string FullDefaultImageUrl { get; set; }

        /// <summary>AES未點選時的圖檔名稱</summary>
        public string AESFullDefaultImageUrl { get; set; }

        /// <summary>點選時的圖檔名稱</summary>
        public string FullFocusImageUrl { get; set; }

        /// <summary>AES點選時的圖檔名稱</summary>
        public string AESFullFocusImageUrl { get; set; }
    }
}