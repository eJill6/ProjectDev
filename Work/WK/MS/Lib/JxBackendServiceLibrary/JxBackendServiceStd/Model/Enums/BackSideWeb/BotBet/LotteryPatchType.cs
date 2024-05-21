using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JxBackendService.Model.Enums.BackSideWeb.BotBet
{
    /// <summary>
    /// 彩種時間區間類型
    /// </summary>
    public enum LotteryPatchType
    {
        /// <summary>
        /// 一分系列
        /// </summary>
        [Description("一分")]
        OM,

        /// <summary>
        /// 極速系列
        /// </summary>
        [Description("极速")]
        JS
    }
}
