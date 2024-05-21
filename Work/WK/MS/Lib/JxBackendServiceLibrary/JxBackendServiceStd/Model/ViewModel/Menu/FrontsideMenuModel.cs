using JxBackendService.Interface.Model.ViewModel;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel.Menu
{
    //public class FrontsideMenuViewModel
    //{
    //    public List<FrontsideMenuTypeViewModel> FrontsideMenuTypes { get; set; }

    //    public List<PagedFrontsideProductMenu> PagedFrontsideProductMenus { get; set; } = new List<PagedFrontsideProductMenu>();
    //}

    //public class PagedFrontsideProductMenu
    //{
    //    public int MenuTypeValue { get; set; }

    //    public List<FrontsideProductMenu> FrontsideProductMenus { get; set; } = new List<FrontsideProductMenu>();
    //}

    public class BasicMenuType {

        public int MenuTypeValue { get; set; }

        public string MenuTypeName { get; set; }
    }


    public class BaseFrontsideMenuTypeViewModel : BasicMenuType
    {
        public int ColsInRow { get; set; }

        public string IconUrl { get; set; }

        public string AESIconUrl { get; set; }
    }

    public class WebMenuTypeViewModel : BaseFrontsideMenuTypeViewModel
    {
        public string CardOutCssClass { get; set; }

        public string MaintainanceCssClass { get; set; }

        public string IconFileName { get; set; }

        public string AESIconFileName { get; set; }

        public List<FrontsideProductMenu> FrontsideProductMenus { get; set; } = new List<FrontsideProductMenu>();
    }

    public class WebGameCenterViewModel
    {
        public List<WebMenuTypeViewModel> WebMenuTypeViewModels { get; set; } = new List<WebMenuTypeViewModel>();
    }


    public class BaseFrontsideProductAESMenu : IBaseFrontsideProductAESMenu
    {
        /// <summary>產品代碼</summary>
        public string ProductCode { get; set; }

        /// <summary>本地遊戲代碼</summary>
        public string GameCode { get; set; }

        /// <summary>遠端遊戲代碼</summary>
        public string RemoteCode { get; set; }

        /// <summary>AES完整圖片路徑</summary>
        public string AESFullImageUrl { get; set; }

        /// <summary>子遊戲大廳代碼</summary>
        public string GameLobbyTypeValue { get; set; }

        /// <summary>標題</summary>
        public string Title { get; set; }
    }

    public class BaseFrontsideProductMenu : BaseFrontsideProductAESMenu, IBaseFrontsideProductMenu
    {
        /// <summary>完整圖片路徑</summary>
        public string FullImageUrl { get; set; }

        /// <summary>是否維護中</summary>
        public bool IsMaintaining { get; set; }
    }

    public class FrontsideProductMenu : BaseFrontsideProductMenu, IFrontsideProductMenu
    {
        public string CardCssClass { get; set; }

        public string CardImageName { get; set; }

        public string AESCardImageName { get; set; }

        public string GameLobbyUrl { get; set; }

        public bool IsHideHeaderWithFullScreen { get; set; }

        public MenuInnerIcon MenuInnerIcon { get; set; }
    }
}