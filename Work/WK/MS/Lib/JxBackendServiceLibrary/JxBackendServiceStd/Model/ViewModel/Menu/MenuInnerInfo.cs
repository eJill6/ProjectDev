using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ViewModel
{
    public class MenuInnerInfo
    {
        /// <summary>產品代碼</summary>
        public string ProductCode { get; set; }

        /// <summary>本地遊戲代碼</summary>
        public string GameCode { get; set; }

        /// <summary>遠端遊戲代碼</summary>
        public string RemoteCode { get; set; }
        
        public string MiseOrderGameID {  get; set; }

        public string Title { get; set; }

        /// <summary>圖檔資訊</summary>
        public MenuInnerIcon IconInfo { get; set; }

        /// <summary>是否維護中</summary>
        public bool IsMaintaining { get; set; }
    }
}