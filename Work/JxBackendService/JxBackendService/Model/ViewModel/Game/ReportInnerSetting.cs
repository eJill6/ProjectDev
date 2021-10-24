using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Game
{
    public class ReportInnerSetting
    {
        /// <summary>
        /// 內頁中的Menu
        /// </summary>
        public List<InnerMenu> InnerMenus { get; set; }

        /// <summary>
        /// 是否顯示紅包欄位
        /// </summary>
        public bool IsHBVisible { get; set; }

        /// <summary>
        /// 是否顯示佣金欄位
        /// </summary>
        public bool IsYJVisible { get; set; }

        public string FooterMemo { get; set; }
    }

    public class InnerMenu
    {
        public string ActionName { get; set; }
        public string MenuName { get; set; }
    }
}
