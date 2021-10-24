using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class FrontsideMenuModel
    {
        public int Type { get; set; }
        public FrontsideMenuTypes MenuType => FrontsideMenuTypes.GetSingle(Type);
        public string TypeName { get { return MenuType.Name; } set { } }
        public string EngName { get { return MenuType.EngName; } set { } }

        public string MenuCssClass { get { return MenuType.MenuCssClass; } set { } }
        public string IndexCssClass { get { return MenuType.IndexCssClass; } set { } }
        public string IndexName { get { return MenuType.IndexName; } set { } }

        public string PicName { get { return MenuType.PicName; } set { } }
        public List<FrontsideMenu> MenuItems { get; set; }
    }
}
