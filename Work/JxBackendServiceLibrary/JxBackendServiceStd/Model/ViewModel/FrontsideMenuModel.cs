using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel
{
    public class FrontsideMenuViewModel
    {
        public List<FrontsideMenuTypeViewModel> FrontsideMenuTypes { get; set; }

        public List<PagedFrontsideProductMenu> PagedFrontsideProductMenus { get; set; } = new List<PagedFrontsideProductMenu>();
    }

    public class PagedFrontsideProductMenu
    {
        public int MenuTypeValue { get; set; }

        public List<FrontsideProductMenu> FrontsideProductMenus { get; set; } = new List<FrontsideProductMenu>();
    }

    public class FrontsideMenuTypeViewModel
    {
        public int MenuTypeValue { get; set; }

        public string MenuTypeName { get; set; }

        public int ColsInRow { get; set; }

        public string CardOutCssClass { get; set; }

        public string MaintainanceCssClass { get; set; }

        public string IconFileName { get; set; }
    }

    public class FrontsideProductMenu
    {
        public string ProductCode { get; set; }

        public string GameCode { get; set; }

        public string RemoteCode { get; set; }

        public string ProductName { get; set; }

        public string CardCssClass { get; set; }

        public string FullImageUrl { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public bool IsMaintaining { get; set; }

        public bool IsRedirectUrl { get; set; }

        public bool IsHideHeaderWithFullScreen { get; set; }
    }
}