using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.BackSideWeb
{
    public class BackSidePageInitSetting
    {
        /// <summary>
        /// 是否顯示VIP有關的欄位 (VIP等級,身份,代理餘額,代理凍結餘額)
        /// </summary>
        public bool IsVIPInfosVisible { get; set; }

        /// <summary>
        /// 是否顯示返點有關的欄位 (日工資,返點,額外返點)
        /// </summary>
        public bool IsRebateInfosVisible { get; set; }

        #region 因為商戶不同而顯示不同文字的欄位
        /// <summary>
        /// 主賬戶餘額 顯示文字
        /// </summary>
        public string UserInfoAvailableScoresText { get; set; }

        /// <summary>
        /// 主賬戶凍結餘額 顯示文字
        /// </summary>
        public string UserInfoFreezeScoresText { get; set; }
        #endregion
    }
}
